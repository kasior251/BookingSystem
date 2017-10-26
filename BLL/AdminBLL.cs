using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public class AdminLogic
    {
        public bool addAdmin(Admin admin)
        {
            var AdminDAL = new DatabaseDAL();
            return AdminDAL.addAdmin(admin);
        }

        public static bool adminExists(Admin admin)
        {
            return DatabaseDAL.adminExists(admin);
        }


    }
}
