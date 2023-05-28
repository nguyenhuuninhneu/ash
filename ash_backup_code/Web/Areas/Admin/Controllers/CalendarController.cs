using System;
using System.Collections.Generic;
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
    public class CalendarController : BaseController
    {
        //
        // GET: /Admin/Calendar/
        readonly ICalendarRepository _calendarRepository = new CalendarRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            //var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            //var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID));
            //TempData["PageElement"] = lstpageElements.ToList();
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int PageElementId = 0, int page = 1)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            /*var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID));
            TempData["PageElement"] = lstpageElements.ToList();*/
            var lstLinkWebsite = _calendarRepository.GetAll().OrderByDescending(g=>g.ID).ToList();
            /*foreach (var item in lstLinkWebsite)
            {
                var firstOrDefault = lstLanguages.FirstOrDefault(g => g.Code == item.LangCode);
                if (firstOrDefault != null)
                    item.LangName = firstOrDefault.Name;
            }*/
            if (!string.IsNullOrEmpty(Name))
            {
                lstLinkWebsite =
                    lstLinkWebsite.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstLinkWebsite = lstLinkWebsite.ToList();
            }
            /*if (PageElementId > 0)
            {
                lstLinkWebsite = lstLinkWebsite.Where(g => g.PageElementId == PageElementId);
            }*/
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Calendar/_ListData.cshtml", lstLinkWebsite),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //var objUser = _userAdminRepository.Find(User.ID);
            //var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            //var lstLang = _languagesRepository.GetAll().ToList();
            //TempData["Languages"] = lstLang;
            //var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID) && g.LangCode == Webconfig.LangCodeVn).ToList();
            //TempData["PageElement"] = lstpageElements;
            return Json(RenderViewToString("~/Areas/Admin/Views/Calendar/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(tbl_Calendar obj)
        {
            try
            {
                obj.CreatedDate = DateTime.Now;
                _calendarRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới Lịch công tác: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới Lịch công tác thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới Lịch công tác thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objUser = _userAdminRepository.Find(User.ID);
           /* var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;*/
            var objLinkWebsite = _calendarRepository.Find(id);
            /*var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID) && g.LangCode == objLinkWebsite.LangCode);
            TempData["PageElement"] = lstpageElements.ToList();*/
            return Json(RenderViewToString("~/Areas/Admin/Views/Calendar/_Edit.cshtml", objLinkWebsite), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tbl_Calendar obj)
        {
            try
            {
                _calendarRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật Lịch công tác: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật Lịch công tác thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật Lịch công tác thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _calendarRepository.Find(id);
                _calendarRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa Lịch công tác: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa Lịch công tác thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa Lịch công tác thành công",
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
                    _calendarRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} Lịch công tác", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
