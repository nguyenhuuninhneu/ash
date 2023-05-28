using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class UserAdminRepository : IUserAdminRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_UserAdmin obj)
        {
            _entities.tbl_UserAdmin.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_UserAdmin.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_UserAdmin obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public void Edit(List<Model.tbl_UserAdmin> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public Model.tbl_UserAdmin Find(int id)
        {
            return _entities.tbl_UserAdmin.Find(id);
        }

        public IEnumerable<Model.tbl_UserAdmin> GetAll()
        {
            return _entities.tbl_UserAdmin;
        }
    }
}
