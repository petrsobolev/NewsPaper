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
using Newtonsoft.Json;

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
        [HttpPost]
        public async Task<bool?> PressLike(int id)
        {
            var user = await userManager.GetUserAsync(User);
            var obj = new UserComment { UserId = user.Id, CommentId = id };
            bool isCommentLikedByUser = Context.UserToLikedComments.Any(o => o.CommentId == obj.CommentId && o.UserId == user.Id);
            await LikeOrUnlikeComment(isCommentLikedByUser, obj);
            return !isCommentLikedByUser;
        }

        public async Task SaveComment(int newId, string text)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!IsSaveCommentDataValid(newId, text))
                    return;
                var user = await userManager.GetUserAsync(User);
                await AddCommentInDb(user, newId, text);
            }
        }

        public async Task<List<ClientComment>> UpdateComments(int newId, int lastCommentId)
        {
            int sum = 0;
            int step = 3000;
            int timeout = 58000;
            if (lastCommentId != 0 && newId != 0)
            {
                while (sum < timeout)
                {
                    var comments = GetNewCommentsForUser(newId, lastCommentId);
                    if (comments != null)
                        return comments;
                    await Task.Delay(step);
                    sum += step;
                }
            }
            return null;
        }

        public List<ClientComment> GetComments(int newId)
        {
            List<Comment> comments = Context.Comments.Where(c => c.NewId == newId).ToList();
            comments.Reverse();
            List<ClientComment> ret = new List<ClientComment>();
            foreach (var c in comments)
            {
                var user = Context.Users.First(u => u.Id == c.UserId);
                ret.Add(CreateClientCommentFromComment(c, user));
            }
            return ret;
        }

        private List<ClientComment> GetNewCommentsForUser(int newId, int lastCommentId)
        {
            List<Comment> comments = Context.Comments.Where(c => c.NewId == newId && c.Id > lastCommentId).ToList();
            if (comments.Count != 0)
            {
                List<ClientComment> ret = new List<ClientComment>();
                foreach (var c in comments)
                {
                    var user = Context.Users.First(u => u.Id == c.UserId);
                    ret.Add(CreateClientCommentFromComment(c, user));
                }
                return ret;
            }
            else return null;
        }

        private ClientComment CreateClientCommentFromComment(Comment c, ApplicationUser user)
        {
            return new ClientComment
            {
                Id = c.Id,
                CreationDate = c.CreationDate,
                LikesCount = Context.UserToLikedComments.Where(uc => uc.CommentId == c.Id).Count(),
                Text = c.Text,
                UserPicturePath = user.UserPhoto,
                UserName = user.UserName
            };
        }

        private async Task AddCommentInDb(ApplicationUser user, int newId, string text)
        {
            Context.Comments.Add(new Comment
            {
                UserId = user.Id,
                UserName = user.UserName,
                NewId = newId,
                Text = text,
                CreationDate = DateTime.Now
            });
            await Context.SaveChangesAsync();
        }

        private bool IsSaveCommentDataValid(int newId, string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            
            else if (Context.News.Where(e => e.Id == newId).FirstOrDefault() == null) return false;
            return true;
        }

        private async Task LikeOrUnlikeComment(bool isCommentLikedByUser, UserComment obj)
        {
            if (isCommentLikedByUser)
                Context.Remove(obj);
            else
                Context.UserToLikedComments.Add(obj);
            await Context.SaveChangesAsync();
        }

    }
}