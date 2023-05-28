using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class SupportCategory_Repository : ISupportCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(support_category obj)
        {
            _entities.support_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.support_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(support_category obj)
        {
            var entity = _entities.support_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.support_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public support_category Find(int id)
        {
            return _entities.support_category.Find(id);
        }

        public List<support_category> GetAll()
        {
            return _entities.support_category.ToList();
        }
    }
}
