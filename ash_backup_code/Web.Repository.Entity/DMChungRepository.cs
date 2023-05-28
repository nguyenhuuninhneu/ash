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
    public class DMChungRepository : IDMChungRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public IEnumerable<tbl_DMChung> GetAll()
        {
            return _entities.tbl_DMChung;
        }
        public bool Add(tbl_DMChung obj)
        {
            try
            {
                _entities.tbl_DMChung.Add(obj);
                _entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //WriteLog.Write(ex.ToString());
                return false;
            }
        }
        public bool Edit(tbl_DMChung obj)
        {
            try
            {
                _entities.Entry(obj).State = System.Data.Entity.EntityState.Detached;
                _entities.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                _entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //WriteLog.Write(ex.ToString());
                return false;
            }
        }
        //public tbl_DMChung Find(int id)
        //{
        //    return _entities.tbl_DMChung.Find(id);
        //}
        public Model.tbl_DMChung Find(int id)
        {
            return _entities.tbl_DMChung.Find(id);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_DMChung.Remove(obj);
            _entities.SaveChanges();
        }
        //public bool Delete(tbl_DMChung obj)
        //{
        //    try
        //    {
        //        _entities.tbl_DMChung.Remove(obj);
        //        _entities.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //WriteLog.Write(ex.ToString());
        //        return false;
        //    }
        //}
    }
}
