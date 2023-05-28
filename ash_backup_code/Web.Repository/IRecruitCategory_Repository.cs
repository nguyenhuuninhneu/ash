using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IRecruitCategory_Repository
    {
        List<recruit_category> GetAll();
        recruit_category Find(int id);

        void Add(recruit_category obj);
        void Edit(recruit_category obj);
        void Delete(int id);
    }
}
