using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IPageElementRepository
    {
        IEnumerable<tbl_PageElement> GetAll();
        tbl_PageElement Find(int id);
        void Add(tbl_PageElement obj);
        void Edit(tbl_PageElement obj);
        void Delete(int id);
    }
}
