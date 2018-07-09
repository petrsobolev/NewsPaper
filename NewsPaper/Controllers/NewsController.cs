using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Data;
using NewsPaper.Models;
using NewsPaper.Models.NewsModels;
using Westwind.AspNetCore.Markdown;

namespace NewsPaper.Controllers
{
    public class NewsController : Controller
    {
        ApplicationDbContext Context { get; set; }
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public NewsController(ApplicationDbContext Context,UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.Context = Context;
        }
        public IActionResult GetFormCreateNew()
        {
            return View();
        }
        public IActionResult CreateNew(string Name, string Description, string Specialization, string Content)
        {
            try
            {
                New AdditionalNew = new New()
                {
                    Name = Name,
                    Description = Description,
                    Specialization = Specialization,
                    Content = Markdown.Parse(Content),
                    CreationTime = DateTime.Now,
                    UserId = userManager.GetUserId(User)
                };
                Context.Add(AdditionalNew);
                Context.SaveChanges();
                return View("Home/Index");
            }
            catch(Exception ex)
            {
                return View("Shared/Error",ex);
            }
        }
        public IActionResult OpenNew(int id)
        {

            
            return View("OpenNew",Context.News.FirstOrDefault(e=>e.Id==id));
        }
        [HttpPost]
        public async Task<double?> SetRating(int rating, int newId)
        {
            var user = await userManager.GetUserAsync(User);
            var oneNew = Context.News.Where(e => e.Id == newId).FirstOrDefault();
            if (RatingVerification(rating, user, oneNew))
            {
                return Math.Round((await SetNewUserRating(rating, newId, user, oneNew)).AverageRating, 1);
            }
            else
                return null;
        }

        public async Task<New> SetNewUserRating(int rating, int newId, ApplicationUser user, New oneNew)
        {
            var userNewRating = Context.UserNewRatings.Where(uer => uer.NewId == newId && uer.UserId == user.Id).FirstOrDefault();
            if (userNewRating == null)
                AddNewRating(rating, newId, user, oneNew);
            else
                UpdateOldRating(rating, newId, user, oneNew, userNewRating);
            await Context.SaveChangesAsync();
            return oneNew;
        }

        public void AddNewRating(int rating, int newId, ApplicationUser user, New oneNew)
        {
            Context.UserNewRatings.Add(new UserNewRating { NewId = newId, UserId = user.Id, Rating = rating });
            double total = oneNew.AverageRating * oneNew.VotersCount + rating;
            oneNew.VotersCount++;
            oneNew.AverageRating = total / oneNew.VotersCount;
        }

        public void UpdateOldRating(int rating, int essayId, ApplicationUser user, New oneNew, UserNewRating userNewRating)
        {
            Context.UserNewRatings.Update(userNewRating);
            double total = oneNew.AverageRating * oneNew.VotersCount - userNewRating.Rating + rating;
            oneNew.AverageRating = total / oneNew.VotersCount;
            userNewRating.Rating = rating;
            Context.UserNewRatings.Update(userNewRating);
        }

        public bool RatingVerification(int rating, ApplicationUser user, New oneNew)
        {
            if ((rating < 1 || rating > 5) || (user == null) || (oneNew == null))
                return false;
            else
                return true;
        }

        public IActionResult AspNetSpecialization()
        {
            return View(Context.News.Where(n=>n.Specialization== "Asp.Net").Take(5));
        }
    }
}