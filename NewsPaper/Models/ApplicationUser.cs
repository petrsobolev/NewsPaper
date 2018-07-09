using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using NewsPaper.Models.NewsModels;

namespace NewsPaper.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            News = new List<New>();
            LikedComments = new List<UserComment>();
            UserNewRatings = new List<UserNewRating>();
        }
        [Display(Name="UserPhoto")]
        public string UserPhoto { get; set; }
        public bool IsBlocked { get; set; }

        public virtual ICollection<New> News { get; set; }
        public virtual ICollection<UserComment> LikedComments { get; set; }
        public virtual ICollection<UserNewRating> UserNewRatings { get; set; }
       
    }
}
