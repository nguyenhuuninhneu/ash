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
    public class TruyNaRepository : ITruyNaRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachetruyna";
        public void Add(Model.tbl_truyna obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_truyna.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_truyna.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_truyna obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_truyna Find(int id)
        {
            return _entities.tbl_truyna.Find(id);
        }

        public List<Model.tbl_truyna> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_truyna>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_truyna.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }

        public IEnumerable<tbl_ToGiac> GetAllCMT()
        {
            return _entities.tbl_ToGiac.OrderByDescending(g => g.CreateDate);
        }

        public tbl_ToGiac FindCMT(int id)
        {
          
            return _entities.tbl_ToGiac.Find(id);
        }

        public void DeleteCMT(int id)
        {
            var obj = FindCMT(id);
            _entities.tbl_ToGiac.Remove(obj);
            _entities.SaveChanges();
        }
    }
}
