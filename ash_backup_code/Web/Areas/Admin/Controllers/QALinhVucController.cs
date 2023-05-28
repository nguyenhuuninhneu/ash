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
    public class QALinhVucController : BaseController
    {
        //
        // GET: /Admin/QALinhVuc/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IQALinhVucRepository _QALinhVucReporitory = new QALinhVucRepository();

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
            var lstCqbh = _QALinhVucReporitory.GetAll().OrderBy(g=>g.Ordering).ToList();
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
                viewContent = RenderViewToString("~/Areas/Admin/Views/QALinhVuc/_ListData.cshtml", lstCqbh),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/QALinhVuc/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_QALinhVuc obj)
        {
            try
            {
                _QALinhVucReporitory.Add(obj);
                LogController.AddLog(string.Format("Thêm mới Lĩnh vực câu hỏi: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới Lĩnh vực câu hỏi thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới Lĩnh vực câu hỏi thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _QALinhVucReporitory.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _QALinhVucReporitory.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objQALinhVuc = _QALinhVucReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/QALinhVuc/_Edit.cshtml", objQALinhVuc), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_QALinhVuc obj)
        {
            try
            {
                _QALinhVucReporitory.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật Lĩnh vực câu hỏi: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật Lĩnh vực câu hỏi thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật Lĩnh vực câu hỏi thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _QALinhVucReporitory.Find(id);
                _QALinhVucReporitory.Delete(id);
                LogController.AddLog(string.Format("Xóa Lĩnh vực câu hỏi: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa Lĩnh vực câu hỏi thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa Lĩnh vực câu hỏi thành công",
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
                    _QALinhVucReporitory.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} Lĩnh vực câu hỏi", count),
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
                var obj = _QALinhVucReporitory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _QALinhVucReporitory.Edit(obj);

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
