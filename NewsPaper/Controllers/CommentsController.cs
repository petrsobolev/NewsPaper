using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Data;
using NewsPaper.Models;
using NewsPaper.Models.ClientDataModel;
using NewsPaper.Models.NewsModels;

namespace NewsPaper.Controllers
{
    public class CommentsController : Controller
    {

        private ApplicationDbContext Context;
        private UserManager<ApplicationUser> userManager;

        public CommentsController(ApplicationDbContext Context, UserManager<ApplicationUser> userManager)
        {
            this.Context = Context;
            this.userManager = userManager;
        }

        public string GetComments(int newId)
        {

            List<ClientComment> tempComments = Context.Comments.Where(c => c.NewId == newId).ToList();
        }
    }
}