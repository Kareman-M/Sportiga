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
    //[Authorize(Roles ="Admin")]

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
            try
            {

            ViewBag.articles = _Context.Articles.Where(d => d.categoryId == id && d.Status == "approved").OrderByDescending(s => s.Date);
                ViewBag.categories = _Context.Categories;
            ViewBag.Users = _UserManagerr.Users.ToList();
            
            return View();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> Write()
        {
            try
            {

            ViewBag.categories = _Context.Categories;
           var user = await _UserManagerr.GetUserAsync(User);
            ViewBag.User = user;
            ViewBag.Roles = await _UserManagerr.GetRolesAsync(user);
            return View();
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }
        }


        [HttpPost]
        [Route("AddArticle")]
        public async Task<IActionResult> AddArticle(IFormFile imgFile,Models.Articles articles)
        {
            try
            {

            string imgtxt = Path.GetExtension(imgFile.FileName);
            var imgSave = Path.Combine(_IWeb.WebRootPath,"images", imgFile.FileName);
           var stream = new FileStream(imgSave, FileMode.Create);
            await imgFile.CopyToAsync(stream);
            articles.Image = imgFile.FileName;
            var _articles = _Context.Articles.Add(articles);
            _Context.SaveChanges();
            return RedirectToAction("Write");
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

        }
        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            var user = await _UserManagerr.GetUserAsync(User);
            ViewBag.User = user;
            ViewBag.Roles = await _UserManagerr.GetRolesAsync(user);
            ViewBag.article = _Context.Articles.Find(id);
            return View();
        }
        public IActionResult DeleteArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            var catID = article.categoryId;
            _Context.Remove(article);
            _Context.SaveChanges();
            return RedirectToAction("Index","Articles", catID);
        }
        public IActionResult RejectedArticles()
        {
            ViewBag.Users = _UserManagerr.Users.ToList();
            ViewBag.articles = _Context.Articles.Where(s => s.Status == "reject").OrderByDescending(s => s.Date);
            return View();
        }
        public IActionResult AppendedArticles()
        {
            ViewBag.Users = _UserManagerr.Users.ToList();
            ViewBag.articles = _Context.Articles.Where(s => s.Status == "append").OrderByDescending(s=> s.Date);
            return View();
        }
        public IActionResult rejectArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            article.Status = "reject";
            _Context.SaveChanges();
            return RedirectToAction("RejectedArticles");
        }
        public IActionResult appendArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            article.Status = "append";
            _Context.SaveChanges();
            return RedirectToAction("AppendedArticles");
        }
        
        public IActionResult approveArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            article.Status = "approved";
            _Context.SaveChanges();
            return RedirectToAction("Index", new { id = article.categoryId });
        }
    }
}
