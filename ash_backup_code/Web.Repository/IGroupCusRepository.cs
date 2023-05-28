using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IGroupCusRepository
    {
        IEnumerable<GroupCusUser> Getall();
        GroupCusUser Find(int id);
        void Add(GroupCusUser obj);
        void Edit(GroupCusUser obj);
        void Delete(int id);
    }
}
