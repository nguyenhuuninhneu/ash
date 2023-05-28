using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IDonViTTHCRepository
    {
        List<tbl_DonViTTHC> GetAll();
        tbl_DonViTTHC Find(int id);
        void Add(tbl_DonViTTHC obj);
        void Edit(tbl_DonViTTHC obj);
        void Delete(int id);
    }
}
