using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class QAReporitory : IQAReporitory
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_QuestionAndAnswer obj)
        {
            _entities.tbl_QuestionAndAnswer.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_QuestionAndAnswer.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_QuestionAndAnswer obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_QuestionAndAnswer Find(int id)
        {
            return _entities.tbl_QuestionAndAnswer.Find(id);
        }


        public IEnumerable<Model.tbl_QuestionAndAnswer> GetAll()
        {
            return _entities.tbl_QuestionAndAnswer.OrderByDescending(g => g.CreatedDate);
        }
    }
}
