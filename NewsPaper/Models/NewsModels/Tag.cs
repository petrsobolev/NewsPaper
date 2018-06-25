using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaper.Models.NewsModels
{
    public class Tag
    {
        public Tag()
        {
            EssayTags = new List<NewTag>();
        }

        public string TagId { get; set; }
        public int Frequency { get; set; }

        public virtual ICollection<NewTag> EssayTags { get; set; }
    }
}
