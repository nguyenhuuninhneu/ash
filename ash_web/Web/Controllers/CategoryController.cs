using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class CategoryController : BaseController
    {
        //
        // GET: /QA/
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly ICategoryRepository _cateRepository = new CategoryRepository();
        readonly ILimitRepository _limitRepository = new LimitRepository();
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        public ActionResult Index(int id)
        {
            TempData["ID"] = id;
            return View();
        }

        [HttpPost]
        public ActionResult ListData(int id, string Title = "", string categoryId = "", string Position = "", int Status = 0, int page = 1)
        {
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
                langCode = Session["langCodeHome"].ToString();

            var objLimit = _limitRepository.GetAll().FirstOrDefault(g => g.Code == "danh-sach-tin-bai");

            var allCat = _cateRepository.GetAll();
            var fullCatId = Common.GetChild(allCat, id);
            var fullCatIdStr = string.Join(",",fullCatId);
            TempData["fullCatIdStr"] = fullCatIdStr;
            // phan trang store procedure
            var pagesize = (objLimit != null ? Convert.ToInt32(objLimit.Value) : 15); // limit
            var condition = " WHERE 1=1 AND IsPublish = 'True' AND CreatedDate <= GETDATE()";
            if (!string.IsNullOrEmpty(fullCatIdStr))
            {
                // condition += " AND CategoryIdStr IN ('" + fullCatIdStr.Replace(",", "','") + "')";
                // condition += "AND ('" + fullCatIdStr.Replace(",", "' = ANY (SELECT Split.a.value('.', 'NVARCHAR(MAX)') id FROM (SELECT CAST('<X>' + REPLACE(CategoryIdStr, ',', '</X><X>') + '</X>' AS XML) AS String) AS A CROSS APPLY String.nodes('/X') AS Split(a)) OR '") + "' = ANY (SELECT Split.a.value('.', 'NVARCHAR(MAX)') id FROM (SELECT CAST('<X>' + REPLACE(CategoryIdStr, ',', '</X><X>') + '</X>' AS XML) AS String) AS A CROSS APPLY String.nodes('/X') AS Split(a)))"; // 1 tin bai nhieu danh muc -- XIN DUNG XOA
                condition += " AND (CateComma like '%," + fullCatIdStr.Replace(",", ",%' OR CateComma like '%,") + ",%' )";
            }
            var orderby = "ORDER BY CreatedDate DESC";
            var field = "ID,Title,Photo,Description,CategoryIdStr,CreatedDate,IsPublish,CateComma";
            var DicLevel = _newsRepository.spGetListNews(page, pagesize, field,"view_News_MultiCate", condition, orderby);
            var lstNews = new List<tbl_News_Custom>();
            var totalNews = 0;
            if (DicLevel.Count > 0)
            {
                var objDic = DicLevel.FirstOrDefault();
                lstNews = objDic.Key;
                totalNews = Convert.ToInt32(objDic.Value);
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Category/_ListData.cshtml", lstNews),
               totalPages = Math.Ceiling(((double)totalNews / pagesize)),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
