using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sportiga.Models;
using Sportiga.ViewModelComponant;
using Sportiga.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CreateRole : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _UserManagerr;
        private readonly ApplicationDbContext _Context;

        public CreateRole(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> UserManagerr, ApplicationDbContext Context)
        {
            _roleManager =roleManager;
            _UserManagerr =UserManagerr;
            _Context = Context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Create()
        {
            ViewBag.roles = _roleManager.Roles.OrderBy(s=> s.Id).ToList();
                return View();
        }

        [HttpPost]
       
        public async Task <IActionResult> Create(ProjectRole role)
        {
            try
            {

            if(role != null)
            {

                var roleExists = await _roleManager.RoleExistsAsync(role.RoleName);
                if (!roleExists)
                {
                    var result = await _roleManager.CreateAsync(new IdentityRole(role.RoleName));
                }
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("Create");

            }
            }
            catch 
            {
                return RedirectToAction("Create");

            }
        }

        [HttpGet]

        public IActionResult AssignRoleToUser()
        {
            ViewBag.Users = _UserManagerr.Users.ToList();

            ViewBag.Roles = _roleManager.Roles.ToList();

            ViewBag.UserRole = _Context.UserRoles.ToList();
            return View();
        }

        public async Task<IActionResult> SaveRole(string UserId, string roleId)
        {
            ApplicationUser user = await _UserManagerr.FindByNameAsync(UserId);
           var  role = await _roleManager.FindByNameAsync(roleId);
          
          await  _UserManagerr.AddToRoleAsync(user,role.ToString());
            return RedirectToAction("AssignRoleToUser", "CreateRole");
        }

      

    }
}
