using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILinhVucTTHCRepository
    {
        List<tbl_LinhVucTTHC> GetAll();
        tbl_LinhVucTTHC Find(int id);
        void Add(tbl_LinhVucTTHC obj);
        void Edit(tbl_LinhVucTTHC obj);
        void Delete(int id);
    }
}
