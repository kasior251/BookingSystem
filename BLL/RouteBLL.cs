using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public class RouteLogic
    {
        public List<Route> findAllRoutes()
        {
            var RouteDAL = new DatabaseDAL();
            return RouteDAL.findAllRoutes();
        }

        public bool addNew(Route route)
        {
            var RouteDAL = new DatabaseDAL();
            return RouteDAL.addNew(route);
        }

        public bool deleteRoute(int routeId)
        {
            var RouteDAL = new DatabaseDAL();
            return RouteDAL.deleteRoute(routeId);
        }

        public Route getRoute(int id)
        {
            var RouteDAL = new DatabaseDAL();
            return RouteDAL.getRoute(id);
        }
    }
}
