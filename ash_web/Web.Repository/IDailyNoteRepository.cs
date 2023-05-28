using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IDailyNoteRepository
    {
        List<DailyNote> GetAll();
        DailyNote Find(int id);
        void Add(DailyNote obj);
        void Edit(DailyNote obj);
        void Delete(int id);
    }
}
