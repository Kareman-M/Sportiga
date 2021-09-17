using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportiga.Data;
using Sportiga.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class Dashboard : Controller
    {
        private ApplicationDbContext context;
        public Dashboard(ApplicationDbContext _context)
        {

            context = _context;
        }
     
        // Categories (sections) crud operations with views
        #region Categories
        [HttpGet]
        public IActionResult Sections()
        {
            ViewBag.categories = context.Categories;
            return View();
        }
        [HttpPost]
        [Route("createSection")]
        public IActionResult createSection(Categories category)
        {
            var cat = context.Categories.Add(category);
            context.SaveChanges();
            return RedirectToAction("Sections");

        }
        [HttpGet]

        public IActionResult find(int id)
        {
            var cat = context.Categories.Find(id);

            return Ok(cat);

        }


        [HttpPost]
        [Route("updateSection")]
        public IActionResult updateSection(string name, int id)
        {
            var cat = context.Categories.Find(id);

            cat.Name = name;
            context.SaveChanges();
            return RedirectToAction("Sections");

        }


        [HttpPost]
        [Route("deleteSection")]
        public IActionResult deleteSection(int id)
        {
            try
            {

                var cat = context.Categories.Find(id);
                context.Remove(cat);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return RedirectToAction("Sections");

        }
        #endregion

        // Social Media crud operations with views
        #region SocialMedia
        [HttpGet]
        public IActionResult SocialMedia()
        {
            ViewBag.Social = context.Socialmedias.ToList();
            return View();
        }
        [HttpPost]
        [Route("createSocialMedia")]
        public IActionResult createSocialMedia(Socialmedia _Socical)
        {
            var social = context.Socialmedias.Add(_Socical);
            context.SaveChanges();
            return RedirectToAction("SocialMedia");

        }
        [HttpGet]

        public IActionResult findSocialMedia(int id)
        {
            var social = context.Socialmedias.Find(id);

            return Ok(social);

        }


        [HttpPost]
        [Route("updateSocialMedia")]
        public IActionResult updateSocialMedia(string name, string link, int id)
        {
            var social = context.Socialmedias.Find(id);

            social.Name = name;
            social.Link = link;
            context.SaveChanges();
            return RedirectToAction("SocialMedia");

        }


        [HttpPost]
        [Route("deleteSocialMedia")]
        public IActionResult deleteSocialMedia(int id)
        {
            try
            {

                var social = context.Socialmedias.Find(id);
                context.Remove(social);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                return NotFound();
            }
            return RedirectToAction("SocialMedia");

        }
        #endregion

    }
}
