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
    public class ContactInfoRepository : IContactInfoRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_ContactInfo obj)
        {
            _entities.tbl_ContactInfo.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_ContactInfo.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_ContactInfo obj)
        {
            try
            {
                _entities.Entry(obj).State = EntityState.Modified;
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
           
        }

        public Model.tbl_ContactInfo Find(int id)
        {
            return _entities.tbl_ContactInfo.Find(id);
        }


        public IEnumerable<Model.tbl_ContactInfo> GetAll()
        {
            return _entities.tbl_ContactInfo;
        }
    }
}
