using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
   public interface IAccessViewRespository
    {
        List<tbl_AccessView> GetAll();
        tbl_AccessView Find(int id);
        void Add(tbl_AccessView obj);
        void Edit(tbl_AccessView obj);
        void Delete(int id);
    }
}
