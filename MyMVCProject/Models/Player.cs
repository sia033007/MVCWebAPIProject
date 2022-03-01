using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyMVCProject.Models
{
    public class Player
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This field is required!")]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required(ErrorMessage ="This field is required!")]
        [MaxLength(50)]
        public string Position { get; set; }
        [Required(ErrorMessage ="This field is required!")]
        [MaxLength(50)]
        public string Nationality { get; set; }
    }
}
