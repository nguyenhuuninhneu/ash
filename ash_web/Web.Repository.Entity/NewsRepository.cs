using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class NewsRepository : INewsRepository
    {
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public void Add(Model.tbl_News obj)
        {
            try {
                _entities.tbl_News.Add(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
          
        }
        public IEnumerable<View_DataHome> GetDataHome()
        {
            return _entities.View_DataHome;
        }
        public Dictionary<List<tbl_News_Custom>,string> spGetListNews(int page = 1, int pagesize = 10, string field = "*", string table = "tbl_news", string condition = "", string orderby = "ORDER BY CreatedDate DESC")
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@page",page),
                new SqlParameter("@pagesize", pagesize),
                new SqlParameter("@field", field),
                new SqlParameter("@table", table),
                new SqlParameter("@condition", condition),
                new SqlParameter("@orderby", orderby),
                new SqlParameter("@total",SqlDbType.Int){Direction = ParameterDirection.Output}
            };
            var listNews = _entities.Database.SqlQuery<tbl_News_Custom>("NewsGetAll @page,@pagesize,@field,@table,@condition,@orderby,@total out", sqlParams).ToList();
            var DicLevel = new Dictionary<List<tbl_News_Custom>, string>();
            var totalNews = sqlParams[6].Value.ToString();
            DicLevel.Add(listNews, totalNews);
            return DicLevel;
        }
        public List<tbl_News_Custom> spGetOtherNews(DateTime pObjNewsCreatedDate, int pObjNewsId = 0, string fullCatIdStr = "", int Pagesize = 10)
        {
            var sqlParams = new SqlParameter[]
            {
                new SqlParameter("@pObjNewsCreatedDate", pObjNewsCreatedDate),
                new SqlParameter("@pObjNewsId", pObjNewsId),
                new SqlParameter("@pCategoryIdStr", fullCatIdStr),
                new SqlParameter("@pPageSize", Pagesize),
            };
            var listNews = _entities.Database.SqlQuery<tbl_News_Custom>("NewsOther @pObjNewsCreatedDate,@pObjNewsId,@pCategoryIdStr,@pPageSize", sqlParams).ToList();
            return listNews;
        }

        public void Delete(int id)
        {
            try
            {
                var obj = Find(id);
                _entities.tbl_News.Remove(obj);
                _entities.SaveChanges();
            }catch(Exception ex)
            {

            }
           
        }

        public bool Edit(Model.tbl_News obj)
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
                    var set = _entities.Set<tbl_News>();
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
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void Edit(List<Model.tbl_News> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public IEnumerable<view_News> GetAll_View()
        {
            return _entities.view_News.OrderByDescending(g => g.CreatedDate);
        }
        public IEnumerable<view_News_Public> GetAll_View_Public()
        {
            return _entities.view_News_Public.OrderByDescending(g => g.CreatedDate);
        }
        public IEnumerable<view_News_noPublic> GetAll_View_noPublic()
        {
            return _entities.view_News_noPublic.OrderByDescending(g => g.CreatedDate);
        }
        public IEnumerable<view_News_hotAndUnderhot> GetAll_View_Hot()
        {
            return _entities.view_News_hotAndUnderhot.OrderByDescending(g => g.CreatedDate);
        }
        public IEnumerable<view_News_Sukien> GetAll_View_SuKien()
        {
            return _entities.view_News_Sukien.OrderByDescending(g => g.CreatedDate);
        }
        public IEnumerable<view_News_Limit> GetAll_View_Limit()
        {
            return _entities.view_News_Limit.OrderByDescending(g => g.CreatedDate);
        }
        public IEnumerable<view_NEWS_LISTDATA> Get_News_ListData()
        {
            return _entities.view_NEWS_LISTDATA.OrderByDescending(g => g.CreatedDate);
        }
        public tbl_News Find(int id)
        {     
           //return _entities.tbl_News.SingleOrDefault(x => x.ID == id && x.IsDelete == 0);
           return _entities.tbl_News.Find(id);
        }

        public IEnumerable<Model.tbl_News> GetAll()
        {
            return _entities.tbl_News.ToList().OrderByDescending(x=>x.CreatedDate);
            
        }
        public IEnumerable<Model.tbl_News> TestGetAll()
        {
            return _entities.tbl_News.Where(x=>x.ID >1 && x.ID < 300 && (x.NewMoney == 11 || x.NewMoney == 13 || x.NewMoney == 14));

        }
        #region Comment
        public IEnumerable<Model.tbl_NewsComment> GetAllCMT()
        {
            return _entities.tbl_NewsComment.OrderByDescending(g => g.CreateDate);
        }
        public void AddCMT(Model.tbl_NewsComment obj)
        {
            _entities.tbl_NewsComment.Add(obj);
            _entities.SaveChanges();
        }

        public void DeleteCMT(int id)
        {
            var obj = FindCMT(id);
            _entities.tbl_NewsComment.Remove(obj);
            _entities.SaveChanges();
        }

        public void EditCMT(Model.tbl_NewsComment obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public Model.tbl_NewsComment FindCMT(int id)
        {
            return _entities.tbl_NewsComment.Find(id);
        }
        #endregion
    }
}
