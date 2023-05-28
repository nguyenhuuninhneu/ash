using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository
{
    public interface IConfigRepository
    {
        IEnumerable<tbl_Config> GetAll();
        void Add(tbl_Config  obj);
        void Edit(tbl_Config  obj);
        tbl_Config  Find(int id);
        void Delete(int id);
    }
}
