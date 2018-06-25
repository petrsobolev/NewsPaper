using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaper.Models.NewsModels
{
    public class NewTag
    {
        public int NewId { get; set; }
        public New New { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
