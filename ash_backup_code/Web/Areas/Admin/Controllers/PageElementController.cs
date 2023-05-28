using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class PageElementController : BaseController
    {
        //
        // GET: /Admin/PageElement/
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {
            var lstPageElement = _pageElementRepository.GetAll();

            var lstLang = _languagesRepository.GetAll().ToList();
            foreach (var item in lstPageElement)
            {
                var firstOrDefault = lstLang.FirstOrDefault(g => g.Code == item.LangCode);
                if (firstOrDefault != null)
                    item.LangName = firstOrDefault.Name;
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/PageElement/_ListData.cshtml", lstPageElement),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        public ActionResult GetAllByLangCode(string LangCode)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var arrPageElementId = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstpageElements = _pageElementRepository.GetAll().Where(g=>arrPageElementId.Contains(g.ID)).ToList();
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstpageElements = lstpageElements.Where(g => g.LangCode == LangCode).ToList();
            }
            return Json(lstpageElements.ToList(), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/PageElement/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(tbl_PageElement obj)
        {
            try
            {
                _pageElementRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới thông tin trang: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới thông tin trang thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới thông tin trang thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var objPageElement = _pageElementRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/PageElement/_Edit.cshtml", objPageElement), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(tbl_PageElement obj)
        {
            try
            {
                _pageElementRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật thông tin trang: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật link thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật thông tin trang thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _pageElementRepository.Find(id);
                _pageElementRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa thông tin trang: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa thông tin trang thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa thông tin trang thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    _pageElementRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} thông tin trang", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
