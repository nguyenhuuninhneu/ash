using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IContactReporitory
    {
        IEnumerable<tbl_Contact> GetAll();
        tbl_Contact Find(int id);
        void Add(tbl_Contact obj);
        void Edit(tbl_Contact obj);
        void Delete(int id);
    }
}
