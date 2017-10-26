using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookingSystem.Models
{
    public class DB : DbContext
    {
        public DB() : base("name=DB")
        {
            //Database.CreateIfNotExists();
            //Database.SetInitializer(new DBInit());
        }

        public DbSet<Route> Routes { get; set; }
        public virtual DbSet<Schedule> Schedules { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
    }
}