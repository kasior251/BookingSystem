using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.DAL
{
    public interface IDatabaseDAL
    {
        bool addAdmin(Admin admin);
        bool adminExists(Admin admin);
        List<Route> findAllRoutes();
        bool addNew(Route route);
        bool deleteRoute(int routeId);
        List<Schedule> findFlights(int routeId);
        Route getRoute(int id);
        bool deleteFlight(int id);
        bool addNewFlight(long departure, long arrival, int seats, int price, int routeId);

    }
}
