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
    public class SlideImagesRepository : ISlideImagesRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "SlideImages";
        public void Add(Model.tbl_SlideImages obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_SlideImages.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_SlideImages.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_SlideImages obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_SlideImages Find(int id)
        {
            return _entities.tbl_SlideImages.Find(id);
        }

        public List<Model.tbl_SlideImages> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_SlideImages>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_SlideImages.OrderByDescending(g=>g.CreatedDate).ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
