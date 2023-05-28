using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IOnlineRepository
    {
        IEnumerable<tbl_Online> GetAll();
        tbl_Online Find(int id);
        void Add(tbl_Online obj);
        void Edit(tbl_Online obj);
        void Delete(int id);
    }
}
