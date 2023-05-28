using PublicService.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class NewsPaperController : BaseController
    {
        readonly INewsPaperRepository RepNewsPaper = new NewsPaperRepository();
        private readonly IUserAdminRepository RepUser = new UserAdminRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(string key, int page = 1)
        {
            var lstData = RepNewsPaper.GetAll().ToList();
            var lstUser = RepUser.GetAll().ToList();
            if (!string.IsNullOrEmpty(key))
            {
                lstData = lstData.Where(g => g.Name.Contains(key)).ToList();
            }
            if (lstData.Any())
            {
                foreach (var item in lstData)
                {
                    var obj = lstUser.FirstOrDefault(g => g.ID == item.CreateBy);
                    if (obj != null)
                    {
                        item.UserName = obj.UserName;
                    }
                }
            }
            var total = lstData.Count();
            lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/NewsPaper/ListData.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        public ActionResult Reply(int NewsPaperID)
        {
            ViewBag.NewsPaperID = NewsPaperID;
            return View();
        }
        //[Authorize(Roles = "Index")]
        //public ActionResult ListDataCmt(int id,int page = 1)
        //{
        //    var lstData = RepNewsPaper.GetAllCmt().Where(g=>g.NewsPaperID == id).ToList();
        //    TempData["Title"] = RepNewsPaper.Find(id).Name;
        //    var total = lstData.Count();
        //    lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
        //    return Json(new  var lstData = RepNewsPaper.GetAllCmt().Where(g=>g.NewsPaperID == id).ToList();
        //    TempData["Title"] = RepNewsPaper.Find(id).Name;
        //    {
        //        viewContent = RenderViewToString("~/Areas/Admin/Views/NewsPaper/ListDataCmt.cshtml", lstData),
        //        totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
        //    }, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult ListDataCmt(string TuNgay, string DenNgay, int id, int page = 1)
        {
            TempData["cmtIDz"] = id;
            var lstData = RepNewsPaper.GetAllCmt();
           TempData["Title"] = RepNewsPaper.Find(id).Name;
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            if (id > 0)
            {
                lstData = lstData.Where(g => g.NewsPaperID == id);
            }
            var total = lstData.Count();
            lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                cmtid = id,
                viewContent = RenderViewToString("~/Areas/Admin/Views/NewsPaper/ListDataCmt.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAllRep(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = RepNewsPaper.FindCmt(Convert.ToInt32(item));
                    // ten bai viet
                    var tenbaiviet = RepNewsPaper.Find(obj.NewsPaperID).Name;
                    //
                    RepNewsPaper.DeleteCmt(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", obj.NoiDung, tenbaiviet), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bình luận", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        public ActionResult DetailsCmt(int id)
        {
            var obj = RepNewsPaper.FindCmt(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/NewsPaper/DetailsCmt.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            return Json(RenderViewToString("~/Areas/Admin/Views/NewsPaper/Add.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        public ActionResult Add(tbl_NewsPaper obj)
        {
            try
            {
                obj.CreateDate = DateTime.Now;
                obj.CreateBy = User.ID;
                RepNewsPaper.Add(obj);
                LogController.AddLog(string.Format("Thêm mới bài viết: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới bài viết thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Thêm mới bài viết thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var obj = RepNewsPaper.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/NewsPaper/Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = RepNewsPaper.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            RepNewsPaper.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        public ActionResult Edit(tbl_NewsPaper obj)
        {
            try
            {
                RepNewsPaper.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật bài viết: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật bài viết thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Cập nhật bài viết thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = RepNewsPaper.Find(id);
                LogController.AddLog(string.Format("Xóa bài viết: {0}", obj.Name), User.Username);
                var arrCMT = RepNewsPaper.GetAllCmt().Where(g => g.NewsPaperID == id).Select(g => g.ID).ToArray();
                var strCMT = string.Join(",", arrCMT);
                DeleteAllRep(strCMT);
                RepNewsPaper.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa bài viết thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa bài viết thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }
      
        [Authorize(Roles = "Delete")]
        public ActionResult DeleteCmt(int id)
        {
            try
            {
                var obj = RepNewsPaper.FindCmt(id);
                LogController.AddLog(string.Format("Xóa ý kiến: {0}", obj.FullName), User.Username);
                RepNewsPaper.DeleteCmt(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa ý kiến thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa ý kiến thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
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
                    var obj = RepNewsPaper.Find(Convert.ToInt32(item));
                    RepNewsPaper.Delete(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa bài viết: {0}", obj.Name), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bài viết", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
