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
    public class LinhVucTTHCController : BaseController
    {
        //
        // GET: /Admin/LinhVucTTHC/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly ILinhVucTTHCRepository _linhVucTTHCReporitory = new LinhVucTTHCRepository();

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
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstlinhvuctthc = _linhVucTTHCReporitory.GetAll();
            if (!string.IsNullOrEmpty(Name))
            {
                lstlinhvuctthc =
                    lstlinhvuctthc.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstlinhvuctthc = lstlinhvuctthc.Where(g => g.LangCode == LangCode).ToList();
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/LinhVucTTHC/_ListData.cshtml", lstlinhvuctthc),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/LinhVucTTHC/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_LinhVucTTHC obj)
        {
            try
            {
                _linhVucTTHCReporitory.Add(obj);
                LogController.AddLog(string.Format("Thêm mới lĩnh vực thủ tục hành chính: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới lĩnh vực thủ tục hành chính thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới lĩnh vực thủ tục hành chính thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var objLinhVucTTHC = _linhVucTTHCReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/LinhVucTTHC/_Edit.cshtml", objLinhVucTTHC), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_LinhVucTTHC obj)
        {
            try
            {
                _linhVucTTHCReporitory.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật lĩnh vực thủ tục hành chính: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật lĩnh vực thủ tục hành chính thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật lĩnh vực thủ tục hành chính thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _linhVucTTHCReporitory.Find(id);
                _linhVucTTHCReporitory.Delete(id);
                LogController.AddLog(string.Format("Xóa lĩnh vực thủ tục hành chính: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa lĩnh vực thủ tục hành chính thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa lĩnh vực thủ tục hành chính thành công",
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
                    _linhVucTTHCReporitory.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} lĩnh vực thủ tục hành chính", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
