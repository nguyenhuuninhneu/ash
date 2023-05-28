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
    public class CategoryRepository : ICategoryRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachecategories"; 
        private const string KeyCacheView = "cachecategories_view"; 
        public void Add(Model.tbl_Category obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                HelperCache.RemoveCache(KeyCacheView);
                _entities.tbl_Category.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
            
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            HelperCache.RemoveCache(KeyCacheView);
            var obj = Find(id);
            _entities.tbl_Category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Category obj)
        {
            //try
            //{
            //    _entities.Entry(obj).State = System.Data.Entity.EntityState.Detached;
            //    _entities.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            //    _entities.SaveChanges();
            //}catch(Exception ex)
            //{

            //}
            try
            {
                if (obj == null)
                {
                    throw new ArgumentException("null");
                }

                var entry = _entities.Entry(obj);

                if (entry.State == EntityState.Detached)
                {
                    var set = _entities.Set<tbl_Category>();
                    var attachedEntity = set.Local.SingleOrDefault(e => e.ID == obj.ID);

                    if (attachedEntity != null)
                    {
                        var attachedEntry = _entities.Entry(attachedEntity);
                        attachedEntry.CurrentValues.SetValues(obj);
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }
                }
                _entities.SaveChanges();
                return ;
            }
            catch (Exception ex)
            {
                return ;
            }
        }
        public List<view_Category_Languages> GetAllCategoryLanguageses()
        {
            var lstData = HelperCache.GetCache<List<view_Category_Languages>>(KeyCacheView);
            if (lstData == null)
            {
                lstData = _entities.view_Category_Languages.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }

        public Model.tbl_Category Find(int id)
        {
            return _entities.tbl_Category.Find(id);
        }
        public List<Model.tbl_Category> GetAll()
        {
            var lstData = new List<tbl_Category>();
            using (var _db = new PortalNewsEntities())
            {
                lstData = _db.tbl_Category.ToList();
            }
            
            return lstData;
        }
    }
}
