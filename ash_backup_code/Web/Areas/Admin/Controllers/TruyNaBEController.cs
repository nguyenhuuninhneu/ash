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
    public class TruyNaBEController : BaseController
    {
        readonly ITruyNaRepository _truynaRepository = new TruyNaRepository();
        readonly ICategoryRepository _categoryRepository = new CategoryRepository();
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
            var lstData = _truynaRepository.GetAll().OrderBy(g=>g.IsDinhNa).ThenByDescending(g=>g.ID).ToList();
            var total = lstData.Count();
            lstData = lstData.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/TruyNaBE/_ListData.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            return Json(RenderViewToString("~/Areas/Admin/Views/TruyNaBE/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_truyna obj)
        {
            obj.CreateDate = DateTime.Now;
            try
            {
                _truynaRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới truy nã: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới truy nã thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới truy nã thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var obj = _truynaRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/TruyNaBE/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_truyna obj)
        {
            try
            {
                var abc = obj.Status;
                var oa = obj.IsHome;
                _truynaRepository.Edit(obj);
                LogController.AddLog(string.Format("Sửa truy nã : {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật truy nã thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật truy nã thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _truynaRepository.Find(id);
                var arrRep = _truynaRepository.GetAllCMT().Where(g => g.ToiPhamID == id).Select(g => g.ID).ToArray();
                var strRep = string.Join(",", arrRep);
                DeleteAllToGiac(strRep);
                _truynaRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa truy nã: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa truy nã thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa truy nã thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAllToGiac(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _truynaRepository.FindCMT(Convert.ToInt32(item));
                    var tenbaiviet = _truynaRepository.Find(Convert.ToInt32(obj.ToiPhamID)).Name;
                    _truynaRepository.DeleteCMT(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa tố giác: {0} trong bài viết {1}", obj.NoiDung, tenbaiviet), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} tố giác", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _truynaRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _truynaRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
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
                    _truynaRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} truy nã", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
