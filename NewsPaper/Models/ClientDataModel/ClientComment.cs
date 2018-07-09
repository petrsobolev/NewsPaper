using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsPaper.Models.ClientDataModel
{
    public class ClientComment
    {
        public int Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public string UserPicturePath { get; set; }
        public int LikesCount { get; set; }
        public bool UserHasUpVoted { get; set; }
        
    }
}
