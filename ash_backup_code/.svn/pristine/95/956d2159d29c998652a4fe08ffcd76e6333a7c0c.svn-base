using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class CusGroupRepository : IGroupCusRepository
    {
        private readonly PortalNewsEntities dbcontext = new PortalNewsEntities();
        public void Add(GroupCusUser obj)
        {
            dbcontext.GroupCusUsers.Add(obj);
            dbcontext.SaveChanges();
        }

        public void Delete(int id)
        {
            var model = Find(id);
            dbcontext.GroupCusUsers.Remove(model);
            dbcontext.SaveChanges();
        }
        public void Edit(GroupCusUser obj)
        {
            dbcontext.Entry(obj).State = EntityState.Detached;
            dbcontext.Entry(obj).State = EntityState.Modified;
            dbcontext.SaveChanges();
        }
        public GroupCusUser Find(int id)
        {
            return dbcontext.GroupCusUsers.Find(id);
        }

        public IEnumerable<GroupCusUser> Getall()
        {
            return dbcontext.GroupCusUsers;
        }
    }
}
