using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
   public interface IThamDoYKienRepository
    {
        void Add(tbl_ThamDoYKien obj);
        void UpDate(tbl_ThamDoYKien obj);
        void Delete(int id);
        List<tbl_ThamDoYKien> GetAll();
        tbl_ThamDoYKien Find(int id);

    }
}
