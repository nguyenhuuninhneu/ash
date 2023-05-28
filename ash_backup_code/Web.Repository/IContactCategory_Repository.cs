using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IContactCategory_Repository
    {
        List<contact_category> GetAll();
        contact_category Find(int id);

        void Add(contact_category obj);
        void Edit(contact_category obj);
        void Delete(int id);
    }
}
