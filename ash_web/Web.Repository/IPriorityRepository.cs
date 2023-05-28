using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IPriorityRepository
    {
        List<Priority> GetAll();
        Priority Find(int id);
        void Add(Priority obj);
        void Edit(Priority obj);
        void Delete(int id);
    }
}
