using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Repository;

namespace Web.Repository.Entity
{
    public class AccessViewRepository : IAccessViewRespository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(tbl_AccessView obj)
        {
            _entities.tbl_AccessView.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_AccessView.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(tbl_AccessView obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public tbl_AccessView Find(int id)
        {
            return _entities.tbl_AccessView.Find(id);
        }

        public List<tbl_AccessView> GetAll()
        {
            var lstData = _entities.tbl_AccessView.ToList();
            return lstData;
        }
    }
}
