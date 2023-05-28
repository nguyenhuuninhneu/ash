using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITeamMemberNoteDetailRepository
    {
        List<TeamMemberNoteDetail> GetAll();
        TeamMemberNoteDetail Find(int id);
        void Add(TeamMemberNoteDetail obj);
        void Edit(TeamMemberNoteDetail obj);
        void Delete(int id);
    }
}
