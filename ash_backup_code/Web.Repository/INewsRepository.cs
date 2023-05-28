using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface INewsRepository
    {
        Dictionary<List<tbl_News_Custom>, string> spGetListNews(int page = 1, int pagesize = 10, string field = "*", string table = "tbl_news", string condition = "", string orderby = "ORDER BY CreatedDate DESC");
        List<tbl_News_Custom> spGetOtherNews(DateTime pObjNewsCreatedDate, int pObjNewsId = 0, string fullCatIdStr = "", int Pagesize = 10);
        IEnumerable<View_DataHome> GetDataHome();
        IEnumerable<tbl_News> GetAll();
        IEnumerable<view_News> GetAll_View();
        IEnumerable<tbl_News> TestGetAll();
        IEnumerable<view_News_Public> GetAll_View_Public();
        IEnumerable<view_News_noPublic> GetAll_View_noPublic();
        IEnumerable<view_News_hotAndUnderhot> GetAll_View_Hot();
        IEnumerable<view_News_Sukien> GetAll_View_SuKien();
        IEnumerable<view_News_Limit> GetAll_View_Limit();
        IEnumerable<view_NEWS_LISTDATA> Get_News_ListData();
        tbl_News Find(int id);
        void Add(tbl_News obj);
        bool Edit(tbl_News obj);
        void Edit(List<tbl_News> lstobj);
        void Delete(int id);

        IEnumerable<tbl_NewsComment> GetAllCMT();
        tbl_NewsComment FindCMT(int id);
        void AddCMT(tbl_NewsComment obj);
        void EditCMT(tbl_NewsComment obj);
        void DeleteCMT(int id);
    }
}
