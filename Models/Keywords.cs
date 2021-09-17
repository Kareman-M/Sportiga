using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Models
{
    public class Keywords
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Articles> Articles { get; set; }

        [ForeignKey("Articles")]
        public int ArticlesId { get; set; }
    }
}
