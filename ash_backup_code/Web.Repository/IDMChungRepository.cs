using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IDMChungRepository
    {
        IEnumerable<tbl_DMChung> GetAll();
        tbl_DMChung Find(int id);
        bool Add(tbl_DMChung obj);
        bool Edit(tbl_DMChung obj);
        void Delete(int id);
    }
}
