using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Data;
using Project.Models;
using Project.Services;

namespace Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider, ApplicationDbContext _context)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("System", "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
                routes.MapRoute("HR", "{area:exists}/{controller=Requests}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}");
            });
            //CreateRoles(serviceProvider, _context).Wait();
        }


        private async Task CreateRoles(IServiceProvider serviceProvider, ApplicationDbContext _context)
        {

            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string email = "justin-elias@outlook.com";
            string[] roleNames = { "Administrator", "System", "HR", "User" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            // find the user with the admin email 
            var user = await UserManager.FindByEmailAsync("Justin.Elias@ecisd.us");
            // check if the user exists
            if (user == null)
            {
                var poweruser = new ApplicationUser
                {
                    Email = email,
                    UserName = "Justin-Elias",
                    Address = "123 W Waterworld",
                    City = "Pharr",
                    State = "TX",
                    Country = "United States",
                    ZipCode = "78577",
                    FirstName = "Justin",
                    MiddleName = "A",
                    LastName = "Elias",
                    HRID = Guid.NewGuid().ToString(),
                    StaffId = "315464",
                    PhoneNumber = "956-545-5451"
                };

                var createPowerUser1 = await UserManager.CreateAsync(poweruser, "Password01!");
                var RemoveLockout1 = await UserManager.SetLockoutEnabledAsync(poweruser, false);
                if (createPowerUser1.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Administrator");
                    await UserManager.AddToRoleAsync(poweruser, "System");
                    await UserManager.AddToRoleAsync(poweruser, "HR");
                    await UserManager.AddToRoleAsync(poweruser, "User");
                }
            }
        }
        internal static object CreateRoles(ServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }


    }
}
