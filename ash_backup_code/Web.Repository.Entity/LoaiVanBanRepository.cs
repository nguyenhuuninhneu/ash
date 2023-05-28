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
    public class LoaiVanBanRepository : ILoaiVanBanRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cacheloaivanban";
        public void Add(Model.tbl_LoaiVanBan obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_LoaiVanBan.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_LoaiVanBan.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_LoaiVanBan obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_LoaiVanBan Find(int id)
        {
            return _entities.tbl_LoaiVanBan.Find(id);
        }

        public List<Model.tbl_LoaiVanBan> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_LoaiVanBan>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_LoaiVanBan.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
