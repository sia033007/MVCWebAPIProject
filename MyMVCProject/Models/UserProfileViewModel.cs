using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class UserProfileViewModel
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name ="Your email")]
        public string Email { get; set; }
        [Display(Name = "Your first name (optional)")]
        public string FirstName { get; set; }
        [Display(Name = "Your last name (optional)")]
        public string LastName { get; set; }
        [Display(Name = "Your city (optional)")]
        public string City { get; set; }
        [Display(Name = "Your profession (optional)")]
        public string Profession { get; set; }
    }
}
