using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Models
{
    public class Articles
    {
        
        public int ID{ get; set; }
        public string Title { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
       public string Status { get; set; }

        [ForeignKey("category")]
        public int categoryId { get; set; }
        public virtual Categories category { get; set; }

        public virtual ApplicationUser ApplicationUsers { get; set; }
        [ForeignKey("ApplicationUsers")]
        public string ApplicationUsersId { get; set; }
        public virtual IEnumerable<Keywords> Keywords { get; set; }
        [ForeignKey("Keywords")]
        public int KeywordsId { get; set; }
 
    }
}
