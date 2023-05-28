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
    public class ContactReporitory : IContactReporitory
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_Contact obj)
        {
            _entities.tbl_Contact.Add(obj);
            _entities.SaveChanges();
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Contact.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_Contact obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_Contact Find(int id)
        {
            return _entities.tbl_Contact.Find(id);
        }


        public IEnumerable<Model.tbl_Contact> GetAll()
        {
            return _entities.tbl_Contact.OrderByDescending(g => g.CreatedDate);
        }
    }
}
