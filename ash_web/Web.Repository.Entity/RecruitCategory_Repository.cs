using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class RecruitCategory_Repository : IRecruitCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(recruit_category obj)
        {
            _entities.recruit_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.recruit_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(recruit_category obj)
        {
            var entity = _entities.recruit_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.recruit_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public recruit_category Find(int id)
        {
            return _entities.recruit_category.Find(id);
        }

        public List<recruit_category> GetAll()
        {
            return _entities.recruit_category.ToList();
        }
    }
}
