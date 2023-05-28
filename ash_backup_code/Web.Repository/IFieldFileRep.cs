using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IFieldFileRep
    {
        IEnumerable<tbl_fieldfiles> GetData();
        IEnumerable<tbl_fieldfiles> GetFiles(int FieldID);
        bool Create(tbl_fieldfiles obj);
        bool Edit(tbl_fieldfiles obj);
        bool Delete(int Id);
        bool DeletePermanently(int Id);
        tbl_fieldfiles Find(int Id);
    }
}
