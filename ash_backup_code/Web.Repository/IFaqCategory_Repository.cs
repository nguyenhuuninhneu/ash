using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IFaqCategory_Repository
    {
        List<faq_category> GetAll();
        faq_category Find(int id);

        void Add(faq_category obj);
        void Edit(faq_category obj);
        void Delete(int id);
    }
}
