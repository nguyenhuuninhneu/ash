using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILichCongTacRepository
    {
        List<tbl_LichCongTac> GetAll();
        tbl_LichCongTac Find(int id);
        void Add(tbl_LichCongTac obj);
        void Edit(tbl_LichCongTac obj);
        void Delete(int id);
    }
}
