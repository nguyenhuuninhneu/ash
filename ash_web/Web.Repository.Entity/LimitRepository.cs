using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;
namespace Web.Repository.Entity
{
  public  class LimitRepository:ILimitRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public List<tbl_Limit> GetAll()
        {
            return _entities.tbl_Limit.ToList();
        }
        public void Add(tbl_Limit obj)
        {
            try
            {
                _entities.tbl_Limit.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {
                
            }
             
        }
        public tbl_Limit Find(int id)
        {
            return _entities.tbl_Limit.Find(id);
        }
        public void Edit(tbl_Limit obj)
        {
            try
            {
                if (obj == null)
                {
                    throw new ArgumentException("null");
                }

                var entry = _entities.Entry(obj);

                if (entry.State == EntityState.Detached)
                {
                    var set = _entities.Set<tbl_Limit>();
                    var attachedEntity = set.Local.SingleOrDefault(e => e.ID == obj.ID);

                    if (attachedEntity != null)
                    {
                        var attachedEntry = _entities.Entry(attachedEntity);
                        attachedEntry.CurrentValues.SetValues(obj);
                    }
                    else
                    {
                        entry.State = EntityState.Modified;
                    }
                }
                _entities.SaveChanges();
                return;
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Limit.Remove(obj);
            _entities.SaveChanges();
        }
        public List<tbl_Limit> GetListPosByCode(string code)
        {
            var obj = _entities.tbl_Limit.FirstOrDefault(g=>g.Code == code);
            if(obj != null)
            {
                var id = obj.ID;
                return _entities.tbl_Limit.Where(g => g.ParentID == id).OrderBy(g=>g.Ordering).ToList();
            }
            return null;
        }
    }
}
