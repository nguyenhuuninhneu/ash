using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class FooterController : BaseController
    {
        //
        // GET: /Admin/Footer/
        readonly IFooterRepository _FooterRepository = new FooterRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        { 
            var objFooter = _FooterRepository.GetAll().FirstOrDefault();
            //ngôn ngữ
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;


            return View(objFooter);
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {
            var lstFooter = _FooterRepository.GetAll();

            var lstLang = _languagesRepository.GetAll().ToList();
            foreach (var item in lstFooter)
            {
                var firstOrDefault = lstLang.FirstOrDefault(g => g.Code == item.LangCode);
                if (firstOrDefault != null)
                    item.LangName = firstOrDefault.Name;
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Footer/_ListData.cshtml", lstFooter),
            }, JsonRequestBehavior.AllowGet);
        }
       
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {

            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/Footer/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(tbl_Footer obj)
        {
            try
            {
                _FooterRepository.Add(obj);
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
            var objFooter = _FooterRepository.Find(id);

            return Json(RenderViewToString("~/Areas/Admin/Views/Footer/_Edit.cshtml", objFooter), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(tbl_Footer obj)
        {
            //Thêm ngôn ngữ mặc định
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            obj.LangCode = langCode.ToString();

           _FooterRepository.Edit(obj);
            GenFooterJson();
            return Redirect("/Admin/Footer?sc=" + 2);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _FooterRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _FooterRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _FooterRepository.Find(id);
                _FooterRepository.Delete(id);
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
                    _FooterRepository.Delete(Convert.ToInt32(item));
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
        
        public void GenFooterJson()
        {
            var file = Common.GetPath("~/Content/Json/footer.json");
            var objFooter = _FooterRepository.GetAll().FirstOrDefault();
            JavaScriptSerializer jsonseria = new JavaScriptSerializer();
            var data = jsonseria.Serialize(objFooter);
            System.IO.File.WriteAllText(file, data, Encoding.UTF8);
        }
    }
}
