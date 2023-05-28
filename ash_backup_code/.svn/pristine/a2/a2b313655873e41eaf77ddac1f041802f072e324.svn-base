using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class TestCategory_Repository:ITestCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(test_category obj)
        {
            _entities.test_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.test_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(test_category obj)
        {
            var entity = _entities.test_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.test_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public test_category Find(int id)
        {
            return _entities.test_category.Find(id);
        }

        public List<test_category> GetAll()
        {
            return _entities.test_category.ToList();
        }
    }
}
