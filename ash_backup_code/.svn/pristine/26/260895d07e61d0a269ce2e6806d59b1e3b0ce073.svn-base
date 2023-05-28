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
    public class ImageCategoryRepository : IImageCategoryRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cacheGalleryCategory";
        public void Add(Model.tbl_ImagesCategory obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_ImagesCategory.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
           
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_ImagesCategory.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_ImagesCategory obj)
        {
            try
            {
                _entities.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                _entities.SaveChanges();
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public Model.tbl_ImagesCategory Find(int id)
        {
            return _entities.tbl_ImagesCategory.Find(id);
        }

        public List<tbl_ImagesCategory> GetAll()
        {
             
           return  _entities.tbl_ImagesCategory.ToList();
                
        }
    }
}
