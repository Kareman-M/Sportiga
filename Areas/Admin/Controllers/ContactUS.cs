using Microsoft.AspNetCore.Mvc;
using Sportiga.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class ContactUS : Controller
    {
        private readonly ApplicationDbContext _Context;
        public ContactUS(ApplicationDbContext Context)
        {
            _Context = Context;
        }
        public IActionResult Index()
        {
            ViewBag.ContactUS = _Context.Contacts;
            return View();
        }
        
        public IActionResult Message(int id)
        {
            ViewBag.Message = _Context.Contacts.Find(id);
            return View();
        }

    }
}
