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
    public class ImagesController : BaseController
    {
        readonly IImagesRepository _newsImagesRepository = new ImagesRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IImageCategoryRepository _newsimgCategoryReporitory = new ImageCategoryRepository();
        //
        // GET: /Admin/SlideImages/
        [Authorize(Roles = "Index")]
        public ActionResult Index(int catid)
        {
            //var objUser = _userAdminRepository.Find(User.ID);
            TempData["catid"] = catid;
            return View();
        }
         
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int catid = 0, string LangCode = "vn", int PageElementId = 0, int page = 1)
        {
            var lstCat = _newsimgCategoryReporitory.GetAll().OrderBy(x => x.Ordering).ToList();
            TempData["Category"] = lstCat;
            var lstImages = _newsImagesRepository.GetAll().OrderBy(x=>x.Ordering).ToList();
            if (catid > 0)
            {
                lstImages = lstImages.Where(g => g.ImageCategoryId == catid).ToList();
            }
             
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var images in lstImages)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(images.LangCode) && images.LangCode == language.Code)
                        images.NgonNgu = language.Name;
                }
            }
            lstImages = lstImages.Where(g => g.LangCode == LangCode).ToList();
            var totalSlideImages = lstImages.Count();
            lstImages = lstImages.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Images/_ListData.cshtml", lstImages),
                totalPages = Math.Ceiling(((double)totalSlideImages / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add(int catid)
        {
            //ngôn ngữ
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name; 
            TempData["catid"] = catid; 
            return Json(RenderViewToString("~/Areas/Admin/Views/Images/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_Images obj)
        {
            //var imageCategoryId = Request["ImageCategoryId"];
            //obj.ImageCategoryId = imageCategoryId;
            
            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = User.ID;
            obj.ModifiedBy = User.ID;
           
            try
            {
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                var lstImg = obj.Images;
                for (int i = 0; i < lstImg.Split('|').Length; i++)
                {
                    obj.Images = lstImg.Split('|')[i];
                    _newsImagesRepository.Add(obj);
                }

                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objImages = _newsImagesRepository.Find(id);
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objImages.LangCode))
                langCode = objImages.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;
            return Json(RenderViewToString("~/Areas/Admin/Views/Images/_Edit.cshtml", objImages), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Images obj)
        {
            //var imageCategoryId = Request["ImageCategoryId"];
            //obj.ImageCategoryId = imageCategoryId;
            obj.ModifiedBy = User.ID;
            obj.ModifiedDate = DateTime.Now;
            
            try
            {
                _newsImagesRepository.Edit(obj);
                LogController.AddLog(string.Format("Sửa ảnh: {0}", obj.Title), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _newsImagesRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _newsImagesRepository.Edit(obj);
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
                var obj = _newsImagesRepository.Find(id);
                _newsImagesRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa ảnh: {0}", obj.Title), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
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
                    _newsImagesRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
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
                var title = item.Split(':')[2];
                var obj = _newsImagesRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                obj.Title = title;
                try
                {
                    _newsImagesRepository.Edit(obj);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật danh sách thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật danh sách thành công",
            }, JsonRequestBehavior.AllowGet);
        }
       
    }
}
