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
    public class SubeMagazineRepository : ISubeMagazineRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "NewsSubeMagazine";
        public void Add(Model.tbl_SubeMagazine obj)
        {
            try {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_SubeMagazine.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
            
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_SubeMagazine.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_SubeMagazine obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_SubeMagazine Find(int id)
        {
            return _entities.tbl_SubeMagazine.Find(id);
        }

        public List<Model.tbl_SubeMagazine> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_SubeMagazine>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_SubeMagazine.OrderBy(g=>g.Ordering).ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
