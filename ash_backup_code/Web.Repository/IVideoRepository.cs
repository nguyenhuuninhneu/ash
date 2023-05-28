using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IVideoRepository
    {
        IEnumerable<tbl_Video> GetAll();
        tbl_Video Find(int id);
        void Add(tbl_Video obj);
        void Edit(tbl_Video obj);
        void Delete(int id);
    }
}
