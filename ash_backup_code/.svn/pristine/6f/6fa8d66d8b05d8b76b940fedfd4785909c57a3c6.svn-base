using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class DocumentPublisher_Repository : IDocumentPublisher_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(document_publisher obj)
        {
            _entities.document_publisher.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.document_publisher.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(document_publisher obj)
        {
            var entity = _entities.document_publisher.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.document_publisher.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public document_publisher Find(int id)
        {
            return _entities.document_publisher.Find(id);
        }

        public List<document_publisher> GetAll()
        {
            return _entities.document_publisher.ToList();
        }
    }
}
