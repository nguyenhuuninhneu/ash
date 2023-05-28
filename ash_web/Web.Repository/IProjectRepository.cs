using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IProjectRepository
    {
        List<Project> GetAll();
        Project Find(int id);
        void Add(Project obj);
        void Edit(Project obj);
        void Delete(int id);
    }
}
