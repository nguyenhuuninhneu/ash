using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILinhVucVanBanRepository
    {
        List<tbl_LinhVucVanBan> GetAll();
        tbl_LinhVucVanBan Find(int id);
        void Add(tbl_LinhVucVanBan obj);
        void Edit(tbl_LinhVucVanBan obj);
        void Delete(int id);
    }
}
