using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookingSystem.Model;

namespace BookingSystem.DAL
{
    public class DatabaseDALStub : IDatabaseDAL
    {
        public bool addAdmin(Admin admin)
        {
            if (admin.username == "")
            {
                return false;
            } else {
                return true;
            }
        }

        public bool addNew(Route route)
        {
            if (route.id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool addNewFlight(long departure, long arrival, int seats, int price, int routeId)
        {
            if (departure == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool adminExists(Admin admin)
        {
            if (admin.username == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool deleteFlight(int id)
        {
            if (id == 0)
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
            if (routeId == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<Route> findAllRoutes()
        {
            var routes = new List<Route>();
            Route r = new Route()
            {
                id = 1,
                origin = "Oslo",
                destination = "Bergen"
            };
            routes.Add(r);
            routes.Add(r);
            routes.Add(r);
            return routes;
        }

        public List<Schedule> findFlights(int routeId)
        {
            var flights = new List<Schedule>();
            Schedule s = new Schedule()
            {
                id = 1,
                departureDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                arrivalDate = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                seatsLeft = 0,
                price = 0,
                route = new Route()
                {
                    id = 1,
                    origin = "Oslo",
                    destination = "Bergen"
                }
            };
            flights.Add(s);
            flights.Add(s);
            flights.Add(s);
            return flights;
        }

        public Route getRoute(int id)
        {
            return new Route()
            {
                id = 1,
                origin = "Oslo",
                destination = "Bergen"
            };
        }
    }
}
