using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Models
{
    public class ArticleWriter
    {
        public virtual IEnumerable<Articles> Articles { get; set; }
        [ForeignKey("Articles")]
        public int ArticlesId { get; set; }

       

    }
}
