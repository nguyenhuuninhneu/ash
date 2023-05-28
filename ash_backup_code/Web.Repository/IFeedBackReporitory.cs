using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IFeedBackReporitory
    {
        IEnumerable<tbl_FeedBack> GetAll();
        tbl_FeedBack Find(int id);
        void Add(tbl_FeedBack obj);
        void Edit(tbl_FeedBack obj);
        void Delete(int id);
    }
}
