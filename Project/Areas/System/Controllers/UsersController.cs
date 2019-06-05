using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index()
        {
            UserIndexViewModel userIndexVM = new UserIndexViewModel();
            userIndexVM.ApplicationUsers = await _context.Users.Where(x=>x.DeletedAt == null).ToListAsync();
            return View(userIndexVM);
        }      
     
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUser)
        {
            if(createUser == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new ApplicationUser {
                        Email =createUser.Email,
                        PhoneNumber =createUser.PhoneNumber
                        ,UserName=createUser.Email,
                        FirstName=createUser.FirstName,
                        MiddleName=createUser.MiddleName,
                        LastName=createUser.LastName,
                        Address=createUser.Address,
                        City=createUser.City,
                        State=createUser.State,
                        ZipCode=createUser.ZipCode,
                        Country=createUser.Country,
                        StaffId=createUser.StaffId,
                        HomeEmail=createUser.HomeEmail                   
                    };
                    var result = await _userManager.CreateAsync(user, "UME_%e~!^=;2V!e");
                    if (result.Succeeded)                    
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Edit), new {UserId=user.Id});

                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
                catch
                {
                    return View(createUser);
                }
            }
            return View(createUser);
        }

        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> Edit(string UserId)
        {
            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }
            var model = new UserEditViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Country = user.Country,
                StaffId = user.StaffId,
                HomeEmail = user.HomeEmail,
                IsHomeEmailConfirmed = user.HomeEmailConfirmed
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string UserId, UserEditViewModel userEditVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userEditVM);
            }

            var user = await _context.Users.FindAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }

            var ChangeLog = new UsersChangeLog
            {
                HRID = user.HRID,
                StaffId = user.StaffId,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                State = user.State,
                ZipCode = user.ZipCode,
                Country = user.Country,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                UserName = user.UserName,
                HomeEmail = user.HomeEmail,
                HomeEmailConfirmed = user.HomeEmailConfirmed,
                CreatedAt = user.CreatedAt,
                DeletedAt = user.DeletedAt,
                UpdatedAt = DateTime.Now
            };
            await _context.UsersChangeLog.AddAsync(ChangeLog);
            await _context.SaveChangesAsync();

            user.StaffId = userEditVM.StaffId;
            user.Address = userEditVM.Address;
            user.City = userEditVM.City;
            user.State = userEditVM.State;
            user.ZipCode = userEditVM.ZipCode;
            user.Country = userEditVM.Country;
            user.HomeEmail = userEditVM.HomeEmail;
            user.CreatedAt = ChangeLog.CreatedAt;
            user.UpdatedAt = ChangeLog.UpdatedAt;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            var username = userEditVM.Username;
            if(userEditVM.Username != username)
            {
                var setUsernameResult = await _userManager.SetUserNameAsync(user, userEditVM.Username);
                if(!setUsernameResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting username for user with ID '{user.Id}'.");
                }
            }

            var email = user.Email;
            if (userEditVM.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, userEditVM.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (userEditVM.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, userEditVM.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Profile has been updated";
            return RedirectToAction(nameof(Edit),new { @UserId=UserId });
        }
      
        [HttpPost]
        public async Task<IActionResult> Delete(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var setLockout = await _userManager.SetLockoutEnabledAsync(user, true);
            if (!setLockout.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred deleting account for user with ID '{user.Id}'.");
            }
            user.DeletedAt = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            StatusMessage = "User has been deleted.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var model = new SetUserPasswordViewModel { StatusMessage = StatusMessage, Code=code, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(string UserId, SetUserPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                StatusMessage = "Password has been set.";

                return RedirectToAction(nameof(SetPassword), new { @UserId = UserId });
            }
            AddErrors(result);
            return RedirectToAction(nameof(SetPassword), new { @UserId = UserId });
        }

        [HttpGet]
        public async Task<IActionResult> Access(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }
            UserAccessViewModel userAccess = new UserAccessViewModel();
            userAccess.AccessVM = new List<AccessViewModel>();
            var Roles = await _context.Roles.ToListAsync();
            foreach (var item in Roles)
            {
                if(await _userManager.IsInRoleAsync(user,item.Name)== true)
                {
                    userAccess.AccessVM.Add(new AccessViewModel{ Id = item.Id, Name = item.Name, Selected = true });
                }
                else
                {
                    var a2 = new AccessViewModel { Id = item.Id, Name = item.Name, Selected = false };
                    userAccess.AccessVM.Add(a2);
                }
            }
            return View(userAccess);
        }

        [HttpPost]
        public async Task<IActionResult> Access(string UserId, UserAccessViewModel accessVM, IFormCollection collection)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserId}'.");
            }

            return View();
        }


        public async Task<IActionResult> BulkCreateUsers()
        {
            List<ADUImport> BulkUsers = new List<ADUImport>();
            BulkUsers = await _context.AD_Import.ToListAsync();
            List<string> Errors = new List<string>();

            foreach (var item in BulkUsers)
            {

                    var user = new ApplicationUser
                    {
                        HRID = item.HRID,
                        Address = item.Address,
                        City = item.City,
                        StaffId = item.StaffId,
                        Country = item.Country,
                        Email = item.Email,
                        FirstName = item.FirstName,
                        MiddleName = item.MiddleName,
                        LastName = item.LastName,
                        PhoneNumber = item.HomePhone,
                        State = item.State,
                        UserName = item.UserName,
                        ZipCode = item.ZipCode
                    };
                    var result = await _userManager.CreateAsync(user, item.Password);
                    if (result.Succeeded)
                    {

                        await _context.SaveChangesAsync();
                        await _userManager.AddToRoleAsync(user, "User");

                    }
                    else
                    {
                        Errors.Add(result.Errors.ToString());
                    }
            }
            return RedirectToAction(nameof(Index));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


    }
}