using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;
using Web.Repository;

namespace Web.Repository.Entity
{
    public class LinkWebsiteRepository: ILinkWebsiteRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachelinkwebsite";
        public void Add(Model.tbl_LinkWebsite obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                _entities.tbl_LinkWebsite.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
           
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_LinkWebsite.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_LinkWebsite obj)
        {
            try
            {
                HelperCache.RemoveCache(KeyCache);
                _entities.Entry(obj).State = EntityState.Modified;
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
          
        }

        public Model.tbl_LinkWebsite Find(int id)
        {
            return _entities.tbl_LinkWebsite.Find(id);
        }

        public List<Model.tbl_LinkWebsite> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_LinkWebsite>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_LinkWebsite.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
