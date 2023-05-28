using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class AdvertCategory_Repository : IAdvertCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(advert_category obj)
        {
            _entities.advert_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.advert_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(advert_category obj)
        {
            var entity = _entities.advert_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.advert_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public advert_category Find(int id)
        {
            return _entities.advert_category.Find(id);
        }

        public List<advert_category> GetAll()
        {
            return _entities.advert_category.ToList();
        }
    }
}
