﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LabManagementSystem.Models;
using LabManagementSystem.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LabManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly LabDbContext context;
        readonly string role1 = "student";
        readonly string role2 = "admin";

        public UserController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<AppRole> _roleManager, LabDbContext _context, IWebHostEnvironment _hostingEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            hostingEnvironment = _hostingEnvironment;
            context = _context;
        }
        //GET
        public IActionResult Login()
        {
            if(signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }
        public IActionResult Register()
        {
            if(signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model, string ReturnUrl)
        {
            //TODO: Make user registration 
            if (ModelState.IsValid)
            {
                String UploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "UserImage");
                String UniqueFileName = Guid.NewGuid().ToString() + "_" + model.UserImage.FileName;
                String FilePath = Path.Combine(UploadFolder, UniqueFileName);
                model.UserImage.CopyTo(new FileStream(FilePath, FileMode.Create));
                
                // Copy data from RegisterViewModel to IdentityUser
                var user = new AppUser
                {
                    Email = model.Email,
                    Name = model.Fullname,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Email,
                    Address = model.Address,
                    UserImageName = UniqueFileName
                };


                // Store user data in AspNetUsers database table
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Automatic signin 
                    await signInManager.SignInAsync(user, isPersistent: false);
                    // var imageStore = new UserImageStore
                    // {
                    //     UserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    //     ImageName = UniqueFileName
                    // };
                    // context.Add(imageStore);
                    // await context.SaveChangesAsync();
                    //if the use has no role, give them student Role.
                    if (await roleManager.FindByNameAsync(role1) == null)
                    {
                        await roleManager.CreateAsync(new AppRole(role1));
                    }
                    await userManager.AddToRoleAsync(user, role1);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
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

        //POST: User/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    // Login is successful here, so we return now and the execution stops, meaning the bottom code never runs.
                    return RedirectToAction("index", "home");
                }
            }

            // If we get to this line, either the MoxelState isn't valid or the login failed.
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return RedirectToAction("Login", "User");
        }
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("index", "home");
            }
        }
    }
}