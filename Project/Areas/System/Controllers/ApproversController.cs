using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Areas.System.Models;
using Project.Data;
using Project.Models;

namespace Project.Areas.System.Controllers
{
    [Area("System")]
    [Authorize(Roles = "Administrator, System")]
    public class ApproversController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ApproversController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApproverViewModel approverVM = new ApproverViewModel();
            var Approvers = await _userManager.GetUsersInRoleAsync("HR");
            approverVM.Approver = Approvers;
            approverVM.UserList = await _context.Users.Where(x => x.DeletedAt == null && !Approvers.Any(a=>a.Id==x.Id)).ToListAsync();            
            return View(approverVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddApprover(string UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }

           await _userManager.AddToRoleAsync(user, "HR");

           return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }

            await _userManager.RemoveFromRoleAsync(user, "HR");

            return RedirectToAction(nameof(Index));
        }
    }
}