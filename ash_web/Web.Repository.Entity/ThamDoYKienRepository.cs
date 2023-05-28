using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ThamDoYKienRepository : IThamDoYKienRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(tbl_ThamDoYKien obj)
        {
            _entities.tbl_ThamDoYKien.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_ThamDoYKien.Remove(obj);
            _entities.SaveChanges();
        }

        public tbl_ThamDoYKien Find(int id)
        {
            return _entities.tbl_ThamDoYKien.Find(id);
        }

        public List<tbl_ThamDoYKien> GetAll()
        {
            return _entities.tbl_ThamDoYKien.ToList();
        }

        public void UpDate(tbl_ThamDoYKien obj)
        {
            _entities.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            _entities.SaveChanges();
        }
    }
}
