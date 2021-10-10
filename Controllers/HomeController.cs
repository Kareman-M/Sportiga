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
            KoreMasrya.AddRange(_Context.Articles.Where(s => s.categoryId == 1 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(3));
            ViewBag.KoraMsrya = KoreMasrya;
            //
            var KoraArabya = new List<Models.Articles>();
            KoraArabya.AddRange(_Context.Articles.Where(s => s.categoryId == 2 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(3));
            ViewBag.KoraArabya = KoraArabya;
            // SecondSection
            var KoraAlamia = new List<Models.Articles>();
            KoraAlamia.AddRange(_Context.Articles.Where(s => s.categoryId == 3 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(4));
            ViewBag.KoraAlamia = KoraAlamia;
            //
            var NgomElkora = new List<Models.Articles>();
            NgomElkora.AddRange(_Context.Articles.Where(s => s.categoryId == 1002 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(4));
            ViewBag.NgomElkora = NgomElkora;
            //
            var Merkato = new List<Models.Articles>();
            Merkato.AddRange(_Context.Articles.Where(s => s.categoryId == 1003 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(4));
            ViewBag.Merkato = Merkato;
            //
            // ThirdSection
            var OtherSports = new List<Models.Articles>();
            OtherSports.AddRange(_Context.Articles.Where(s => s.categoryId == 1004 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(6));
            ViewBag.OtherSports = OtherSports;
            //
            // FourththSection
            var SportigaLite = new List<Models.Articles>();
            SportigaLite.AddRange(_Context.Articles.Where(s => s.categoryId == 1009 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(5));
            ViewBag.SportigaLite = SportigaLite;

            //
            // FifththSection
            var TqarerAnd7warat = new List<Models.Articles>();
            TqarerAnd7warat.AddRange(_Context.Articles.Where(s => s.categoryId == 1005 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(3));
            ViewBag.TqarerAnd7warat = TqarerAnd7warat;

            //
            var Aklam7ora = new List<Models.Articles>();
            Aklam7ora.AddRange(_Context.Articles.Where(s => s.categoryId == 1007 && s.Status == "approved").OrderByDescending(s => s.Date).ToList().Take(3));
            ViewBag.Aklam7ora = Aklam7ora;
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
            var keywords = _Context.Keywords.Where(k => k.ArticlesId == id);
            if(keywords != null)
            {
                ViewBag.Keywords = keywords;
            }
            ViewBag.category = _Context.Categories.Where(c => c.ID == article.categoryId).FirstOrDefault();
            ViewBag.username = _UserManagerr.Users.Where(u => u.Id == article.ApplicationUsersId).FirstOrDefault().FullName;
            ViewBag.Date = article.Date.ToString(" dddd dd / MMMM / yyyy - HH:mm", new CultureInfo("ar-AE"));
            ViewBag.prefired = _Context.Articles.Where(a=> a.ID != id).Take(8);
            List<Models.Articles> similer = new List<Models.Articles>();
            var title = article.Title.Split(" ").ToArray();
            foreach (var item in title)
            {
                if(item.Length >= 3)
                {
                    similer.Add(_Context.Articles.Where(s => s.Title.Contains(item) && s.ID != id).FirstOrDefault());
                }
            }
            if(similer != null)
            {
            ViewBag.simelar = similer.Count >= 4 ? similer.Distinct().Take(4) : similer.Distinct();
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult writer(string id)
        {
            ViewBag.username = _UserManagerr.Users.Where(u => u.Id == id).FirstOrDefault().FullName;
            ViewBag.Articles = _Context.Articles.Where(s => s.ApplicationUsersId == id && s.Status == "approved").OrderByDescending(s => s.Date).ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Search(string search)
        {
            List < Models.Articles > articles = new List<Models.Articles>();
            articles.AddRange(_Context.Articles.Where(s => s.Title.Contains(search) || s.Topic.Contains(search)).OrderByDescending(s=> s.Date).Distinct());
            ViewBag.Word = search;
            ViewBag.Articles = articles;
            return View();
        }
        
        [HttpGet]
        public IActionResult keyword(string id)
        {
            ViewBag.Word = id;
            List<Models.Articles> articles = new List<Models.Articles>();
            List<Models.Keywords> keywords = new List<Models.Keywords>();
            keywords.AddRange(_Context.Keywords.Where(s => s.Name.Contains(id)));

            foreach (var keyword in keywords)
            {
               
              articles.AddRange(_Context.Articles.Where(s=> s.ID == keyword.ArticlesId));
                 
            }
            ViewBag.articles = articles.Distinct().OrderByDescending(s => s.Date);

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
