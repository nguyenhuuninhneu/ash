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
    public class HomeMenuRepository : IHomeMenuRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "HomeMenu";
        public void Add(Model.tbl_HomeMenu obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_HomeMenu.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_HomeMenu.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_HomeMenu obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_HomeMenu Find(int id)
        {
            return _entities.tbl_HomeMenu.Find(id);
        }

          public List<Model.tbl_HomeMenu> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_HomeMenu>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_HomeMenu.OrderBy(g=>g.Ordering).ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
