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
    public class qProcedureRepository : IqProcedureRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "CacheQuyTrinhXuatBan";
        public void Add(Model.qProcedure obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.qProcedures.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.qProcedures.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.qProcedure obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.qProcedure Find(int id)
        {
            return _entities.qProcedures.Find(id);
        }

        public List<Model.qProcedure> GetAll()
        {
            var lstData = HelperCache.GetCache<List<qProcedure>>(KeyCache);
            try
            {
                if (lstData == null)
                {
                    lstData = _entities.qProcedures.ToList();
                    HelperCache.AddCache(lstData, KeyCache);
                }
            }
            catch (Exception)
            {

            }
            
            return lstData;
        }
    }
}
