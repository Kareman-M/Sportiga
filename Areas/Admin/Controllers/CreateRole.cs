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
using System.Security.Claims;
using iTextSharp.tool.xml.html;
using System.Text;

namespace WebApplication2.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CreateRole : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _UserManagerr;
        private readonly ApplicationDbContext _Context;

        public CreateRole(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> UserManagerr, ApplicationDbContext Context)
        {
            _roleManager = roleManager;
            _UserManagerr = UserManagerr;
            _Context = Context;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Create()
        {
            ViewBag.roles = _roleManager.Roles.OrderBy(s => s.Id).ToList();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(ProjectRole role)
        {
            try
            {

                if (role != null)
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
            var role = await _roleManager.FindByNameAsync(roleId);

            await _UserManagerr.AddToRoleAsync(user, role.ToString());
            return RedirectToAction("AssignRoleToUser", "CreateRole");
        }


        public async Task<IActionResult> RemoveRole(string userid, string roleid)
        {
            ApplicationUser user = _UserManagerr.Users.Where(u => u.Id == userid).FirstOrDefault();
            var role = _roleManager.Roles.Where(r => r.Id == roleid).FirstOrDefault();
            await _UserManagerr.RemoveFromRoleAsync(user, role.ToString());
            return RedirectToAction("AssignRoleToUser", "CreateRole");

        }

        [HttpGet]
        public IActionResult find(string id)
        {
            var user = _UserManagerr.Users.Where(u => u.Id == id).FirstOrDefault();

            return Ok(user);

        }

        [HttpPost]
        [Route("updateUSer")]
        public IActionResult updateUSer(string FullName, string id)
        {
            var user = _UserManagerr.Users.Where(u => u.Id == id).FirstOrDefault();

            user.FullName = FullName;
            _Context.SaveChanges();
            return RedirectToAction("AssignRoleToUser", "CreateRole");

        }

        [HttpPost]
        [Route("deleteUser")]
        public IActionResult deleteUser(string id)
        {
            try
            {

                ClaimsPrincipal currentUser = this.User;
                var currentUserName = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                var curntuser = _UserManagerr.Users.Where(s => s.Id == currentUserName).FirstOrDefault().FullName;

                var user = _UserManagerr.Users.Where(u => u.Id == id).FirstOrDefault();
                if (user.Id == currentUserName)
                {
                    var sb = new StringBuilder();
                    sb.Append("alert('denied')");
                    return new ContentResult() { Content = "Can't delete user in use" , ContentType= "Application/Javascript"}  ;

                }
                else
                {
                    _Context.Remove(user);
                _Context.SaveChanges();
                return RedirectToAction("AssignRoleToUser", "CreateRole");
                }
            }
            catch (Exception e)
            {
                return NotFound();
            }

        }
    }
}