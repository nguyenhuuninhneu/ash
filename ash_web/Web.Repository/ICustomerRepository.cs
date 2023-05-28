using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();
        Customer Find(int id);
        void Add(Customer obj);
        void Edit(Customer obj);
        void Delete(int id);
    }
}
