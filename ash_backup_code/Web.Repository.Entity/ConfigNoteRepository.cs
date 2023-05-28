using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ConfigNoteRepository : IConfigNoteRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_configLuuY obj)
        {
            _entities.tbl_configLuuY.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_configLuuY.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_configLuuY obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_configLuuY Find(int id)
        {
            return _entities.tbl_configLuuY.Find(id);
        }


        public IEnumerable<Model.tbl_configLuuY> GetAll()
        {
            return _entities.tbl_configLuuY;
        }
    }
}
