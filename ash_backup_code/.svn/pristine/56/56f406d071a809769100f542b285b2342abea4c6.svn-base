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
    public class TaiLieuController : BaseController
    {
        private readonly ITaiLieuRepository _repository = new TaiLieuRepository();
        private readonly IDanhMucTaiLieuRepository _repositoryCat = new DanhMucTaiLieuRepository();
        // GET: /TaiLieu/
        [Authorize(Roles = "Index")]
        public ActionResult Index(/*string Code = "DMChung"*/)
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData()
        {
            var lstDMChung = _repository.GetAll().OrderBy(item => item.Ordering).ToList();
            var lstCat = _repositoryCat.GetAll();
            if (lstDMChung.Count() > 0)
            {
                foreach (var item in lstDMChung)
                {
                    if (item.CatID != 0)
                    {
                        var obj = lstCat.FirstOrDefault(g => g.ID == item.CatID);
                        if (obj != null)
                        {
                            item.CatName = obj.Name;
                        }
                    }
                }
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/TaiLieu/_ListData.cshtml", lstDMChung),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstDB = _repository.GetAll().ToList();
            TempData["Category"] = _repositoryCat.GetAll().ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/TaiLieu/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_TaiLieu obj)
        {
            try
            {
                if (obj.CatID != 0)
                {
                    var _obj = _repositoryCat.Find(obj.CatID);
                    if (_obj != null)
                    {
                        obj.CatName = _obj.Name;   
                    }
                }
                obj.CreateDate = DateTime.Now;
                int numfile = Convert.ToInt32(Request["NumFile"]);
                for (int i = 0; i < numfile; i++)
                {
                    string namefile = Request["namefile" + i.ToString()];
                    string linkfile = Request["file" + i.ToString()];
                    var replaceName = Request["replace_name_" + i.ToString()];
                    if (!string.IsNullOrEmpty(linkfile))
                    {
                        obj.NameFile = namefile;
                        obj.LinkFile = linkfile;
                        obj.ReplaceName = replaceName;
                    }
                }
                _repository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới tài liệu: {0}", obj.CatName), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới tài liệu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới tài liệu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var obj = _repository.Find(id);
            TempData["Category"] = _repositoryCat.GetAll().ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/TaiLieu/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_TaiLieu obj)
        {
            try
            {
                if (obj.CatID != 0)
                {
                    var _obj = _repositoryCat.Find(obj.CatID);
                    if (_obj != null)
                    {
                        obj.CatName = _obj.Name;
                    }

                }
                int numfile = Convert.ToInt32(Request["NumFile"]);
                for (int i = 0; i < numfile; i++)
                {
                    string namefile = Request["namefile" + i.ToString()];
                    string linkfile = Request["file" + i.ToString()];
                    var replaceName = Request["replace_name_" + i.ToString()];
                    if (!string.IsNullOrEmpty(linkfile))
                    {
                        obj.NameFile = namefile;
                        obj.LinkFile = linkfile;
                        obj.ReplaceName = replaceName;
                    }
                }
                _repository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật tài liệu: {0}", obj.CatName), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật tài liệu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật tài liệu thất bại")
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
                LogController.AddLog(string.Format("Xóa tài liệu: {0}", obj.ID), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa tài liệu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa tài liệu thành công",
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
                Messenger = string.Format("Xóa thành công {0} tài liệu", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
