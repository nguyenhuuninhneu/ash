using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ICountAccessRepository
    {
        tbl_CountAccess Find(int id);
        void Edit(tbl_CountAccess obj);       
    }
}
