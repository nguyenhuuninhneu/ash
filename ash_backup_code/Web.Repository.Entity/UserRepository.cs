using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class UserRepository : IUserRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(tbl_User obj)
        {
            _entities.tbl_User.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_User.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(tbl_User obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public void Edit(List<tbl_User> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public tbl_User Find(int id)
        {
            return _entities.tbl_User.Find(id);
        }

        public IEnumerable<tbl_User> GetAll()
        {
            return _entities.tbl_User;
        }
    }
}
