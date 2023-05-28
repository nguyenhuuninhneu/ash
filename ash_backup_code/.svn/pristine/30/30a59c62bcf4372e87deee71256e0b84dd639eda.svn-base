using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class Support_Category_Module_Repository : ISupport_Category_Module_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(support_category_module obj)
        {
            _entities.support_category_module.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.support_category_module.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(support_category_module obj)
        {
            var entity = _entities.support_category_module.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.support_category_module.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public support_category_module Find(int id)
        {
            return _entities.support_category_module.Find(id);
        }

        public List<support_category_module> GetAll()
        {
            return _entities.support_category_module.ToList();
        }
    }
}
