using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IFooterRepository
    {
        IEnumerable<tbl_Footer> GetAll();
        tbl_Footer Find(int id);
        void Add(tbl_Footer obj);
        bool Edit(tbl_Footer obj);
        void Delete(int id);
    }
}
