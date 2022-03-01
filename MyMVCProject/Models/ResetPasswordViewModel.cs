using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class ResetPasswordViewModel
    {
        public string userId { get; set; }
        public string token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="New Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="ConfirmPassword and Password value must be the same")]
        [Display(Name ="Confirm New Password")]
        public string ConfirmPassword { get; set; }
    }
}
