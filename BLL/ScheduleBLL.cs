using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ScheduleLogic
    {
        public List<Schedule> getFlights(int routeId)
        {
            var ScheduleDAL = new DatabaseDAL();
            return ScheduleDAL.findFlights(routeId);
        }

        public bool addNewFlight(long departure, long arrival, int seats, int price, int routeId)
        {
            var ScheduleDAL = new DatabaseDAL();
            return ScheduleDAL.addNewFlight(departure, arrival, seats, price, routeId);
        }

        public bool deleteFlight(int id)
        {
            var ScheduleDAL = new DatabaseDAL();
            return ScheduleDAL.deleteFlight(id);
        }

    }
}
