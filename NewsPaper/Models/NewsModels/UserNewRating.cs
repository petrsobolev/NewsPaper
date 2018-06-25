using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaper.Models.NewsModels
{
    public class UserNewRating
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int NewId { get; set; }
        public New New { get; set; }

        public double Rating { get; set; }
    }
}
