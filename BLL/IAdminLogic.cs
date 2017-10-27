using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public interface IAdminLogic
    {
        bool addAdmin(Admin admin);
        bool adminExists(Admin admin);
        IDatabaseDAL getRepository();
    }
}
