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
    public class ImagesRepository : IImagesRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "NewsImages";
        public void Add(Model.tbl_Images obj)
        {
            try {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_Images.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
            
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_Images.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Images obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_Images Find(int id)
        {
            return _entities.tbl_Images.Find(id);
        }

        public List<Model.tbl_Images> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_Images>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_Images.OrderByDescending(g=>g.CreatedDate).ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
        public IEnumerable<view_Images_ImageCategory> GetAllView()
        {
            var lstData = _entities.view_Images_ImageCategory.ToList();
            return lstData;
        }
    }
}
