using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public interface  IRouteLogic
    {
        List<Route> findAllRoutes();
        bool addNew(Route route);
        bool deleteRoute(int routeId);
        Route getRoute(int id);

    }
}
