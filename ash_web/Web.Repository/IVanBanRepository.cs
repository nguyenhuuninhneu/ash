using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IVanBanRepository
    {
        IEnumerable<tbl_VanBan> GetAll();
        tbl_VanBan Find(int id);
        void Add(tbl_VanBan obj);
        void Edit(tbl_VanBan obj);
        void Delete(int id);
    }
}
