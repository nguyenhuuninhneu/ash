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
    public class BoxNewsConfigRepository : IBoxNewsConfigRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "BoxNewsConfig";
        public List<tbl_BoxNewsConfig> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_BoxNewsConfig>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_BoxNewsConfig.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
        public void Add(Model.tbl_BoxNewsConfig obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_BoxNewsConfig.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(string Code, int PageElementId, string LangCode)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(Code, PageElementId, LangCode);
            _entities.tbl_BoxNewsConfig.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_BoxNewsConfig obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public tbl_BoxNewsConfig Find(string Code, int PageElementId, string LangCode)
        {
            return _entities.tbl_BoxNewsConfig.FirstOrDefault(g => g.Code == Code && g.PageElementId == PageElementId && g.LangCode == LangCode);
        }
    }
}
