using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Data;
using NewsPaper.Models;
using NewsPaper.Models.NewsModels;
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
        [AllowAnonymous]
        [HttpPost]
        public IActionResult StartSearch(string Data)
        {
            List<New> NewsList = GetNewsBySearchNameAndContent(Data);
            List<New> result = NewsList.Union(GetNewsSerachedByName(Data)).ToList();
            ViewData.Add("news", result);
            return View("SearchResult");
        }

        [HttpGet]
        public IActionResult TagsSearch(string Data)
        {
            var NewsTags = Context.NewToTags.Where(e => e.TagId == Data).ToList();
            List<New> result = new List<New>();
            foreach (var newTag in NewsTags)
                result.Add(Context.News.Where(e => e.Id == newTag.NewId).FirstOrDefault());
            ViewData.Add("news", result);
            return View("SearchResult");
        }

        private List<New> GetNewsSerachedByName(string data)
        {
            var comments = Context.Comments.Where(с => с.Text.Contains(data)).ToList();
            List<New> NewsList = new List<New>();
            foreach (var comment in comments)
                NewsList.Add(Context.News.Where(e => e.Id == comment.NewId).First());
            return NewsList;
        }

        private List<New> GetNewsBySearchNameAndContent(string data)
        {
            return Context.News.Where(
                    oneNew =>
                    oneNew.Content.Contains(data) ||
                    oneNew.Name.Contains(data)).ToList();
        }
    }
}

