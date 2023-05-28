using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class Entity_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public IEnumerable<dynamic> Query(string tableName, string sql, params object[] parameters)
        {
            var dbSetType = Type.GetType($"<Web.Model.{tableName}>");
            var method = typeof(Entity_Repository).GetMethod("GenericQuery", new Type[] { typeof(string), typeof(object[]) });
            method = method.MakeGenericMethod(dbSetType);
            return (IEnumerable<dynamic>)method.Invoke(this, new object[] { sql, parameters });
        }
        public virtual IEnumerable<T> GenericQuery<T>(string sql, params object[] parameters) where T : class
        {
            var dbSet = (DbSet<T>)typeof(Entity_Repository).GetProperty(typeof(T).Name).GetValue(this, null);
            return dbSet.SqlQuery(sql, parameters);
        }
    }
}
