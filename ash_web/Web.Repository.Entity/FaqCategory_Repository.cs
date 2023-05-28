using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class FaqCategory_Repository:IFaqCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(faq_category obj)
        {
            _entities.faq_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.faq_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(faq_category obj)
        {
            var entity = _entities.faq_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.faq_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public faq_category Find(int id)
        {
            return _entities.faq_category.Find(id);
        }

        public List<faq_category> GetAll()
        {
            return _entities.faq_category.ToList();
        }
    }
}
