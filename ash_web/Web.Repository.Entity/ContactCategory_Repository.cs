using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ContactCategory_Repository:IContactCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(contact_category obj)
        {
            _entities.contact_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.contact_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(contact_category obj)
        {
            var entity = _entities.contact_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.contact_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public contact_category Find(int id)
        {
            return _entities.contact_category.Find(id);
        }

        public List<contact_category> GetAll()
        {
            return _entities.contact_category.ToList();
        }
    }
}
