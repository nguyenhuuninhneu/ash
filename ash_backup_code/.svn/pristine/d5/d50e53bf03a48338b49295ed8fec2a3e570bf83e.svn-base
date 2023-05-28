using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class Module_Repository:IModule_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(module obj)
        {
            _entities.modules.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.modules.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(module obj)
        {
            var entity = _entities.modules.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.modules.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public module Find(int id)
        {
            return _entities.modules.Find(id);
        }

        public List<module> GetAll()
        {
            return _entities.modules.ToList();
        }
    }
}
