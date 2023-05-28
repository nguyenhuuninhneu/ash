using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ChatRepository : IChatRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_Chat obj)
        {
            _entities.tbl_Chat.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Chat.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Chat obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public void Edit(List<Model.tbl_Chat> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }

        public Model.tbl_Chat Find(int id)
        {
            return _entities.tbl_Chat.Find(id);
        }

        public IEnumerable<Model.tbl_Chat> GetAll()
        {
            return _entities.tbl_Chat;
        }
    }
}
