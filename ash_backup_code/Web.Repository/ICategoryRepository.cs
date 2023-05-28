using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ICategoryRepository
    {
      
        List<tbl_Category> GetAll();
        List<view_Category_Languages> GetAllCategoryLanguageses();
        tbl_Category Find(int id);        
        void Add(tbl_Category obj);
        void Edit(tbl_Category obj);
        void Delete(int id);
    }
}
