using InoLibrary.Models;
using InoLibrary.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Controllers
{
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            model.Email = user.Email;
            model.FullName = user.FullName;
            model.Id = user.Id;
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            EditUserViewModel model = new EditUserViewModel {Email = user.Email, FullName = user.FullName};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.Password))
                    {
                        user.Email = model.Email;
                        user.UserName = model.Email;
                        user.FullName = model.FullName;

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Profile", "Users");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Пароль неверный");
                    }
                }
            }
            return View(model);
        }
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    //PasswordValidator<User> passwordValidator = HttpContext.RequestServices.GetService(typeof(PasswordValidator<User>)) as PasswordValidator<User>;
                    //PasswordHasher<User> passwordHasher = HttpContext.RequestServices.GetService(typeof(PasswordHasher<User>)) as PasswordHasher<User>;
                    PasswordValidator<User> passwordValidator = new PasswordValidator<User>();
                    PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                    if (await _userManager.CheckPasswordAsync(user, model.OldPassword))
                    {
                        IdentityResult result = await passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                        if (result.Succeeded)
                        {
                            user.PasswordHash = passwordHasher.HashPassword(user, model.NewPassword);
                            await _userManager.UpdateAsync(user);
                            return RedirectToAction("Profile", "Users");
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("OldPassword", "Старый пароль введён неверно");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}
