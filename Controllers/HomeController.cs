using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sportiga.Data;
using Sportiga.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly UserManager<ApplicationUser> _UserManagerr;

        public HomeController( ApplicationDbContext Context, UserManager<ApplicationUser> UserManagerr)
        {
            _Context = Context;
            _UserManagerr = UserManagerr;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var categories = _Context.Categories.ToList();
            var lastArticles = new List<Articles>();
            // First section "Slider" 
            for (int i = 0; i < categories.Count; i++)
            {
               lastArticles.Add(_Context.Articles.Where(s => s.categoryId == categories[i].ID && s.Status== "approved").OrderByDescending(s=>s.Date).First());
            }
            ViewBag.lastArticles = lastArticles;
            // Sections
            //FirstSection
            var KoreMasrya = new List<Models.Articles>();
            KoreMasrya.AddRange(_Context.Articles.Where(s => s.categoryId == 1).OrderByDescending(s => s.Date).ToList().Take(3));
            ViewBag.KoraMsrya = KoreMasrya;
            //
            var KoraArabya = new List<Models.Articles>();
            KoraArabya.AddRange(_Context.Articles.Where(s => s.categoryId == 2).OrderByDescending(s => s.Date).ToList().Take(3));
            ViewBag.KoraArabya = KoraArabya;
            // SecondSection
            var KoraAlamia = new List<Models.Articles>();
            KoraAlamia.AddRange(_Context.Articles.Where(s => s.categoryId == 3).OrderByDescending(s => s.Date).ToList().Take(4));
            ViewBag.KoraAlamia = KoraAlamia;
            //
            var NgomElkora = new List<Models.Articles>();
            NgomElkora.AddRange(_Context.Articles.Where(s => s.categoryId == 1002).OrderByDescending(s => s.Date).ToList().Take(4));
            ViewBag.NgomElkora = NgomElkora;
            //
            var Merkato = new List<Models.Articles>();
            Merkato.AddRange(_Context.Articles.Where(s => s.categoryId == 1003).OrderByDescending(s => s.Date).ToList().Take(4));
            ViewBag.Merkato = Merkato;
            //
            // ThirdSection
            var OtherSports = new List<Models.Articles>();
            OtherSports.AddRange(_Context.Articles.Where(s => s.categoryId == 1004).OrderByDescending(s => s.Date).ToList().Take(6));
            ViewBag.OtherSports = OtherSports;


            return View();
        }

        [HttpGet]
        public IActionResult Categories(int id)
        {
            ViewBag.category = _Context.Categories.Find(id).Name;
            ViewBag.Articles = _Context.Articles.Where(s => s.categoryId == id && s.Status == "approved").OrderByDescending(s=> s.Date).ToList();

            return View();
        }

        
         [HttpGet]
        public IActionResult essay(int id)
        {
            var article = _Context.Articles.Find(id);
            ViewBag.article = article;
            ViewBag.category = _Context.Categories.Where(c => c.ID == article.categoryId).FirstOrDefault();
            ViewBag.username = _UserManagerr.Users.Where(u => u.Id == article.ApplicationUsersId).FirstOrDefault().FullName;
            ViewBag.Date = article.Date.ToString(" dddd dd / MMMM / yyyy - HH:mm", new CultureInfo("ar-AE"));
            ViewBag.prefired = _Context.Articles.Take(8);
            return View();
        }
        
        [HttpGet]
        public IActionResult writer(string id)
        {
            ViewBag.username = _UserManagerr.Users.Where(u => u.Id == id).FirstOrDefault().FullName;
            ViewBag.Articles = _Context.Articles.Where(s => s.ApplicationUsersId == id && s.Status == "approved").OrderByDescending(s => s.Date).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
