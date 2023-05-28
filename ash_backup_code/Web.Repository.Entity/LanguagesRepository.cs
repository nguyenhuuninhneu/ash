using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class LanguagesRepository : ILanguagesRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachelangguages"; 
        public IEnumerable<tbl_Languages> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_Languages>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_Languages.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
