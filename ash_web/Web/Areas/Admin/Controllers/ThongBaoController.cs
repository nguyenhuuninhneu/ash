using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;
using Web.Core;

namespace Web.Areas.Admin.Controllers
{
    public class ThongBaoController : BaseController
    {
        readonly IUserAdminRepository _userRepository = new UserAdminRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly ITTHCRepository _tthcsRepository = new TTHCRepository();
        readonly IqProcedureRepository _qProcedureRepository = new qProcedureRepository();
        //
        // GET: /Admin/Home/
        [Authorize]
        public ActionResult Index()
        {
            if (User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            List<ThongBao> lstThongBao = new List<ThongBao>();
            var lstDataNews = _newsRepository.GetAllCMT();
            lstDataNews = lstDataNews.Where(x => x.Status == false && x.CreatedBy == 0).Take(5);
            //var lstDataTTHC = _tthcsRepository.GetAllCMT();
            //lstDataTTHC = lstDataTTHC.Where(x => x.Status == false && x.CreatedBy == 0).Take(5);
            // Type = 0 => Binh luan tin tuc, Type = 1 => Tra loi binh luan tin tuc, Type = 2 => Binh Luan TTHC, Type = 3 => Tra loi Binh luan TTHC
            foreach (var item in lstDataNews)
            {
                var thongBaoTinTuc = new ThongBao();
                thongBaoTinTuc.ID = item.ID;
                thongBaoTinTuc.Title = item.CommentID == null || item.CommentID == 0 ? "Bình luận tin tức" : "Trả lời tin tức";
                thongBaoTinTuc.Type = item.CommentID == null || item.CommentID == 0 ? 0 : 1;
                thongBaoTinTuc.CommentId = item.CommentID == null || item.CommentID == 0 ? 0 : item.CommentID;
                thongBaoTinTuc.CreateDate = item.CreateDate;
                thongBaoTinTuc.Content = item.NoiDung;
                thongBaoTinTuc.TypeId = item.NewsID;
                thongBaoTinTuc.IsView = item.IsView;
                lstThongBao.Add(thongBaoTinTuc);
            }

            //foreach (var item in lstDataTTHC)
            //{
            //    var thongBaoTTHC = new ThongBao();
            //    thongBaoTTHC.ID = item.ID;
            //    thongBaoTTHC.Title = item.CommentID == null ? "Bình luận TTHC" : "Trả lời TTHC";
            //    thongBaoTTHC.Type = item.CommentID == null ? 2 : 3; ;
            //    thongBaoTTHC.CommentId = item.CommentID == null ? 0 : item.CommentID;
            //    thongBaoTTHC.CreateDate = item.CreateDate;
            //    thongBaoTTHC.Content = item.NoiDung;
            //    thongBaoTTHC.TypeId = item.TTHCID;
            //    thongBaoTTHC.IsView = item.IsView;
            //    lstThongBao.Add(thongBaoTTHC);
            //}
            lstThongBao = lstThongBao.OrderByDescending(x => x.CreateDate).ToList();
            ViewBag.TongSoThongBao = lstThongBao.Where(x => x.IsView == false).Count();
            lstThongBao = lstThongBao.Take(5).ToList();

            var objUser = _userRepository.Find(User.ID);
            TempData["objUser"] = objUser;  
            return View("~/Areas/Admin/Views/ThongBao/Index.cshtml", lstThongBao);
        }


        public ActionResult Daxem(int id, int type)
        {
            if (type == 0 || type == 1)
            {
                var itemNews = _newsRepository.GetAllCMT().Where(x => x.ID == id).FirstOrDefault();
                itemNews.IsView = true;
                _newsRepository.EditCMT(itemNews);
            }
            if (type == 2 || type == 3) 
            {
                var itemTTHC = _tthcsRepository.GetAllCMT().Where(x => x.ID == id).FirstOrDefault();
                itemTTHC.IsView = true;
                _tthcsRepository.EditCMT(itemTTHC);
            }
            return Json(new
            {
                IsSuccess = true

            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Comment()
        {
            return View("~/Areas/Admin/Views/ThongBao/Comment.cshtml");
        }
        public ActionResult ListData(string TuNgay, string DenNgay)
        {
            var objUser = _userRepository.Find(User.ID);
            TempData["objUser"] = objUser; 
            List<ThongBao> lstThongBao = new List<ThongBao>();
            var lstDataNews = _newsRepository.GetAllCMT();
            lstDataNews = lstDataNews.Where(x => x.Status == false && x.CreatedBy == 0).ToList();
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstDataNews = lstDataNews.Where(g => g.CreateDate != null && g.CreateDate.Value.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstDataNews = lstDataNews.Where(g => g.CreateDate != null && g.CreateDate.Value.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            var lstDataTTHC = _tthcsRepository.GetAllCMT();
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstDataTTHC = lstDataTTHC.Where(g => g.CreateDate != null && g.CreateDate.Value.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstDataTTHC = lstDataTTHC.Where(g => g.CreateDate != null && g.CreateDate.Value.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            lstDataTTHC = lstDataTTHC.Where(x => x.Status == false && x.CreatedBy == 0).ToList();
            foreach (var item in lstDataNews)
            {
                var thongBaoTinTuc = new ThongBao();
                thongBaoTinTuc.ID = item.ID;
                thongBaoTinTuc.Title = item.CommentID == null ? "Bình luận tin tức" : "Trả lời tin tức";
                thongBaoTinTuc.Type = item.CommentID == null ? 0 : 1;
                thongBaoTinTuc.CommentId = item.CommentID == null ? 0 : item.CommentID;
                thongBaoTinTuc.CreateDate = item.CreateDate;
                thongBaoTinTuc.FullName = item.FullName;
                thongBaoTinTuc.Email = item.Email;
                thongBaoTinTuc.Content = item.NoiDung;
                thongBaoTinTuc.TypeId = item.NewsID;
                thongBaoTinTuc.IsView = item.IsView;
                thongBaoTinTuc.Status = item.Status;
                lstThongBao.Add(thongBaoTinTuc);
            }

            foreach (var item in lstDataTTHC)
            {
                var thongBaoTTHC = new ThongBao();
                thongBaoTTHC.ID = item.ID;
                thongBaoTTHC.Title = item.CommentID == null ? "Bình luận TTHC" : "Trả lời TTHC";
                thongBaoTTHC.Type = item.CommentID == null ? 2 : 3; ;
                thongBaoTTHC.CommentId = item.CommentID == null ? 0 : item.CommentID;
                thongBaoTTHC.CreateDate = item.CreateDate;
                thongBaoTTHC.FullName = item.FullName;
                thongBaoTTHC.Email = item.Email;
                thongBaoTTHC.Content = item.NoiDung;
                thongBaoTTHC.TypeId = item.TTHCID;
                thongBaoTTHC.IsView = item.IsView;
                thongBaoTTHC.Status = item.Status;
                lstThongBao.Add(thongBaoTTHC);
            }
            lstThongBao = lstThongBao.OrderByDescending(x => x.CreateDate).ToList();
            lstThongBao = lstThongBao.ToList();
            var total = lstThongBao.Count();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/ThongBao/ListData.cshtml", lstThongBao),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, int type)
        {
            if (type == 0 || type == 1)
            {
                var obj = _newsRepository.FindCMT(id);
                obj.Status = !obj.Status;
                _newsRepository.EditCMT(obj);
            }
            else
            {
                var obj = _tthcsRepository.FindCMT(id);
                obj.Status = !obj.Status;
                _tthcsRepository.EditCMT(obj);
            }

            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
