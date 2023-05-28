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
    public class DanhMucTaiLieuController : BaseController
    {
        private readonly IDanhMucTaiLieuRepository _repository = new DanhMucTaiLieuRepository();
        // GET: /DanhMucTaiLieu/
        [Authorize(Roles = "Index")]
        public ActionResult Index(/*string Code = "DMChung"*/)
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData()
        {
            var lstDMChung = _repository.GetAll().ToList();
            lstDMChung = Common.CreateLevel(lstDMChung);
            if (lstDMChung.Count() > 0)
            {
                foreach (var item in lstDMChung)
                {
                    if (item.ParentID != 0)
                    {
                        var obj = lstDMChung.FirstOrDefault(g=>g.ID == item.ParentID);
                        if (obj != null)
                        {
                            item.ParentName = obj.Name;
                        }
                    }
                }
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DanhMucTaiLieu/_ListData.cshtml", lstDMChung),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstDB = _repository.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstParent"] = lstDB;
            return Json(RenderViewToString("~/Areas/Admin/Views/DanhMucTaiLieu/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_DanhMucTaiLieu obj)
        {
            try
            {
                _repository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới danh mục tài liệu: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục tài liệu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục tài liệu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var obj = _repository.Find(id);
            var lstDB = _repository.GetAll().Where(g=>g.ParentID == 0 && g.ID !=  obj.ID);
            lstDB = Common.CreateLevel(lstDB.ToList());
            TempData["lstParent"] = lstDB;
            return Json(RenderViewToString("~/Areas/Admin/Views/DanhMucTaiLieu/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_DanhMucTaiLieu obj)
        {
            try
            {
                _repository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật danh mục tài liệu: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục tài liệu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục tài liệu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
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
                var obj = _repository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _repository.Edit(obj);

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

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _repository.Find(id);
                _repository.Delete(id);
                LogController.AddLog(string.Format("Xóa danh mục tài liệu: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục tài liệu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục tài liệu thành công",
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
                    _repository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục tài liệu", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
