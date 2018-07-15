using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Models.NewsModels;
using NewsPaper.Data;
using Microsoft.AspNetCore.Identity;
using NewsPaper.Models;

namespace NewsPaper.Controllers
{
    public class HomeController : Controller
    {
        private  ApplicationDbContext Context { get; set; }
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ApplicationDbContext Context, UserManager<ApplicationUser> userManager)
        {
            this.Context = Context;
            this.userManager = userManager;
        }
        
        public IActionResult Index(string SpecializationArg)
        {
            
                IQueryable<New> model;
            if (string.IsNullOrEmpty(SpecializationArg))
            {
                model = Context.News.Select(n => n).OrderBy(n => n.CreationTime).Take(5);
                ViewBag.Specialization = "Последние";

            }
            else
            {
                model = Context.News.Select(n => n)
                    .Where(n => n.Specialization == SpecializationArg)
                    .OrderByDescending(n => n.CreationTime)
                    .Take(5);
                ViewBag.Specialization = model.First().Specialization;
            }

                Dictionary<string, string> UserPhotos = new Dictionary<string, string>();
                foreach (var user in userManager.Users)
                    UserPhotos.Add(user.Id, user.UserPhoto);
                ViewBag.UserPhotos = UserPhotos;

                return View(model);
            
            
        }
        
        public IActionResult CreateNew(New model)
        {
            
            return View();
        }

    }
}