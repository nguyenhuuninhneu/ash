using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class DocumentType_Repository : IDocumentType_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(tbl_Document_Type obj)
        {
            _entities.tbl_Document_Type.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Document_Type.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(tbl_Document_Type obj)
        {
            var entity = _entities.tbl_Document_Type.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.tbl_Document_Type.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();
        }

        public tbl_Document_Type Find(int id)
        {
            return _entities.tbl_Document_Type.Find(id);
        }

        public List<tbl_Document_Type> GetAll()
        {
            return _entities.tbl_Document_Type.ToList();
        }
    }
}
