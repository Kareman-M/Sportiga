using Microsoft.AspNetCore.Mvc;
using Sportiga.Areas.Identity.Pages.Account;
using Sportiga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Sportiga.Areas.Identity.Pages
{
    [Area("Identity")]
    [Authorize(Roles ="Admin")]
    public class ForgetPassword : Controller 
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgetPassword(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
          )
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }
        [HttpGet]
        public IActionResult forgetPassowrdView()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> ForgetPassowrdView(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email.Trim());
                if (user == null)
                {
                    return new ContentResult() { Content = "Can't Find user with this Email.", ContentType = "Application/Javascript" };


                }
                await _userManager.RemovePasswordAsync(user);
               var result =  await _userManager.AddPasswordAsync(user, model.newPassword );
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

            }
            return View(model);

        }
    }
}

