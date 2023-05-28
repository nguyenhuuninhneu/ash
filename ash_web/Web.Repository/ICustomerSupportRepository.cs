using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ICustomerSupportRepository
    {
        List<CustomerSupport> GetAll();
        CustomerSupport Find(int id);
        void Add(CustomerSupport obj);
        void Edit(CustomerSupport obj);
        void Delete(int id);
    }
}
