using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IModuleCateRepository
    {
        IEnumerable<tbl_ModuleCate> GetAll();
        tbl_ModuleCate Find(int id);
        void Add(tbl_ModuleCate obj);
        bool Edit(tbl_ModuleCate obj);
        void Edit(List<tbl_ModuleCate> lstobj);
        void Delete(int id); 
    }
}
