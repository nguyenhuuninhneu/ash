using System.Collections.Generic; 
using Web.Model;

namespace Web.Repository
{
    public interface IMagazineRepository
    {
        IEnumerable<tbl_Magazine> GetAll();
        tbl_Magazine Find(int id);
        void Add(tbl_Magazine obj);
        bool Edit(tbl_Magazine obj);
        void Edit(List<tbl_Magazine> lstobj);
        void Delete(int id); 
    }
}
