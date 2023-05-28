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
    public class LinkWebsiteController : BaseController
    {
        //
        // GET: /Admin/LinkWebsite/
        readonly ILinkWebsiteRepository _linkWebsiteRepository = new LinkWebsiteRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            /*var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID));
            TempData["PageElement"] = lstpageElements.ToList();*/
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {
            var objUser = _userAdminRepository.Find(User.ID);
           /* var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID));
            TempData["PageElement"] = lstpageElements.ToList();*/
            var lstLinkWebsite = _linkWebsiteRepository.GetAll().OrderBy(g=>g.Ordering).ToList();
            /*foreach (var item in lstLinkWebsite)
            {
                var firstOrDefault = lstLanguages.FirstOrDefault(g => g.Code == item.LangCode);
                if (firstOrDefault != null)
                    item.LangName = firstOrDefault.Name;
            }*/

            LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var linklienket in lstLinkWebsite)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(linklienket.LangCode) && linklienket.LangCode == language.Code)
                        linklienket.NgonNgu = language.Name;
                }
            }
            lstLinkWebsite = lstLinkWebsite.Where(g => g.LangCode == LangCode).ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                lstLinkWebsite =
                    lstLinkWebsite.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstLinkWebsite = lstLinkWebsite.Where(g => g.LangCode == LangCode).ToList();
            }
           
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/LinkWebsite/_ListData.cshtml", lstLinkWebsite),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //ngôn ngữ
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            return Json(RenderViewToString("~/Areas/Admin/Views/LinkWebsite/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_LinkWebsite obj)
        {
            try
            {
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                _linkWebsiteRepository.Add(obj);
                GenListHome();
                LogController.AddLog(string.Format("Thêm mới liên kết website: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới link thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới link thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objLinkWebsite = _linkWebsiteRepository.Find(id);
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objLinkWebsite.LangCode))
                langCode = objLinkWebsite.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            return Json(RenderViewToString("~/Areas/Admin/Views/LinkWebsite/_Edit.cshtml", objLinkWebsite), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_LinkWebsite obj)
        {
            try
            {
                _linkWebsiteRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật liên kết website: {0}", obj.Name), User.Username);
                GenListHome();
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
                    Messenger = string.Format("Cập nhật link thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _linkWebsiteRepository.Find(id);
                _linkWebsiteRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa liên kết website: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa liên kết thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            GenListHome();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa liên kết thành công",
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
                    _linkWebsiteRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            GenListHome();
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} liên kết", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _linkWebsiteRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _linkWebsiteRepository.Edit(obj);
            GenListHome();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = _linkWebsiteRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _linkWebsiteRepository.Edit(obj);

                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật vị trí thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public void GenListHome()
        {
            var lstHomeNews = new List<tbl_LinkWebsite>();
            lstHomeNews = _linkWebsiteRepository.GetAll().Where(g=>g.Status == true).OrderBy(g => g.Ordering).ToList();
            string strFileName = "linkwebsite.json";
            Common.genCommonFileJson(lstHomeNews, strFileName);
        }
    }
}
