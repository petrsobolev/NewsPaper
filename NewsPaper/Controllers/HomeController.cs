using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Models.NewsModels;
using NewsPaper.Data;

namespace NewsPaper.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext Context { get; set; }
        public HomeController(ApplicationDbContext Context)
        {
            this.Context = Context;
        }

        public IActionResult Index()
        {

            return View(Context.News.Select(news =>news).OrderBy(news=>news.CreationTime).Take(5));
        }
        public IActionResult CreateNew(New model)
        {
            
            return View();
        }

    }
}