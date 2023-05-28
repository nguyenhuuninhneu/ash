using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class GuideCategory_Repository:IGuideCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(guide_category obj)
        {
            _entities.guide_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.guide_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(guide_category obj)
        {
            var entity = _entities.guide_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.guide_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public guide_category Find(int id)
        {
            return _entities.guide_category.Find(id);
        }

        public List<guide_category> GetAll()
        {
            return _entities.guide_category.ToList();
        }
    }
}
