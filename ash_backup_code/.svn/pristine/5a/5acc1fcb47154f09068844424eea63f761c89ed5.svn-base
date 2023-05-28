using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class VanBanRepository : IVanBanRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_VanBan obj)
        {
            _entities.tbl_VanBan.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_VanBan.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_VanBan obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_VanBan Find(int id)
        {
            return _entities.tbl_VanBan.Find(id);
        }

        public IEnumerable<Model.tbl_VanBan> GetAll()
        {
            return _entities.tbl_VanBan;
        }
    }
}
