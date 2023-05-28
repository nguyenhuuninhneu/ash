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
    public class AdvController : BaseController
    {
        readonly IPositionRepository _positionRepository = new PositionRepository();
        readonly IAdvRepository _advRepository = new AdvRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();

        //
        // GET: /Admin/SlideImages/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
           
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int page = 1)
        {
            var lstAdv = _advRepository.GetAll().OrderBy(x => x.Ordering).ToList();
            var lstPos = _positionRepository.GetAll().OrderBy(x => x.Ordering).ToList();
            TempData["ListPos"] = lstPos;
            var totalAdv = lstAdv.Count();
            lstAdv = lstAdv.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();

           var LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var adv in lstAdv)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(adv.LangCode) && adv.LangCode == language.Code)
                        adv.NgonNgu = language.Name;
                }
            }
            lstAdv = lstAdv.Where(g => g.LangCode == LangCode).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Adv/_ListData.cshtml", lstAdv),
                totalPages = Math.Ceiling(((double)totalAdv / Webconfig.RowLimit)),
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

            var listPosByCode =  _positionRepository.GetListPosByCode("advert");
            if (listPosByCode == null)
            {
                listPosByCode = new List<tbl_Position>();
            }
            TempData["LstPos"] = listPosByCode;
            return Json(RenderViewToString("~/Areas/Admin/Views/Adv/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_Advert obj)
        {
           
            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = User.ID;
            obj.ModifiedBy = User.ID;
            var find = _advRepository.GetAll().Where(x => x.Name == obj.Name).ToList();
            if (find.Count >= 1)
            {
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Tên quảng cáo đã tồn tại",
                }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                _advRepository.Add(obj);
                GenListHome();
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới quảng cáo thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới quảng cáo thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objAdv = _advRepository.Find(id);
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objAdv.LangCode))
                langCode = objAdv.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;


            var listPosByCode = _positionRepository.GetListPosByCode("advert");
            TempData["LstPos"] = listPosByCode;
            return Json(RenderViewToString("~/Areas/Admin/Views/Adv/_Edit.cshtml", objAdv), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Advert obj)
        {
            var objAdv = _advRepository.Find(obj.ID);
            obj.CreatedDate = objAdv.CreatedDate;
            obj.ModifiedBy = User.ID;
            obj.ModifiedDate = DateTime.Now;
            try
            {
                _advRepository.Edit(obj);
                LogController.AddLog(string.Format("Sửa ảnh: {0}", obj.Name), User.Username);
                GenListHome();
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật quảng cáo thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật quảng cáo thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
       
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _advRepository.Find(id);
                _advRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa ảnh: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            GenListHome();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa ảnh thành công",
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
                    _advRepository.Delete(Convert.ToInt32(item));
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
                Messenger = string.Format("Xóa thành công {0} ảnh", count),
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
                var obj = _advRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _advRepository.Edit(obj);
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
            GenListHome();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _advRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _advRepository.Edit(obj);
            GenListHome();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public void GenListHome()
        {
            var lstAdvRight1 = new List<tbl_Advert>();
            lstAdvRight1 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_right1")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            string strFileName = "adv_right1.json";
            Common.genCommonFileJson(lstAdvRight1, strFileName);
            var lstAdvRight2 = new List<tbl_Advert>();
            lstAdvRight2 = _advRepository.GetAll().Where(g => g.Status && g.Position != null && g.Position.Split(',').Contains("adv_right2")).OrderBy(x => x.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            strFileName = "adv_right2.json";
            Common.genCommonFileJson(lstAdvRight2, strFileName);
        }
    }
}
