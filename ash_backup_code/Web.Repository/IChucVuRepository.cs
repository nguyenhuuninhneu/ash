using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IChucVuRepository
    {
        List<tbl_ChucVu> GetAll();
        tbl_ChucVu Find(int id);
        void Add(tbl_ChucVu obj);
        void Edit(tbl_ChucVu obj);
        void Delete(int id);
    }
}
