using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public class ScheduleLogic : IScheduleLogic
    {
        private IDatabaseDAL _repository;

        public ScheduleLogic()
        {
            _repository = new DatabaseDAL();
        }

        public ScheduleLogic(IDatabaseDAL stub)
        {
            _repository = stub;
        }
        public List<Schedule> getFlights(int routeId)
        {
            return _repository.findFlights(routeId);
        }

        public bool addNewFlight(long departure, long arrival, int seats, int price, int routeId)
        {
            return _repository.addNewFlight(departure, arrival, seats, price, routeId);
        }

        public bool deleteFlight(int id)
        {
            return _repository.deleteFlight(id);
        }

    }
}
