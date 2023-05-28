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
    public class ToGiacBEController : BaseController
    {
        readonly IToGiacRepository RepToGiac = new ToGiacRepository();
        readonly ITruyNaRepository RepTruyNa = new TruyNaRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataAdmin(string TuNgay, string DenNgay, int page = 1)
        {
            var lstToGiac = RepToGiac.GetAll();
            var lstTruyNa = RepTruyNa.GetAll().Where(g=>g.Status == 1);
            if (lstToGiac.Any())
            {
                foreach (var item in lstToGiac)
                {
                    var obj = lstTruyNa.FirstOrDefault(g => g.ID == item.ToiPhamID);
                    if (obj != null)
                    {
                        item.NameToiPham = obj.Name;
                    }
                }
            }
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstTruyNa = lstTruyNa.Where(g => g.CreateDate.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstTruyNa = lstTruyNa.Where(g => g.CreateDate.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            var totalQa = lstToGiac.Count();
            lstToGiac = lstToGiac.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/ToGiacBE/_ListData.cshtml", lstToGiac),
                totalPages = Math.Ceiling(((double)totalQa / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = RepToGiac.Find(id);
                LogController.AddLog("Xóa tố giác", User.Username);
                RepToGiac.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa tố giác thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa tố giác thất bại",
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
                    count++;
                    var obj = RepToGiac.Find(Convert.ToInt32(item));
                    RepToGiac.Delete(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Đã xoá {0} tố giác",count), User.Username);
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
    }
}
