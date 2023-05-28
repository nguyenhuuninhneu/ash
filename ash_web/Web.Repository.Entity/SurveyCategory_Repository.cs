using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class SurveyCategory_Repository:ISurveyCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(survey_category obj)
        {
            _entities.survey_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.survey_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(survey_category obj)
        {
            var entity = _entities.survey_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.survey_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public survey_category Find(int id)
        {
            return _entities.survey_category.Find(id);
        }

        public List<survey_category> GetAll()
        {
            return _entities.survey_category.ToList();
        }
    }
}
