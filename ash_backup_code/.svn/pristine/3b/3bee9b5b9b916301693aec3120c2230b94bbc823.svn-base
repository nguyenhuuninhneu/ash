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
    public class QALinhVucRepository : IQALinhVucRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cacheQALinhVuc";
        public void Add(Model.tbl_QALinhVuc obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_QALinhVuc.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_QALinhVuc.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_QALinhVuc obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_QALinhVuc Find(int id)
        {
            return _entities.tbl_QALinhVuc.Find(id);
        }

        public List<Model.tbl_QALinhVuc> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_QALinhVuc>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_QALinhVuc.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
