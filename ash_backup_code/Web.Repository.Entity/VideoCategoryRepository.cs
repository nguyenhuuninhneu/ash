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
    public class VideoCategoryRepository : IVideoCategoryRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachevideo"; 
        public void Add(Model.tbl_VideoCategory obj)
        {
            try {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_VideoCategory.Add(obj);
                _entities.SaveChanges();
            }
            catch(Exception ex)
            {

            }
           
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_VideoCategory.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_VideoCategory obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                _entities.Entry(obj).State = EntityState.Modified;
                _entities.SaveChanges();
            }
            catch(Exception ex)
            {

            }
           
        }

        public Model.tbl_VideoCategory Find(int id)
        {
            return _entities.tbl_VideoCategory.Find(id);
        }

        public IEnumerable<Model.tbl_VideoCategory> GetAll()
        {
            var lstData = _entities.tbl_VideoCategory.ToList();
            return lstData;
        }
    }
}
