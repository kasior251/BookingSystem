using BookingSystem.Model;
using BookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


//endre origin og destination til drop-down lister
namespace BookingSystem.Controllers
{
    public class HomeController : Controller
    {
        private DB db = new DB();

        public ActionResult Index()
        {
            return View();
        }

        //Få alle mulige avgang-steder
        public string GetOrigin()
        {
            List<Route> allRoutes = db.Routes.ToList();

            var allOrigins = new List<string>();

            foreach (Route r in allRoutes)
            {
                string foundOrigin = allOrigins.FirstOrDefault(rl => rl.Contains(r.origin));
                if (foundOrigin == null)
                {
                    // ikke funnet origin i listen, legg den inn i listen
                    allOrigins.Add(r.origin);
                }
            }
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(allOrigins);

        }

        //få alle destinasjoner hvor man kan reise fra byen som passeres som argument
        public string GetDestination(string fromOrigin)
        {
            List<Route> allRoutes = db.Routes.ToList();
            var allDestinations = new List<string>();
            foreach (Route r in allRoutes)
            {
                if (r.origin == fromOrigin)
                {
                    string foundDestination = allDestinations.FirstOrDefault(r1 => r1.Contains(r.destination));
                    if (foundDestination == null)
                    {
                        //destination finnes ikke i lista ennå, legges til
                        allDestinations.Add(r.destination);
                    }
                }
            }
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(allDestinations);

        }

        //få alle mulige flyvninger mellom de 2 byene, der det er ledige billetter som ønskes
        public string GetSchedule(string fromOrigin, string toDestination, int passengers, long date)
        {
            //finn alle routene som tilsvarer valgte strekningen
            List<int> rId = (from r in db.Routes
                             where r.origin == fromOrigin && r.destination == toDestination
                             select r.id).ToList();
            List<Schedule> allFlights = new List<Schedule>();
            foreach (int i in rId)
            {
                allFlights = db.Schedules.Where(s => s.route.id == i && s.seatsLeft >= passengers && s.departureDate > date).ToList();
            }

            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(allFlights);
        }

        public long GetArrivalOutbound(int id)
        {
            long arrivalTime = (from s in db.Schedules
                                where s.id == id
                                select s.arrivalDate).FirstOrDefault();
            return arrivalTime;
        }

        //hold av valgt flyvning, redirect til siden hvor bookingen gjennomføres
        public ActionResult BookFlight(int passengers, int flightId, int returnId)
        {
            Session["Passengers"] = passengers;
            Session["Schedule"] = flightId;
            Session["ScheduleR"] = returnId;
            int price = (from s in db.Schedules
                            where s.id == flightId
                            select s.price).FirstOrDefault();
            if (returnId != 0)
            {
                price += (from s in db.Schedules
                          where s.id == returnId
                          select s.price).FirstOrDefault();
            }
            Session["Price"] = price;
            return View();
        }

        //book billetten(e) i systemet
        public string Summary(string[] firstNames, string[] lastNames)
        {
            var nr = firstNames.Length;
            List<Passenger> passengers = new List<Passenger>();
            for (int i = 0; i < nr; i++)
            {
                passengers.Add(new Passenger()
                {
                    firstName = firstNames[i],
                    lastName = lastNames[i],
                });
            }

            var flightId = (int)Session["Schedule"];
            var returnId = (int)Session["ScheduleR"];

            Ticket ticket = new Ticket();
            List<Schedule> schedules = new List<Schedule>();
            schedules.Add(db.Schedules.Find(flightId));
            if (returnId != 0)
            {
                schedules.Add(db.Schedules.Find(returnId));
            }
            Session["Ticket"] = ticket;
            db.Tickets.Add(ticket);
            ticket.passengers = passengers;
            ticket.schedule = schedules;
            ticket.id = ConfirmNr();

            var schedule = (from s in db.Schedules
                         where  s.id == flightId
                         select s).FirstOrDefault();


            //oppdater antall tilgj. seter
            schedule.seatsLeft -= nr;
            string retString = "";

            if (returnId != 0)
            {
                var scheduleR = (from s in db.Schedules
                                 where s.id == returnId
                                 select s).FirstOrDefault();
                scheduleR.seatsLeft -= nr;
            }

            try
            {
                db.SaveChanges();
                retString = "OK";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                retString = "error";   
            }
            var jsonSerializer = new JavaScriptSerializer();
            return jsonSerializer.Serialize(retString);
        }

        //generere bookingsnummer
        private string ConfirmNr()
        {
            string c = "11223344556677889900QWERTYUIOPASDFGHJKLZXCVBNM";
            string number = "";
            Random random = new Random();
            for (int i = 0; i < 7; i++)
            {
                number += c[random.Next(c.Length)];
            }
            return number;
        }

        //vis side med bekreftelse av bookingen
        public ActionResult ShowConfirmation()
        {
            Ticket ticket = (Ticket)(Session["Ticket"]);
            return View(ticket);
        }

        //error side
        public ActionResult Error()
        {
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
 