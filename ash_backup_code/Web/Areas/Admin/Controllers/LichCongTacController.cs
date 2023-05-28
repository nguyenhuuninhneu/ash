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
    public class LichCongTacController : BaseController
    {
        //
        // GET: /Admin/LichCongTac/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly ILichCongTacRepository _LichCongTacRepository  = new LichCongTacRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var lstQuyTrinh = _LichCongTacRepository.GetAll().OrderBy(g=>g.CreateDate).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/LichCongTac/_ListData.cshtml", lstQuyTrinh),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/LichCongTac/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_LichCongTac obj)
        {
            try
            {
                var ngaybatdau = Request["ngaybatdau"];
                if (!string.IsNullOrEmpty(ngaybatdau))
                {
                    obj.ngaybatdau = HelperDateTime.ConvertDateTime(ngaybatdau);
                }
                obj.CreateDate = DateTime.Now;
                obj.UserID = User.ID;
                _LichCongTacRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới lịch công tác: {0}", obj.noidung), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới lịch công tác thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới lịch công tác thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var objLichCongTac = _LichCongTacRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/LichCongTac/_Edit.cshtml", objLichCongTac), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_LichCongTac obj)
        {
            try
            {
                var ngaybatdau = Request["ngaybatdau"];
                if (!string.IsNullOrEmpty(ngaybatdau))
                {
                    obj.ngaybatdau = HelperDateTime.ConvertDateTime(ngaybatdau);
                }
                obj.ModifyDate = DateTime.Now;
                _LichCongTacRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật lịch công tác: {0}", obj.noidung), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật lịch công tác thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật lịch công tác thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _LichCongTacRepository.Find(id);
                _LichCongTacRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa lịch công tác: {0}", obj.noidung), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa lịch công tác thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa lịch công tác thành công",
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
                    _LichCongTacRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} lịch công tác", count),
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
                var obj = _LichCongTacRepository.Find(Convert.ToInt32(id));
                try
                {
                    _LichCongTacRepository.Edit(obj);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật lịch công tác thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật lịch công tác thành công",
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
