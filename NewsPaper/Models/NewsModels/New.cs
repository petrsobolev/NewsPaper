using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaper.Models.NewsModels
{
    public class New
    {
        public New()
        {
            Comments = new List<Comment>();
            NewTags = new List<NewTag>();
            UserNewRatings = new List<UserNewRating>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Specialization { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public int VotersCount { get; set; }

        public double AverageRating { get; set; }
       
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<NewTag> NewTags { get; set; }

        public virtual ICollection<UserNewRating> UserNewRatings { get; set; }

        public class DateComparer : IComparer<New>
        {
            public int Compare(New x, New y)
            {
                return y.CreationTime.CompareTo(x.CreationTime);
            }
        }
    }
}

