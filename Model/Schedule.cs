using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BookingSystem.Model
{
    public class Schedule
    {
        public int id { get; set; }
        public long departureDate { get; set; }
        public long arrivalDate { get; set; }
        public int seatsLeft { get; set; }
        public Route route { get; set; }
        public List<Ticket> tickets { get; set; }
        public int price { get; set; }

    }
}