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
    public class TTHCRepository : ITTHCRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_TTHC obj)
        {
            _entities.tbl_TTHC.Add(obj);
            _entities.SaveChanges();
        }
        
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_TTHC.Remove(obj);
            _entities.SaveChanges();
        }
        
        public void Edit(Model.tbl_TTHC obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        
        public Model.tbl_TTHC Find(int id)
        {
            return _entities.tbl_TTHC.Find(id);
        }
        
        public IEnumerable<Model.tbl_TTHC> GetAll()
        {
            return _entities.tbl_TTHC;
        }
        
        public IEnumerable<Model.tbl_TTHCComment> GetAllCMT()
        {
            return _entities.tbl_TTHCComment.OrderByDescending(g => g.CreateDate);
        }
        public void AddCMT(Model.tbl_TTHCComment obj)
        {
            _entities.tbl_TTHCComment.Add(obj);
            _entities.SaveChanges();
        }

        public void DeleteCMT(int id)
        {
            var obj = FindCMT(id);
            _entities.tbl_TTHCComment.Remove(obj);
            _entities.SaveChanges();
        }

        public void EditCMT(Model.tbl_TTHCComment obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public Model.tbl_TTHCComment FindCMT(int id)
        {
            return _entities.tbl_TTHCComment.Find(id);
        }
    }
}
