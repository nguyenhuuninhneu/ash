using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ILanguagesRepository
    {
        IEnumerable<tbl_Languages> GetAll();

    }
}
