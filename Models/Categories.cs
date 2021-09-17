using System.Collections.Generic;

namespace Sportiga.Models
{
    public class Categories
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual List<Articles> Articles { get; set; }
    }
}