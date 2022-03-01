using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebAPIProject.Model
{
    public class UserDto
    {
        [Required (ErrorMessage ="This field is required!")]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        [MaxLength(50)]
        public string EmailAddress { get; set; }
    }
}
