using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DAL
{
    //hit skal metoder for å "betjene" databasen
    public class DatabaseDAL
    {
        private static DatabaseContext db = new DatabaseContext();

        public bool addAdmin(Admin admin)
        {
            try
            {
                var newAdmin = new Admins();
                byte[] passwordDB = makeHash(admin.password);
                newAdmin.username = admin.username;
                newAdmin.password = passwordDB;
                db.Admins.Add(newAdmin);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public static bool adminExists(Admin admin)
        {
            byte[] passwordDB = makeHash(admin.password);
            Admins adminOK = db.Admins.FirstOrDefault(
                a => a.password == passwordDB && a.username == admin.username);
            if (adminOK == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
   
        private static byte[] makeHash(String password)
        {
            string salt = generateSalt(20);
            byte[] passwordInn;
            var algortithm = System.Security.Cryptography.SHA256.Create();
            passwordInn = System.Text.Encoding.ASCII.GetBytes(password + salt);
            return algortithm.ComputeHash(passwordInn);

        }

        private static string generateSalt(int length)
        {
            string signs = "1234567890qwertyuiopasdfghjklzxcvbnm";
            string salt = "";
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < signs.Length; j++)
                {
                    salt += signs[j];
                }
            }
            return salt;
        }

        public List<Route> findAllRoutes()
        {
            List<Route> allRoutes = db.Routes.Select(r => new Route()
            {
                id = r.id,
                origin = r.origin,
                destination = r.destination
            }).ToList();

            return allRoutes;
        }

        public bool addNew(Route route)
        {
            if (routeEksists(route.origin, route.destination))
            {
                return false;
            }
            try
            {
                var newRoute = new Routes()
                {
                    origin = route.origin,
                    destination = route.destination,
                };
                var newRouteR = new Routes()
                {
                    origin = route.destination,
                    destination = route.origin
                };
                db.Routes.Add(newRoute);
                db.Routes.Add(newRouteR);
                db.SaveChanges();
                return true;
            } catch (Exception e)
            {
                return false;
            }
        }

        //sjekker om strekningen finnes i db fra før
        private bool routeEksists(String origin, String destination)
        {
            Routes route = db.Routes.FirstOrDefault(r => r.origin == origin && r.destination == destination);
            if (route == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool deleteRoute(int routeId)
        {
            var route = db.Routes.FirstOrDefault(r => r.id == routeId);
            if (route == null)
            {
                return false;
            }
            else
            {
                try
                {
                    db.Routes.Remove(route);
                    db.SaveChanges();
                    return true;
                } catch (Exception e)
                {
                    return false;
                }
                
            }
        }

        public List<Schedule> findFlights(int routeId)
        {
            List<Schedules> flights = (from s in db.Schedules
                                     where s.route.id == routeId
                                     select s).ToList();
            List<Schedule> list = new List<Schedule>();
            foreach ( Schedules f in flights) {
                Schedule s = new Schedule()
                {
                    id = f.id,
                    departureDate = f.departureDate,
                    arrivalDate = f.arrivalDate,
                    seatsLeft = f.seatsLeft,
                    price = f.price
                };
                list.Add(s);
            }
            return list;
        }

        private DateTime getDate(long milis)
        {
            DateTime dateTime = new DateTime(milis);
            return dateTime;
        }

        public bool addNewFlight(long departure, long arrival, int seats, int price, int routeId)
        {
            try
            {
                var newFlight = new Schedules()
                {
                    departureDate = departure,
                    arrivalDate = arrival,
                    seatsLeft = seats,
                    route = db.Routes.Find(routeId),
                    price = price
                };
                db.Schedules.Add(newFlight);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Route getRoute(int id)
        {
            var route = db.Routes.FirstOrDefault(r => r.id == id);

            return new Route()
            {
                id = route.id,
                origin = route.origin,
                destination = route.destination
            };
        }
    }
}
