using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IBoxNewsConfigRepository
    {
        List<tbl_BoxNewsConfig> GetAll();
        tbl_BoxNewsConfig Find(string Code, int PageElementId, string LangCode);
        void Add(tbl_BoxNewsConfig obj);
        void Edit(tbl_BoxNewsConfig obj);
        void Delete(string Code, int PageElementId, string LangCode);
    }
}
