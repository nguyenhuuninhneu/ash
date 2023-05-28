using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class LogRepository : ILogRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public IEnumerable<tbl_Logs> GetAll()
        {
            return _entities.tbl_Logs;
        }

        public void Add(tbl_Logs obj)
        {
            _entities.tbl_Logs.Add(obj);
            _entities.SaveChanges();
        }

        public void Edit(tbl_Logs obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public tbl_Logs Find(int id)
        {
            return _entities.tbl_Logs.Find(id);
        }
    }
}
