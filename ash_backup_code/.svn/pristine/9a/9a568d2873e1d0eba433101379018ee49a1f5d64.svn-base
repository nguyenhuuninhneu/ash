using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class AccessOnlineRepository : IAccessOnlineRespository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(tbl_AccessOnline obj)
        {
            _entities.tbl_AccessOnline.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(tbl_AccessOnline obj)
        {
            _entities.tbl_AccessOnline.Remove(obj);
            _entities.SaveChanges();
        }
        public tbl_AccessOnline Find(string id)
        {
            return _entities.tbl_AccessOnline.Find(id);
        }

        public List<tbl_AccessOnline> GetAll()
        {
            var lstData = _entities.tbl_AccessOnline.ToList();
            return lstData;
        }
    }
}
