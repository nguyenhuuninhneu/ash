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
    public class SlideImagesController : BaseController
    {
        readonly ISlideImagesRepository _slideImagesRepository = new SlideImagesRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IHomeMenuRepository _homemenuRepository = new HomeMenuRepository();
        //
        // GET: /Admin/SlideImages/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string LangCode, int PageElementId = 0, int page = 1)
        {
            var lstSlideImages = _slideImagesRepository.GetAll().OrderBy(g=>g.Ordering).ToList();
            var totalSlideImages = lstSlideImages.Count();
            lstSlideImages = lstSlideImages.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/SlideImages/_ListData.cshtml", lstSlideImages),
                totalPages = Math.Ceiling(((double)totalSlideImages / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstCat = _homemenuRepository.GetAll().Where(g=>g.IsMenu == true && g.ParentID == 0).ToList();
            TempData["Category"] = lstCat;
            return Json(RenderViewToString("~/Areas/Admin/Views/SlideImages/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_SlideImages obj)
        {
            obj.PageElementId = Request["PageElementId"];
            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = User.ID;
            try
            {
                _slideImagesRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới slide ảnh: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới ảnh slide thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới ảnh slide thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objSlideImages = _slideImagesRepository.Find(id);
            var lstCat = _homemenuRepository.GetAll().Where(g => g.IsMenu == true && g.ParentID == 0).ToList();
            TempData["Category"] = lstCat;
            return Json(RenderViewToString("~/Areas/Admin/Views/SlideImages/_Edit.cshtml", objSlideImages), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_SlideImages obj)
        {
            var abc = Request["PageElementId"];
            obj.PageElementId = abc;
            obj.ModifiedBy = User.ID;
            obj.ModifiedDate = DateTime.Now;
            try
            {
                _slideImagesRepository.Edit(obj);
                LogController.AddLog(string.Format("Sửa slide ảnh: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật slide ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật slide ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _slideImagesRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _slideImagesRepository.Edit(obj);
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
                var obj = _slideImagesRepository.Find(id);
                _slideImagesRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa slide ảnh: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa slide ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa slide ảnh thành công",
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
                    _slideImagesRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} slide ảnh", count),
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
                var obj = _slideImagesRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _slideImagesRepository.Edit(obj);
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
    }
}
