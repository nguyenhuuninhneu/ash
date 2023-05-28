using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IReleaseRepository
    {
        List<Release> GetAll();
        Release Find(int id);
        void Add(Release obj);
        void Edit(Release obj);
        void Delete(int id);
    }
}
