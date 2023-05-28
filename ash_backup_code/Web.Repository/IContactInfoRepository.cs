using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IContactInfoRepository
    {
        IEnumerable<tbl_ContactInfo> GetAll();
        tbl_ContactInfo Find(int id);
        void Add(tbl_ContactInfo obj);
        void Edit(tbl_ContactInfo obj);
        void Delete(int id);
    }
}
