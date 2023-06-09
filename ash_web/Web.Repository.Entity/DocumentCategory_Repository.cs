﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class DocumentCategory_Repository:IDocumentCategory_Repository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(document_category obj)
        {
            _entities.document_category.Add(obj);
            _entities.SaveChanges();
        }


        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.document_category.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(document_category obj)
        {
            var entity = _entities.document_category.Where(c => c.ID == obj.ID).AsQueryable().FirstOrDefault();
            if (entity == null)
            {
                _entities.document_category.Add(obj);
            }
            else
            {
                _entities.Entry(entity).CurrentValues.SetValues(obj);
            }
            _entities.SaveChanges();

        }

        public document_category Find(int id)
        {
            return _entities.document_category.Find(id);
        }

        public List<document_category> GetAll()
        {
            return _entities.document_category.ToList();
        }
        public IEnumerable<document_category> GetAllByTableName(string tableName = "document_category")
        {
            var sql = string.Format("SELECT * FROM [{0}]", tableName);
            var query = _entities.Database.SqlQuery<document_category>(sql);
            return query;
        }
    }
}
