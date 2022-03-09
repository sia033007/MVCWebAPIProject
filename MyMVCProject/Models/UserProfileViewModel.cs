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
        [Display(Name ="Username")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name ="Your Email")]
        public string Email { get; set; }
        [Display(Name = "Your First Name (optional)")]
        public string FirstName { get; set; }
        [Display(Name = "Your Last Name (optional)")]
        public string LastName { get; set; }
        [Display(Name = "Your City (optional)")]
        public string City { get; set; }
        [Display(Name = "Your Profession (optional)")]
        public string Profession { get; set; }
    }
}
