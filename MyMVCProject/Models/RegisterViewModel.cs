using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class RegisterViewModel
    {
        [Required]
        [MaxLength(50)]
        [Display(Name ="Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="Password")]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage ="Confirm Password and Password values must be the same")]
        [Display(Name ="Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name ="Your First Name (optional)")]
        public string FirstName { get; set; }
        [Display(Name ="Your Last Name (optional)")]
        public string LastName { get; set; }
        [Display(Name ="Your City (optional)")]
        public string City { get; set; }
        [Display(Name ="Your Profession (optional)")]
        public string Profession { get; set; }
    }
}
