using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Models
{
    
        public class ApplicationUser : IdentityUser
        {
         public string FullName { get; set; }
        public DateTime? RegisterDate { get; set; }
           
         public string ContactNumber { get; set; }
        public List<Articles> Articles { get; set; }

    }
}
