using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITeamMemberRepository
    {
        List<TeamMember> GetAll();
        TeamMember Find(int id);
        void Add(TeamMember obj);
        void Edit(TeamMember obj);
        void Delete(int id);
    }
}
