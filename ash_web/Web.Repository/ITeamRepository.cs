using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITeamRepository
    {
        List<Team> GetAll();
        Team Find(int id);
        void Add(Team obj);
        void Edit(Team obj);
        void Delete(int id);
    }
}
