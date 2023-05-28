using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface INewsPaperRepository
    {
        IEnumerable<tbl_NewsPaper> GetAll();
        tbl_NewsPaper Find(int id);
        void Add(tbl_NewsPaper obj);
        void Edit(tbl_NewsPaper obj);
        void Delete(int id);
        IEnumerable<tbl_NewsPaperComment> GetAllCmt();
        tbl_NewsPaperComment FindCmt(int id);
        void AddCmt(tbl_NewsPaperComment obj);
        void EditCmt(tbl_NewsPaperComment obj);
        void DeleteCmt(int id);
    }
}
