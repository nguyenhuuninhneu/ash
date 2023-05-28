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
    public class ToGiacRepository : IToGiacRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_ToGiac obj)
        {
            _entities.tbl_ToGiac.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_ToGiac.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_ToGiac obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_ToGiac Find(int id)
        {
            return _entities.tbl_ToGiac.Find(id);
        }


        public IEnumerable<Model.tbl_ToGiac> GetAll()
        {
            return _entities.tbl_ToGiac.OrderByDescending(g => g.CreateDate);
        }
    }
}
