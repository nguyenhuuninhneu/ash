using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IAdvertCategory_Repository
    {
        List<advert_category> GetAll();
        advert_category Find(int id);

        void Add(advert_category obj);
        void Edit(advert_category obj);
        void Delete(int id);
    }
}
