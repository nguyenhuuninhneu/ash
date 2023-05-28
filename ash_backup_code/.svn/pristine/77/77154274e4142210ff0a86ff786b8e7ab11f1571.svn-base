using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class LanguagesController : BaseController
    {
        //
        // GET: /Admin/Languages/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData()
        {
            var lstLanguages = _languagesRepository.GetAll().ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Languages/_ListData.cshtml", lstLanguages),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult menuLanguages()
        {
            var lstLanguages = _languagesRepository.GetAll().ToList();
            TempData["lstLanguages"] = lstLanguages;
            return View();
        }
        public ActionResult ChangeLanguages(string languages)
        {
            Session["langCodeDefaut"] = "vn";
            if (!string.IsNullOrEmpty(languages))
            {
                Session["langCodeDefaut"] = languages;
            }
            return Json(new {
                IsSuccess = true,
                Messages = string.Format("Bạn đã chọn ngôn ngữ tiếng anh để nhập dữ liệu")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
