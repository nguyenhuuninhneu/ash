using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository.Entity
{
    public class ConfigRepository : IConfigRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public IEnumerable<tbl_Config> GetAll()
        {
            return _entities.tbl_Config;
        }

        public void Add(tbl_Config obj)
        {
            _entities.tbl_Config.Add(obj);
            _entities.SaveChanges();
        }

        public void Edit(tbl_Config obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public tbl_Config Find(int id)
        {
            return _entities.tbl_Config.Find(id);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Config.Remove(obj);
            _entities.SaveChanges();
        }
    }
}
