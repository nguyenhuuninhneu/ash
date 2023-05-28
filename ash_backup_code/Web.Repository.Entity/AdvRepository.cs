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
    public class AdvRepository : IAdvRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
       
        public void Add(Model.tbl_Advert obj)
        {
            try {
                
                _entities.tbl_Advert.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
            
        }

        public void Delete(int id)
        {
           
            var obj = Find(id);
            _entities.tbl_Advert.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Advert obj)
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
                    var set = _entities.Set<tbl_Advert>();
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

        public Model.tbl_Advert Find(int id)
        {
            return _entities.tbl_Advert.Find(id);
        }

        public List<Model.tbl_Advert> GetAll()
        {
            return _entities.tbl_Advert.ToList();
        }
        public IEnumerable<dynamic> Query(string tableName, string sql, params object[] parameters)
        {
            var dbSetType = Type.GetType($"<Web.Model>.{tableName}");
            var method = typeof(AdvRepository).GetMethod("GenericQuery", new Type[] { typeof(string), typeof(object[]) });
            method = method.MakeGenericMethod(dbSetType);
            return (IEnumerable<dynamic>)method.Invoke(this, new object[] { sql, parameters });
        }

        public virtual IEnumerable<T> GenericQuery<T>(string sql, params object[] parameters) where T : class
        {
            var dbSet = (DbSet<T>)typeof(AdvRepository).GetProperty(typeof(T).Name).GetValue(this, null);
            return dbSet.SqlQuery(sql, parameters);
        }
    }
}
