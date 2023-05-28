using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class NewsCategory_Repository:INewsCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(news_category obj)
        {
            _entities.news_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.news_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(news_category obj)
        {
            var entity = _entities.news_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.news_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public news_category Find(int id)
        {
            return _entities.news_category.Find(id);
        }

        public List<news_category> GetAll()
        {
            return _entities.news_category.ToList();
        }
    }
}
