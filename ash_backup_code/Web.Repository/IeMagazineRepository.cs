using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IeMagazineRepository
    {
        List<tbl_eMagazine> GetAll();
        tbl_eMagazine Find(int id);
        void Add(tbl_eMagazine obj);
        void Edit(tbl_eMagazine obj);
        void Delete(int id);
    }
}
