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
    public class AdminMenuRepository : IAdminMenuRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cacheadminmenu"; 
        public void Add(Model.tbl_AdminMenu obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_AdminMenu.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_AdminMenu.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_AdminMenu obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_AdminMenu Find(int id)
        {
            return _entities.tbl_AdminMenu.Find(id);
        }

        public List<Model.tbl_AdminMenu> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_AdminMenu>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_AdminMenu.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
