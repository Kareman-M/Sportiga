using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sportiga.Areas.Identity.Pages.Account;
using Sportiga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Areas.Identity.Pages
{
    [Area("Identity")]
    public class ChangePassowrd : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChangePassowrd(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;
         
        }
        [HttpGet]
        public IActionResult ChangePassowrdView()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassowrdView(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if(user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var result = await _userManager.ChangePasswordAsync(user, model.currentPassword, model.newPassword);
                if (!result.Succeeded)
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await _signInManager.RefreshSignInAsync(user);
                return View("ChangePassowrdConfirmation");
            }
            return View(model);
        }
    }
}
