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
    public class LinhVucVanBanRepository : ILinhVucVanBanRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachelinhvucvanban";
        public void Add(Model.tbl_LinhVucVanBan obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_LinhVucVanBan.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_LinhVucVanBan.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_LinhVucVanBan obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_LinhVucVanBan Find(int id)
        {
            return _entities.tbl_LinhVucVanBan.Find(id);
        }

        public List<Model.tbl_LinhVucVanBan> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_LinhVucVanBan>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_LinhVucVanBan.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
