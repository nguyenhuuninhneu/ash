using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITruyNaRepository
    {
        List<tbl_truyna> GetAll();
        tbl_truyna Find(int id);
        void Add(tbl_truyna obj);
        void Edit(tbl_truyna obj);
        void Delete(int id);
        IEnumerable<tbl_ToGiac> GetAllCMT();
        tbl_ToGiac FindCMT(int id);
        void DeleteCMT(int id);
    }
}
