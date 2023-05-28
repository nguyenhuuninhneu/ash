using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq; 
using Web.Model;

namespace Web.Repository.Entity
{
    public class TopicRepository : ITopicRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_Topic obj)
        {
            try {
                _entities.tbl_Topic.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
          
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Topic.Remove(obj);
            _entities.SaveChanges();
        }

        public bool Edit(Model.tbl_Topic obj)
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
        public void Edit(List<Model.tbl_Topic> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public Model.tbl_Topic Find(int id)
        {
            return _entities.tbl_Topic.Find(id);
        }

        public IEnumerable<Model.tbl_Topic> GetAll()
        {
            var lst = _entities.tbl_Topic.ToList().OrderByDescending(x => x.CreatedDate);
            return lst;
        }
        #region Comment
        public IEnumerable<Model.tbl_TopicComment> GetAllCMT()
        {
            return _entities.tbl_TopicComment.OrderByDescending(g => g.CreateDate);
        }
        public void AddCMT(Model.tbl_TopicComment obj)
        {
            _entities.tbl_TopicComment.Add(obj);
            _entities.SaveChanges();
        }

        public void DeleteCMT(int id)
        {
            var obj = FindCMT(id);
            _entities.tbl_TopicComment.Remove(obj);
            _entities.SaveChanges();
        }

        public void EditCMT(Model.tbl_TopicComment obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public Model.tbl_TopicComment FindCMT(int id)
        {
            return _entities.tbl_TopicComment.Find(id);
        }
        #endregion
    }
}
