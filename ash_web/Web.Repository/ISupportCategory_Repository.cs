using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ISupportCategory_Repository
    {
        List<support_category> GetAll();
        support_category Find(int id);

        void Add(support_category obj);
        void Edit(support_category obj);
        void Delete(int id);
    }
}
