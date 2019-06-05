using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;

namespace Project.Areas.System.Controllers
{
    [Area("System")]
    [Authorize(Roles = "Administrator, System")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public DashboardController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {


                        

            ViewBag.UsersCount = await _context.Users.Where(x => x.DeletedAt == null).AsNoTracking().CountAsync();
            var Approvers = await _userManager.GetUsersInRoleAsync("HR");
            ViewBag.ApproversCount = Approvers.Count();
            return View();
        }
    }
}