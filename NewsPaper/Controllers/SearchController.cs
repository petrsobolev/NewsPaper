using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Data;
using NewsPaper.Models;

namespace NewsPaper.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDbContext Context;
        private readonly UserManager<ApplicationUser> UserManager;

        public SearchController(ApplicationDbContext Context,UserManager<ApplicationUser> UserManager)
        {
            this.Context = Context;
            this.UserManager = UserManager;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
