using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface ITagDevOpsRepository
    {
        List<TagDevOp> GetAll();
        TagDevOp Find(int id);
        void Add(TagDevOp obj);
        void Edit(TagDevOp obj);
        void Delete(int id);
    }
}
