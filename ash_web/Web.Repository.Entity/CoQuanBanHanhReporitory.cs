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
    public class CoQuanBanHanhReporitory : ICoQuanBanHanhReporitory
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachecoquanbanhanh";
        public void Add(Model.tbl_CoQuanBanHanh obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_CoQuanBanHanh.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_CoQuanBanHanh.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_CoQuanBanHanh obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_CoQuanBanHanh Find(int id)
        {
            return _entities.tbl_CoQuanBanHanh.Find(id);
        }

        public List<Model.tbl_CoQuanBanHanh> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_CoQuanBanHanh>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_CoQuanBanHanh.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
