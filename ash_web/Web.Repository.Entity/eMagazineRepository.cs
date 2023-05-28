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
    public class eMagazineRepository : IeMagazineRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "NewseMagazine";
        public void Add(Model.tbl_eMagazine obj)
        {
            try {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_eMagazine.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
            
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_eMagazine.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_eMagazine obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_eMagazine Find(int id)
        {
            return _entities.tbl_eMagazine.Find(id);
        }

        public List<Model.tbl_eMagazine> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_eMagazine>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_eMagazine.OrderByDescending(g=>g.CreatedDate).ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
