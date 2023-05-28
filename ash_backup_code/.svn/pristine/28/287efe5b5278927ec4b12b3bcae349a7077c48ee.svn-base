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
    public class NewsPaperRepository : INewsPaperRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_NewsPaper obj)
        {
            _entities.tbl_NewsPaper.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_NewsPaper.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_NewsPaper obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_NewsPaper Find(int id)
        {
            return _entities.tbl_NewsPaper.Find(id);
        }
        public IEnumerable<Model.tbl_NewsPaper> GetAll()
        {
            return _entities.tbl_NewsPaper.OrderByDescending(g => g.CreateDate);
        }

        public void AddCmt(Model.tbl_NewsPaperComment obj)
        {
            _entities.tbl_NewsPaperComment.Add(obj);
            _entities.SaveChanges();
        }

        public void DeleteCmt(int id)
        {
            var obj = FindCmt(id);
            _entities.tbl_NewsPaperComment.Remove(obj);
            _entities.SaveChanges();
        }
        public void EditCmt(Model.tbl_NewsPaperComment obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_NewsPaperComment FindCmt(int id)
        {
            return _entities.tbl_NewsPaperComment.Find(id);
        }
        public IEnumerable<Model.tbl_NewsPaperComment> GetAllCmt()
        {
            return _entities.tbl_NewsPaperComment.OrderByDescending(g => g.CreateDate);
        }
    }
}
