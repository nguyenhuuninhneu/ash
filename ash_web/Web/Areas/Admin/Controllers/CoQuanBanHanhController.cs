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
    public class CoQuanBanHanhController : BaseController
    {
        //
        // GET: /Admin/CoQuanBanHanh/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly ICoQuanBanHanhReporitory _coQuanBanHanhReporitory = new CoQuanBanHanhReporitory();

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
            var lstCqbh = _coQuanBanHanhReporitory.GetAll().ToList();
            foreach (var item in lstCqbh)
            {
                var objParent = lstCqbh.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            lstCqbh = Common.CreateLevel(lstCqbh);

            if (!string.IsNullOrEmpty(Name))
            {
                lstCqbh =
                    lstCqbh.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstCqbh = lstCqbh.Where(g => g.LangCode == LangCode).ToList();
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/CoQuanBanHanh/_ListData.cshtml", lstCqbh),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var lstCqbh = _coQuanBanHanhReporitory.GetAll().ToList();
            lstCqbh = Common.CreateLevel(lstCqbh);
            TempData["lstParent"] = lstCqbh;
            return Json(RenderViewToString("~/Areas/Admin/Views/CoQuanBanHanh/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_CoQuanBanHanh obj)
        {
            try
            {
                _coQuanBanHanhReporitory.Add(obj);
                LogController.AddLog(string.Format("Thêm mới danh mục văn bản: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục văn bản thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var lstCqbh = _coQuanBanHanhReporitory.GetAll().ToList();
            lstCqbh = Common.CreateLevel(lstCqbh);
            TempData["lstParent"] = lstCqbh;
            var objCoQuanBanHanh = _coQuanBanHanhReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/CoQuanBanHanh/_Edit.cshtml", objCoQuanBanHanh), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_CoQuanBanHanh obj)
        {
            try
            {
                _coQuanBanHanhReporitory.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật danh mục văn bản: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục văn bản thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _coQuanBanHanhReporitory.Find(id);
                _coQuanBanHanhReporitory.Delete(id);
                LogController.AddLog(string.Format("Xóa danh mục văn bản: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục văn bản thành công",
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
                    _coQuanBanHanhReporitory.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục văn bản", count),
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
                var obj = _coQuanBanHanhReporitory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _coQuanBanHanhReporitory.Edit(obj);

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
