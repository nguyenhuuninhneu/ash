using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 
using Web.Model;

namespace Web.Repository.Entity
{
    public class MagazineRepository : IMagazineRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_Magazine obj)
        {
            try {
                _entities.tbl_Magazine.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
          
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Magazine.Remove(obj);
            _entities.SaveChanges();
        }

        public bool Edit(Model.tbl_Magazine obj)
        {
            try
            {
                _entities.Entry(obj).State = EntityState.Modified;
                _entities.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }
        public void Edit(List<Model.tbl_Magazine> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public Model.tbl_Magazine Find(int id)
        {
            return _entities.tbl_Magazine.Find(id);
        }

        public IEnumerable<Model.tbl_Magazine> GetAll()
        {
            var lst = _entities.tbl_Magazine.ToList().OrderByDescending(x => x.CreatedDate);
            return lst;
        }
         
    }
}
