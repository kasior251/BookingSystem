using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingSystem.Model
{
    public class Passenger
    {
        public int id { get; set; }

        [Display(Name = "First name")]
        public string firstName { get; set; }

        [Display(Name = "Last name")]
        public string lastName { get; set; }
        public List<Ticket> tickets { get; set; }

    }
}