using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
   public interface IPositionRepository
    {
        List<tbl_Position> GetAll();
        void Add(tbl_Position obj);
        tbl_Position Find(int id);
        void Edit(tbl_Position obj);
        void Delete(int id);
        List<tbl_Position> GetListPosByCode(string code);
    }
}
