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
    public class FooterRepository : IFooterRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        //private const string KeyCache = "cacheFooter";
        public void Add(Model.tbl_Footer obj)
        {
            //HelperCache.RemoveCache(KeyCache);
            _entities.tbl_Footer.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            //HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_Footer.Remove(obj);
            _entities.SaveChanges();
        }

        public bool Edit(Model.tbl_Footer obj)
        {
            try
            {
                _entities.Entry(obj).State = EntityState.Modified;
                _entities.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
            
        }

        public Model.tbl_Footer Find(int id)
        {
            return _entities.tbl_Footer.Find(id);
        }

        public IEnumerable<Model.tbl_Footer> GetAll()
        {
            var lstData = _entities.tbl_Footer.ToList();
            //var lstData = HelperCache.GetCache<List<tbl_Footer>>(KeyCache);
            //if (lstData == null)
            //{
                //lstData = _entities.tbl_Footer.ToList();
                //HelperCache.AddCache(lstData, KeyCache);
            //}
            return lstData;
        }
    }
}
