using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface INewsHistoryRepository
    {
        IEnumerable<tbl_HistoryNews> GetAll();
        tbl_HistoryNews Find(int id);
        void Add(tbl_HistoryNews obj);
        void Edit(tbl_HistoryNews obj);
        void Edit(List<tbl_HistoryNews> lstobj);
        void Delete(int id);
    }
}
