using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class DocumentPrice_Repository: IDocumentPrice_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(document_price obj)
        {
            _entities.document_price.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.document_price.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(document_price obj)
        {
            var entity = _entities.document_price.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.document_price.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public document_price Find(int id)
        {
            return _entities.document_price.Find(id);
        }

        public List<document_price> GetAll()
        {
            return _entities.document_price.ToList();
        }
    }
}
