using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingSystem.Model
{
    public class Route
    {
        public int id { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
    }
}