using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ICalendarRepository
    {
        List<tbl_Calendar> GetAll();
        tbl_Calendar Find(int id);
        void Add(tbl_Calendar obj);
        void Edit(tbl_Calendar obj);
        void Delete(int id);
    }
}
