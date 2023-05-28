using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IChatRepository
    {
        IEnumerable<tbl_Chat> GetAll();
        tbl_Chat Find(int id);
        void Add(tbl_Chat obj);
        void Edit(tbl_Chat obj);
        void Edit(List<tbl_Chat> lstobj);
        void Delete(int id);
    }
}
