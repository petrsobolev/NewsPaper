using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsPaper.Data;
using NewsPaper.Models;
using NewsPaper.Models.NewsModels;
//using Westwind.AspNetCore.Markdown;
using Markdig;
namespace NewsPaper.Controllers
{

    public class NewsController : Controller
    {
        ApplicationDbContext Context { get; set; }
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        readonly MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

        public NewsController(ApplicationDbContext Context,UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.Context = Context;
        }
        #region getForms
        public IActionResult GetFormEditNew(int EditNewId)
        {
            return View("GetFormCreateNew",Context.News.FirstOrDefault(oneNew => oneNew.Id == EditNewId));
        }
        public IActionResult GetFormCreateNew()
        {

            return View(new New());
        }
#endregion
        [HttpGet]
        public IActionResult GetNewsForTag(string tag)
        {
            
            return View("../Home/Index",Context.News.Where(oneNew=>oneNew.NewTags.Any(oneTag=>oneTag.TagId==tag)));
        }
        
        public IActionResult CreateNew(string Name, string Description, string Specialization,
            string Content,int? NewId, string UserId)
        {
            try
            { 
                if(NewId.HasValue)
                {
                    if (!EditNewHelp(Name, Description, Specialization, Content, NewId, UserId))
                        return View("Error");
                }
                else
                {
                    if (!CreateNewHelp(Name, Description, Specialization, Content, UserId))
                        return View("Error");
                }
                return Redirect("~/Home/Index");
            }
            catch(Exception ex)
            {
                return View("Shared/Error",ex);
            }
        }
       
       
        public IActionResult OpenNew(int id)
        {
            var user = userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                var userrating = Context.UserNewRatings.Where(uer => uer.NewId == id && uer.UserId == user.Id).FirstOrDefault();

                if (userrating == null)
                    ViewBag.userRating = 0;
                else
                    ViewBag.userRating = userrating.Rating;
            }
                return View("OpenNew", Context.News.FirstOrDefault(e => e.Id == id));
            
            
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


        public IActionResult AspNetSpecialization()
        {
            return View(Context.News.Where(n=>n.Specialization== "Asp.Net").Take(5));
        }
        private void SetUserRatingInViewData(int newId, string userId)
        {
            var userrating = Context.UserNewRatings.Where(uer => uer.NewId == newId && uer.UserId == userId).FirstOrDefault();
            if (userrating == null)
                ViewData.Add("user-rating", 0);
            else
                ViewData.Add("user-rating", userrating.Rating);
        }
        [HttpPost]
        public List<Tag> GetAutocomplitedList(string lineBeginning)
        {
            return Context.Tags.Where(t => t.TagId.StartsWith(lineBeginning))
                .OrderByDescending(k => k.TagId)
                .Take(5)
                .ToList();
        }
        [HttpGet]
        public IActionResult GetUserNews(string userId)
        {
           
            return View("../Manage/ManageNews/UserNewsView"
                            ,Context.News.Where(OneNew=>OneNew.UserId==userId).ToList());
        }
        [HttpPost]
        public async Task<bool> DeleteNew(int NewId)
        {
            try
            {
                Context.News.Remove(Context.News.FirstOrDefault(oneNew=>oneNew.Id==NewId));
                await Context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        #region Help
        public bool RatingVerification(int rating, ApplicationUser user, New oneNew)
        {
            if ((rating < 1 || rating > 5) || (user == null) || (oneNew == null))
                return false;
            else
                return true;
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
        private bool CreateNewHelp(string Name, string Description, string Specialization,
            string Content, string UserId)
        {
            try
            {
                New AdditionalNew = new New()
                {

                    Name = Name,
                    Description = Description,
                    Specialization = Specialization,
                    Content = Content,
                    CreationTime = DateTime.Now,
                    UserId = GetUserCreateOredit(UserId)

                };

                Context.Add(AdditionalNew);
                Context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool EditNewHelp(string Name, string Description, string Specialization,
            string Content, int? Id, string UserId)
        {
            try
            {
                var EditNew = Context.News.FirstOrDefault(oneNew => oneNew.Id == Id);
                EditNew.Name = Name;
                EditNew.Description = Description;
                EditNew.Specialization = Specialization;
                EditNew.Content = Content;
                EditNew.UserId = GetUserCreateOredit(UserId);
                Context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        private string GetUserCreateOredit(string UserId)
        {
            if (string.IsNullOrEmpty(UserId))
                return userManager.GetUserId(User);
            else
                return UserId;
        }
    }
    #endregion
}