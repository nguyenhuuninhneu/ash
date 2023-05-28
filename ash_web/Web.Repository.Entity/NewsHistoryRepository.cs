using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class NewsHistoryRepository : INewsHistoryRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_HistoryNews obj)
        {
            _entities.tbl_HistoryNews.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_HistoryNews.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_HistoryNews obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public void Edit(List<Model.tbl_HistoryNews> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }

        public Model.tbl_HistoryNews Find(int id)
        {
            return _entities.tbl_HistoryNews.Find(id);
        }

        public IEnumerable<Model.tbl_HistoryNews> GetAll()
        {
            return _entities.tbl_HistoryNews;
        }
    }
}
