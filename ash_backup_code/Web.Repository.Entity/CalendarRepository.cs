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
    public class CalendarRepository : ICalendarRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        private const string KeyCache = "cachecalendar";
        public void Add(Model.tbl_Calendar obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_Calendar.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_Calendar.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Calendar obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_Calendar Find(int id)
        {
            return _entities.tbl_Calendar.Find(id);
        }

        public List<Model.tbl_Calendar> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_Calendar>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_Calendar.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
