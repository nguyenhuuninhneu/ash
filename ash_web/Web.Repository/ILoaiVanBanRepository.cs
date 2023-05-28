using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILoaiVanBanRepository
    {
        List<tbl_LoaiVanBan> GetAll();
        tbl_LoaiVanBan Find(int id);
        void Add(tbl_LoaiVanBan obj);
        void Edit(tbl_LoaiVanBan obj);
        void Delete(int id);
    }
}
