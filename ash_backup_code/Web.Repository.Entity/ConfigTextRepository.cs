using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository.Entity
{
    public class ConfigTextRepository : IConfigTextRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public IEnumerable<tbl_ConfigText> GetAll()
        {
            return _entities.tbl_ConfigText; 
        }

        public void Add(tbl_ConfigText obj)
        {
            _entities.tbl_ConfigText.Add(obj);
            _entities.SaveChanges();
        }

        public void Edit(tbl_ConfigText obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public tbl_ConfigText Find(int id)
        {
            return _entities.tbl_ConfigText.Find(id);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_ConfigText.Remove(obj);
            _entities.SaveChanges();
        }
    }
}
