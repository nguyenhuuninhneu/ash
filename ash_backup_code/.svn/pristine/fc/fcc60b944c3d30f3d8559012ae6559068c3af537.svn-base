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
    public class DanhMucTaiLieuRepository : IDanhMucTaiLieuRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public IEnumerable<tbl_DanhMucTaiLieu> GetAll()
        {
            return _entities.tbl_DanhMucTaiLieu;
        }
        public bool Add(tbl_DanhMucTaiLieu obj)
        {
            try
            {
                _entities.tbl_DanhMucTaiLieu.Add(obj);
                _entities.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //WriteLog.Write(ex.ToString());
                return false;
            }
        }
        public bool Edit(tbl_DanhMucTaiLieu obj)
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
        //public tbl_DanhMucTaiLieu Find(int id)
        //{
        //    return _entities.tbl_DanhMucTaiLieu.Find(id);
        //}
        public Model.tbl_DanhMucTaiLieu Find(int id)
        {
            return _entities.tbl_DanhMucTaiLieu.Find(id);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_DanhMucTaiLieu.Remove(obj);
            _entities.SaveChanges();
        }
        //public bool Delete(tbl_DanhMucTaiLieu obj)
        //{
        //    try
        //    {
        //        _entities.tbl_DanhMucTaiLieu.Remove(obj);
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
