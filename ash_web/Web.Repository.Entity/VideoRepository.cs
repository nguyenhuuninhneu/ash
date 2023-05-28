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
    public class VideoRepository : IVideoRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachevideo"; 
        public void Add(Model.tbl_Video obj)
        {
            try {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_Video.Add(obj);
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
            _entities.tbl_Video.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Video obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_Video Find(int id)
        {
            return _entities.tbl_Video.Find(id);
        }

        public IEnumerable<Model.tbl_Video> GetAll()
        {
            var lstData = _entities.tbl_Video.ToList();
            return lstData;
        }
    }
}
