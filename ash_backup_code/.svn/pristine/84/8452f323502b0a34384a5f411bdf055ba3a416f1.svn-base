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
    public class PageElementRepository : IPageElementRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        //private const string KeyCache = "cachepageelement";
        public void Add(Model.tbl_PageElement obj)
        {
            //HelperCache.RemoveCache(KeyCache);
            _entities.tbl_PageElement.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            //HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_PageElement.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_PageElement obj)
        {
            //HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_PageElement Find(int id)
        {
            return _entities.tbl_PageElement.Find(id);
        }

        public IEnumerable<Model.tbl_PageElement> GetAll()
        {
            var lstData = _entities.tbl_PageElement.ToList();
            //var lstData = HelperCache.GetCache<List<tbl_PageElement>>(KeyCache);
            //if (lstData == null)
            //{
                //lstData = _entities.tbl_PageElement.ToList();
                //HelperCache.AddCache(lstData, KeyCache);
            //}
            return lstData;
        }
    }
}
