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
    public class ProcessRepository : IProcessRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "CacheQuyTrinhXuatBan";
        public void Add(Model.qProcess obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                _entities.qProcesses.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
           
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.qProcesses.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.qProcess obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                _entities.Entry(obj).State = EntityState.Modified;
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
           
        }

        public Model.qProcess Find(int id)
        {
            return _entities.qProcesses.Find(id);
        }

        public List<Model.qProcess> GetAll()
        {
            var lstData = HelperCache.GetCache<List<qProcess>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.qProcesses.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
