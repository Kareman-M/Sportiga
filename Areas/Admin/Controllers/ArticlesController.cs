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

    public class ArticlesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _UserManagerr;
        private readonly ApplicationDbContext _Context;
        private readonly IWebHostEnvironment _IWeb;

        public ArticlesController(IWebHostEnvironment IWeb, RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> UserManagerr, ApplicationDbContext Context, UserManager<ApplicationUser> userManagerr)
        {
            _roleManager = roleManager;
            _Context = Context;
            _UserManagerr = userManagerr;
            _IWeb = IWeb;
        }
        [Authorize(Roles ="Admin")]
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
        [Authorize]
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

        [Authorize]
        [HttpPost]
        [Route("AddArticle")]
        public async Task<IActionResult> AddArticle(IFormFile imgFile,Models.Articles articles)
        {
            try
            {
            string imgtxt = Path.GetExtension(imgFile.FileName);
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
            var imgSave = Path.Combine(_IWeb.WebRootPath,"images", uniqueFileName);
           var stream = new FileStream(imgSave, FileMode.Create);
            await imgFile.CopyToAsync(stream);
            articles.Image = uniqueFileName;
            var _articles = _Context.Articles.Add(articles);
            _Context.SaveChanges();
                return RedirectToAction("Keywords", "Articles", new { id = articles.ID });
            }
            catch (Exception e)
            {
                return Ok(e.Message);
            }

        }

        [Authorize(Roles = "Admin,Desk")]
        public async Task<IActionResult> EditArticle(int id )
        
        
        {
            if (id == 0)
            {
                return NotFound();
            }

            var articles =  _Context.Articles.Where(s => s.ID == id).FirstOrDefault();
            if (articles == null)
            {
                return NotFound();
            }
            ViewBag.Image = articles.Image;
            ViewBag.categories = _Context.Categories;
            var user = await _UserManagerr.FindByIdAsync(articles.ApplicationUsersId);
            ViewBag.User = user;
            ViewBag.articles = articles;
            ViewBag.catogry = _Context.Categories.Find(articles.categoryId);
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Desk")]
        public async Task<IActionResult> Edit( IFormFile imgFile, Models.Articles articles)
        {
            var ExistingArticle = _Context.Articles.Find(articles.ID);
            if(imgFile != null)
            {
                string uploadsFolder = Path.Combine(_IWeb.WebRootPath, "images");
                var path = Path.Combine(_IWeb.WebRootPath, "images", ExistingArticle.Image);

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    string imgtxt = Path.GetExtension(imgFile.FileName);
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imgFile.FileName;
                    var imgSave = Path.Combine(_IWeb.WebRootPath, "images", uniqueFileName);
                    var stream = new FileStream(imgSave, FileMode.Create);
                    await imgFile.CopyToAsync(stream);
                    ExistingArticle.Image = uniqueFileName;
                }
            }
            ExistingArticle.Title = articles.Title;
            ExistingArticle.Topic = articles.Topic;
            ExistingArticle.categoryId = articles.categoryId;
            _Context.SaveChanges();
            return RedirectToAction("EditKeywords", "Articles", new { id = articles.ID });
        }

        [Authorize(Roles = "Admin,Desk")]
        [HttpGet]
        public async Task<IActionResult> ViewArticle(int id)
        {
            var user = await _UserManagerr.GetUserAsync(User);
            ViewBag.User = user;
            ViewBag.Roles = await _UserManagerr.GetRolesAsync(user);
            ViewBag.article = _Context.Articles.Find(id);
            ViewBag.Keywords = _Context.Keywords.Where(k => k.ArticlesId == id);
            return View();
        }
        [Authorize(Roles = "Admin,Desk")]
        public IActionResult DeleteArticle(int id)
        {
            var keywords = _Context.Keywords.Where(k => k.ArticlesId == id);
            _Context.RemoveRange(keywords);
            _Context.SaveChanges();
            var article = _Context.Articles.Find(id);
            var catID = article.categoryId;
            _Context.Remove(article);
            _Context.SaveChanges();
            return RedirectToAction("Index","Articles", catID);
        }

        [Authorize(Roles = "Admin,Desk")]
        public IActionResult RejectedArticles()
        {
            ViewBag.Users = _UserManagerr.Users.ToList();
            ViewBag.articles = _Context.Articles.Where(s => s.Status == "reject").OrderByDescending(s => s.Date);
            return View();
        }
        [Authorize(Roles = "Admin,Desk")]
        public IActionResult AppendedArticles()
        {
            ViewBag.Users = _UserManagerr.Users.ToList();
            ViewBag.articles = _Context.Articles.Where(s => s.Status == "append").OrderByDescending(s=> s.Date);
            return View();
        }
        [Authorize(Roles = "Admin,Desk")]
        public IActionResult rejectArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            article.Status = "reject";
            _Context.SaveChanges();
            return RedirectToAction("RejectedArticles");
        }
        [Authorize(Roles = "Admin,Desk")]
        public IActionResult appendArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            article.Status = "append";
            _Context.SaveChanges();
            return RedirectToAction("AppendedArticles");
        }

        [Authorize(Roles = "Admin,Desk")]
        public IActionResult approveArticle(int id)
        {
            var article = _Context.Articles.Find(id);
            article.Status = "approved";
            _Context.SaveChanges();
            return RedirectToAction("Index","Home" ,  new { area ="Admin"});
        }

        #region Keywords
        [Authorize]
        public IActionResult Keywords(int id)
        {
            ViewBag.Article = _Context.Articles.Find(id);
            ViewBag.ArticleID = id;
            return View();
        }

        [Authorize]
        public IActionResult AddKeywords( [FromBody]List<Keywords> keywords)
        {
            try
            {

            _Context.Keywords.AddRange(keywords);
            _Context.SaveChanges();
            
            }
            catch
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home", new { area = "Admin" });

        }

        [Authorize(Roles = "Admin,Desk")]

        public IActionResult EditKeywords(int id)
        {
            ViewBag.keywords = _Context.Keywords.Where(k => k.ArticlesId == id);
            ViewBag.articleID = id;
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Desk")]

        public IActionResult find(int id)
        {
            var keyword = _Context.Keywords.Find(id);

            return Ok(keyword);

        }
        [Authorize(Roles = "Admin,Desk")]
        [HttpPost]
        [Route("updateKeyword")]
        public IActionResult updateKeyword(string name, int id)
        {
            var keyword = _Context.Keywords.Find(id);

            keyword.Name = name;
            _Context.SaveChanges();
            return Redirect(HttpContext.Request.Headers["Referer"]);
        }
        [Authorize(Roles = "Admin,Desk")]
        public IActionResult DeleteKeyword(int id)
        {
            var keyword = _Context.Keywords.Find(id);
            _Context.Keywords.Remove(keyword);
            _Context.SaveChanges();
            return Redirect(HttpContext.Request.Headers["Referer"]);

        }
        [Authorize(Roles = "Admin,Desk")]
        [HttpPost]
        [Route("CreateKeyword")]
        public IActionResult CreateKeyword(int id , string Name)
        {

            var Keyword = _Context.Keywords.Add(new Models.Keywords { 
                Name= Name,
                ArticlesId = id
            });
            _Context.SaveChanges();
            return Redirect(HttpContext.Request.Headers["Referer"]);

        }

        #endregion
    }
}
