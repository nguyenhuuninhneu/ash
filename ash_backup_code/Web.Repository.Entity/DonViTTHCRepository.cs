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
    public class DonViTTHCRepository : IDonViTTHCRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachedonvitthc";
        
        public void Add(Model.tbl_DonViTTHC obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_DonViTTHC.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_DonViTTHC.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_DonViTTHC obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_DonViTTHC Find(int id)
        {
            return _entities.tbl_DonViTTHC.Find(id);
        }
        public List<Model.tbl_DonViTTHC> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_DonViTTHC>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_DonViTTHC.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
