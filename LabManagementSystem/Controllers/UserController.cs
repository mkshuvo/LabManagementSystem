using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabManagementSystem.Models;
using LabManagementSystem.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LabManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly LabDbContext context;
        readonly string role1 = "student";
        readonly string role2 = "admin";

        public UserController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, LabDbContext _context)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            context = _context;
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            //TODO: Make user registration 
            if (ModelState.IsValid)
            {

                // Copy data from RegisterViewModel to IdentityUser
                var user = new AppUser
                {
                    Email = model.Email,
                    Name = model.Fullname,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email
                };

                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Automatic signin 
                    await signInManager.SignInAsync(user, isPersistent: false);
                    //if the use has no role, give them student Role.
                    if (await roleManager.FindByNameAsync(role1) == null)
                    {
                        await roleManager.CreateAsync(new AppRole(role1));
                    }
                    await userManager.AddToRoleAsync(user, role1);

                    return RedirectToAction("index", "home");
                }

                // If there are any errors, add them to the ModelState object
                // which will be displayed by the validation summary tag helper
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

    }
}
