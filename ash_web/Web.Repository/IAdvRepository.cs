using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IAdvRepository
    {
        List<tbl_Advert> GetAll();
        tbl_Advert Find(int id);
        void Add(tbl_Advert obj);
        void Edit(tbl_Advert obj);
        void Delete(int id);
    }
}
