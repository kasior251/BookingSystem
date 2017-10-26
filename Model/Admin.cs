using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.Model
{
    public class Admin
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username can't be empty")]
        public string username { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password  can't be empty")]
        public string password { get; set; }
    }
}
