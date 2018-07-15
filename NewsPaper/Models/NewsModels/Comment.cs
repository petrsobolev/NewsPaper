using Microsoft.AspNetCore.Identity;
using NewsPaper.Models.ClientDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaper.Models.NewsModels
{
    public class Comment
    {

        public Comment()
        {
            UsersWhoLiked = new List<UserComment>();
        }

        public int Id { get; set; }

        public string Text { get; set; }
        public DateTime CreationDate { get; set; }


        //author
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPicturePath { get; set; }

        public virtual ICollection<UserComment> UsersWhoLiked { get; set; }

        public int NewId { get; set; }
        public New New { get; set; }
        public static explicit operator ClientComment(Comment param)
        {
            return new ClientComment()
            {
                Id = param.Id,
                CreationDate = param.CreationDate,
                Text = param.Text,
                UserName = param.UserName,
                UserPicturePath = param.UserPicturePath,
                LikesCount = param.UsersWhoLiked.Count(),
            };
        }
    }
}
