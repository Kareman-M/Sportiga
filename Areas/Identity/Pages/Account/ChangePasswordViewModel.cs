using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sportiga.Areas.Identity.Pages.Account
{
    public class ChangePasswordViewModel 
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Passowrd")]
        public string currentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string newPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("newPassword" ,ErrorMessage ="The new password and confirmation password don't match.")]
        public string confirmPassword { get; set; }

    }
}
