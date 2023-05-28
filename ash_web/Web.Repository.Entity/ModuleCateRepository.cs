using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 
using Web.Model;

namespace Web.Repository.Entity
{
    public class ModuleCateRepository : IModuleCateRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_ModuleCate obj)
        {
            try {
                _entities.tbl_ModuleCate.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
          
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_ModuleCate.Remove(obj);
            _entities.SaveChanges();
        }

        public bool Edit(Model.tbl_ModuleCate obj)
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
        public void Edit(List<Model.tbl_ModuleCate> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public Model.tbl_ModuleCate Find(int id)
        {
            return _entities.tbl_ModuleCate.Find(id);
        }

        public IEnumerable<Model.tbl_ModuleCate> GetAll()
        {
            var lst = _entities.tbl_ModuleCate.ToList();
            return lst;
        } 
    }
}
