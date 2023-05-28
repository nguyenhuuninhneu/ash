using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITestCategory_Repository
    {
        List<test_category> GetAll();
        test_category Find(int id);

        void Add(test_category obj);
        void Edit(test_category obj);
        void Delete(int id);
    }
}
