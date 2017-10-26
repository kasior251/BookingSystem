using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DAL
{
    public class Admins
    {
        [Key]
        public string username { get; set; }
        [Required(ErrorMessage = "Password  can't be empty")]
        public byte[] password { get; set; }
    }

    public class Routes
    {
        public int id { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
    }

    public class Schedules
    {
        public int id { get; set; }
        public long departureDate { get; set; }
        public long arrivalDate { get; set; }
        public int seatsLeft { get; set; }
        public Routes route { get; set; }
        public List<Tickets> tickets { get; set; }
        public int price { get; set; }
    }

    public class Tickets
    {
        public string id { get; set; }
        public List<Passengers> passengers { get; set; }
        public List<Schedules> schedule { get; set; }
    }

    public class Passengers
    {
        public int id { get; set; }
       public string firstName { get; set; }
        public string lastName { get; set; }
        public List<Tickets> tickets { get; set; }
    }

    public class DatabaseContext: DbContext
    {
        public DatabaseContext() : base("name=DB")
        {
            //Database.CreateIfNotExists();
        }

        public DbSet<Admins> Admins { get; set; }
        public DbSet<Routes> Routes { get; set; }
        public DbSet<Schedules> Schedules { get; set; }
        public DbSet<Tickets> Tickets { get; set; }
        public DbSet<Passengers> Passengers { get; set; }
    }
}
