using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITeamMemberNoteRepository
    {
        List<TeamMemberNote> GetAll();
        TeamMemberNote Find(int id);
        void Add(TeamMemberNote obj);
        void Edit(TeamMemberNote obj);
        void Delete(int id);
    }
}
