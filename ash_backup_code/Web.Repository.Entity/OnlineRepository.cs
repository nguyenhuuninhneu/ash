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
    public class OnlineRepository : IOnlineRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();

        public void Add(Model.tbl_Online obj)
        {
            _entities.tbl_Online.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {            
            var obj = Find(id);
            _entities.tbl_Online.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Online obj)
        {            
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_Online Find(int id)
        {
            return _entities.tbl_Online.Find(id);
        }

        public IEnumerable<Model.tbl_Online> GetAll()
        {
           
            var lstData = _entities.tbl_Online.ToList();             
            return lstData;

        }
                
    }
}
