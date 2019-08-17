using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InoLibrary.Models;

namespace InoLibrary.Controllers
{
    public class HomeController : Controller
    {
        InoLibraryDbContext db;

        public HomeController(InoLibraryDbContext context)
        {
            db = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            ViewBag.Publications = db.Publications.Include(p => p.User).Include(p => p.PublicationCategories).ToList();
            ViewBag.Categories = db.Categories.Include(c => c.PublicationCategories).ToList();
            ViewBag.PublicationCategories = db.PublicationCategories.ToList();
            //return View(db.Publications.ToList());
            return View();
        }
    }
}
