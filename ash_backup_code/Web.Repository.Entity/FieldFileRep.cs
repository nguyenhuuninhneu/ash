using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Core;
namespace Web.Repository.Entity
{
   public class FieldFileRep: IFieldFileRep
    {

        readonly PortalNewsEntities _db = new PortalNewsEntities();
        public IEnumerable<tbl_fieldfiles> GetData()
        {
            using(var _db2 = new PortalNewsEntities())
            {
                return _db2.tbl_fieldfiles.ToList();
            }
            
        }
        public IEnumerable<tbl_fieldfiles> GetFiles(int FieldID)
        {
            return _db.tbl_fieldfiles.Where(g => g.FieldID == FieldID);
        }
        public tbl_fieldfiles Find(int Id)
        {
            return _db.tbl_fieldfiles.Find(Id);
        }
        public bool Create(tbl_fieldfiles obj)
        {
            _db.tbl_fieldfiles.Add(obj);
            _db.SaveChanges();
            return true;
        }
        public bool Edit(tbl_fieldfiles obj)
        {
            _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        public bool DeletePermanently(int Id)
        {
            try
            {
                var obj = Find(Id);
                _db.tbl_fieldfiles.Remove(obj);
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        public bool Delete(int Id)
        {
            try
            {
                var obj = Find(Id);
                obj.Status = 0;
                _db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
    }
}