using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IAdminMenuRepository
    {
        List<tbl_AdminMenu> GetAll();
        tbl_AdminMenu Find(int id);
        void Add(tbl_AdminMenu obj);
        void Edit(tbl_AdminMenu obj);
        void Delete(int id);
    }
}
