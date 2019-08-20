using InoLibrary.Models;
using InoLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InoLibrary.Controllers
{
    //Отвечает за учётную запись пользователя
    [Authorize]
    public class UsersController : Controller
    {
        UserManager<User> _userManager;

        public UsersController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        //Просмотр профиля
        public async Task<IActionResult> Profile()
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            ProfileViewModel model = new ProfileViewModel
            {
                Email = user.Email,
                FullName = user.FullName,
                Id = user.Id,
                Nickname = user.Nickname
            };
            return View(model);
        }

        //Изменение профиля
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
                if (model != null)
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
                return NotFound();
            }
            return View(model);
        }

        //Смена пароля
        [HttpGet]
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
                if (model != null)
                {
                    User user = await _userManager.FindByIdAsync(model.Id);
                    if (user != null)
                    {
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
                else
                {
                    return NotFound();
                }
            }
            return View(model);
        }
    }
}
