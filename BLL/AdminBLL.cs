using BookingSystem.DAL;
using BookingSystem.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingSystem.BLL
{
    public class AdminLogic : IAdminLogic
    {
        private IDatabaseDAL _repository;

        public IDatabaseDAL getRepository()
        {
            return _repository;
        }

        public AdminLogic()
        {
            _repository = new DatabaseDAL();
        }

        public AdminLogic(IDatabaseDAL stub)
        {
            _repository = stub;
        }

        public bool addAdmin(Admin admin)
        {

            return _repository.addAdmin(admin);
        }

        public bool adminExists(Admin admin)
        {
            return _repository.adminExists(admin);
        }


    }
}
