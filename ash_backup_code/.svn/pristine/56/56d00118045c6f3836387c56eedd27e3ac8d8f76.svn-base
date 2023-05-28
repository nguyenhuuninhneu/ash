using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ThamDoYKienIpRepository : IThamDoYKienIpRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(tbl_ThamDoYKien_Ip obj)
        {
            _entities.tbl_ThamDoYKien_Ip.Add(obj);
            _entities.SaveChanges();
        }

        public tbl_ThamDoYKien_Ip Find(int id)
        {
           return _entities.tbl_ThamDoYKien_Ip.Find(id);
        }

        public List<tbl_ThamDoYKien_Ip> GetAll()
        {
            return _entities.tbl_ThamDoYKien_Ip.ToList();
        }
    }
}
