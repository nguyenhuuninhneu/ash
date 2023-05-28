using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class BoxNewsConfigController : BaseController
    {
        readonly IBoxNewsConfigRepository _boxNewsConfigRepository = new BoxNewsConfigRepository();
        readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        //
        // GET: /Admin/BoxNewsConfig/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstLang = _languagesRepository.GetAll();
            TempData["Languages"] = lstLang;
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string LangCode, int PageElementId = 0, int page = 1)
        {
            var lstLang = _languagesRepository.GetAll();
            TempData["Languages"] = lstLang;
            var lstPageElements = _pageElementRepository.GetAll();
            TempData["PageElement"] = lstPageElements;
            var lstBoxNewsConfig = _boxNewsConfigRepository.GetAll();
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstBoxNewsConfig = lstBoxNewsConfig.Where(g => g.LangCode == LangCode).ToList();
            }
            else
            {
                PageElementId = 0;
            }
            if (PageElementId > 0)
            {
                lstBoxNewsConfig = lstBoxNewsConfig.Where(g => g.PageElementId == PageElementId).ToList();
            }
            lstBoxNewsConfig = lstBoxNewsConfig.OrderBy(g => g.PageElementId).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/BoxNewsConfig/_ListData.cshtml", lstBoxNewsConfig),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/BoxNewsConfig/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_BoxNewsConfig obj)
        {
            try
            {
                _boxNewsConfigRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới vùng hiển thị: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới vùng hiển thị thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới vùng hiển thị thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(string Code, int PageElementId, string LangCode)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var lstpageElement = _pageElementRepository.GetAll().ToList();
            TempData["PageElement"] = lstpageElement;
            var obj = _boxNewsConfigRepository.Find(Code, PageElementId, LangCode);
            return Json(RenderViewToString("~/Areas/Admin/Views/BoxNewsConfig/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_BoxNewsConfig obj)
        {
            try
            {
                _boxNewsConfigRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật vùng hiển thị: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật vùng hiển thị thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật vùng hiển thị thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(string Code, int PageElementId, string LangCode)
        {
            try
            {
                var obj = _boxNewsConfigRepository.Find(Code, PageElementId, LangCode);
                _boxNewsConfigRepository.Delete(Code, PageElementId, LangCode);
                LogController.AddLog(string.Format("Xóa vùng hiển thị: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa vùng hiển thị thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa vùng hiển thị thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult GetListConfigByCatId(int catid)
        //{
        //    if (catid > 0)
        //    {
        //        var objCat = _categoryRepository.Find(catid);
        //        var pageElementId = objCat.PageElementId;
        //        var lstBoxNewsConfig = _boxNewsConfigRepository.GetAll().Where(g => g.PageElementId == pageElementId).ToList();
        //        return Json(lstBoxNewsConfig, JsonRequestBehavior.AllowGet);
        //    }
        //    return Json(new List<tbl_BoxNewsConfig>(), JsonRequestBehavior.AllowGet);
        //}
    }
}
