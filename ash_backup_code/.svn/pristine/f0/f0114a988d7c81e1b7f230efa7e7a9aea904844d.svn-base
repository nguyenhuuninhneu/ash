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
    public class LichCongTacRepository : ILichCongTacRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "CacheLichCongTac";
        public void Add(tbl_LichCongTac obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_LichCongTac.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_LichCongTac.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_LichCongTac obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_LichCongTac Find(int id)
        {
            return _entities.tbl_LichCongTac.Find(id);
        }

        public List<Model.tbl_LichCongTac> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_LichCongTac>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_LichCongTac.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
