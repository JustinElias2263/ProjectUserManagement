using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ADUImport> AD_Import { get; set; }
        public DbSet<UsersChangeLog> UsersChangeLog { get; set; }
        public DbSet<Approver> Approvers { get; set; }
        public DbSet<NameChangeRequest> NameChangeRequests { get; set; }
        public DbSet<NameChangeAttachements> NameChangeAttachements { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("RoleClaim"); });
        }
    }
}
