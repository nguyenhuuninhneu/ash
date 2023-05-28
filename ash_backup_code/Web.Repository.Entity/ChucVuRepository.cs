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
    public class ChucVuRepository : IChucVuRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "CacheChucVu";
        public void Add(Model.tbl_ChucVu obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_ChucVu.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_ChucVu.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_ChucVu obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_ChucVu Find(int id)
        {
            return _entities.tbl_ChucVu.Find(id);
        }

        public List<Model.tbl_ChucVu> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_ChucVu>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_ChucVu.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
