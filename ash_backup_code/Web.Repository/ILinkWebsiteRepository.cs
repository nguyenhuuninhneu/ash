using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILinkWebsiteRepository
    {
        List<tbl_LinkWebsite> GetAll();
        tbl_LinkWebsite Find(int id);
        void Add(tbl_LinkWebsite obj);
        void Edit(tbl_LinkWebsite obj);
        void Delete(int id);
    }
}
