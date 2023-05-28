using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IqProcedureRepository
    {
        List<qProcedure> GetAll();
        qProcedure Find(int id);
        void Add(qProcedure obj);
        void Edit(qProcedure obj);
        void Delete(int id);
    }
}
