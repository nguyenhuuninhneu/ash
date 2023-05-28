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
    public class DMNhomRepository : IDMNhomRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "CacheChucVu";
        public void Add(Model.tbl_DMNhom obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_DMNhom.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_DMNhom.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_DMNhom obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_DMNhom Find(int id)
        {
            return _entities.tbl_DMNhom.Find(id);
        }

        public List<Model.tbl_DMNhom> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_DMNhom>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_DMNhom.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
