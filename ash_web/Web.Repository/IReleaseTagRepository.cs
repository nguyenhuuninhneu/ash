using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IReleaseTagRepository
    {
        List<ReleaseTag> GetAll();
        ReleaseTag Find(int id);
        void Add(ReleaseTag obj);
        void Edit(ReleaseTag obj);
        void Delete(int id);
    }
}
