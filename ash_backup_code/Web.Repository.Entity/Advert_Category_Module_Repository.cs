using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class Advert_Category_Module_Repository : IAdvert_Category_Module_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(advert_category_module obj)
        {
            _entities.advert_category_module.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.advert_category_module.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(advert_category_module obj)
        {
            var entity = _entities.advert_category_module.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.advert_category_module.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public advert_category_module Find(int id)
        {
            return _entities.advert_category_module.Find(id);
        }

        public List<advert_category_module> GetAll()
        {
            return _entities.advert_category_module.ToList();
        }
    }
}
