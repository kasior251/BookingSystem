using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookingSystem.Model
{
    public class Ticket
    {
        public string id { get; set; }

        public List<Passenger> passengers { get; set; }

        public Schedule schedule { get; set; }
    }
}