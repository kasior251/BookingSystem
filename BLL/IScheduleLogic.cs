using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public interface IScheduleLogic
    {
        List<Schedule> getFlights(int routeId);
        bool addNewFlight(long departure, long arrival, int seats, int price, int routeId);
        bool deleteFlight(int id);
    }
}
