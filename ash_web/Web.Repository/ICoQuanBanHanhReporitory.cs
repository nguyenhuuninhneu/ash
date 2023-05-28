using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ICoQuanBanHanhReporitory
    {
        List<tbl_CoQuanBanHanh> GetAll();
        tbl_CoQuanBanHanh Find(int id);
        void Add(tbl_CoQuanBanHanh obj);
        void Edit(tbl_CoQuanBanHanh obj);
        void Delete(int id);
    }
}
