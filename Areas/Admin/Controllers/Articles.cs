using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportiga.Data;
using Sportiga.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]

    public class Articles : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _UserManagerr;
        private readonly ApplicationDbContext _Context;
        private readonly IWebHostEnvironment _IWeb;

        public Articles(IWebHostEnvironment IWeb, RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> UserManagerr, ApplicationDbContext Context, UserManager<ApplicationUser> userManagerr)
        {
            _roleManager = roleManager;
            _Context = Context;
            _UserManagerr = userManagerr;
            _IWeb = IWeb;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            ViewBag.articles = _Context.Articles.Where(d => d.categoryId == id && d.Status == "approved").Select(s => s).ToList();
            ViewBag.Users = _UserManagerr.Users.ToList();
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Write()
        {
            ViewBag.categories = _Context.Categories;
           var user = await _UserManagerr.GetUserAsync(User);
            ViewBag.User = user;
            ViewBag.Roles = await _UserManagerr.GetRolesAsync(user);
            return View();
        }


        [HttpPost]
        [Route("AddArticle")]
        public async Task<IActionResult> AddArticle(IFormFile imgFile,Models.Articles articles)
        {
            string imgtxt = Path.GetExtension(imgFile.FileName);
            var imgSave = Path.Combine(_IWeb.WebRootPath,"images", imgFile.FileName);
           var stream = new FileStream(imgSave, FileMode.Create);
            await imgFile.CopyToAsync(stream);
            articles.Image = imgFile.FileName;
            var _articles = _Context.Articles.Add(articles);
            //    new Models.Articles { 
                
            //});
            _Context.SaveChanges();
            return RedirectToAction("Write");

        }
    }
}
