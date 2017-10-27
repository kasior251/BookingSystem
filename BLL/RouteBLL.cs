using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public class RouteLogic: IRouteLogic
    {
        private IDatabaseDAL _repository;

        public RouteLogic()
        {
            _repository = new DatabaseDAL();
        }

        public RouteLogic(IDatabaseDAL stub)
        {
            _repository = stub;
        }

        public List<Route> findAllRoutes()
        {
            return _repository.findAllRoutes();
        }

        public bool addNew(Route route)
        {
            return _repository.addNew(route);
        }

        public bool deleteRoute(int routeId)
        {
            return _repository.deleteRoute(routeId);
        }

        public Route getRoute(int id)
        {
            return _repository.getRoute(id);
        }

    }
}
