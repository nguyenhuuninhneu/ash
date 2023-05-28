using PagedList;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Web;
using Web.Areas.Admin.Controllers;
using Web.Core;
using Web.Repository;
using Web.Repository.Entity;

namespace CMS.Controllers
{
    public class VanBanFEController : Controller
    {
        //readonly ICoQuanBanHanhReporitory _coQuanBanHanhReporitory = new CoQuanBanHanhReporitory();
        //readonly ILinhVucVanBanRepository _linhVucVanBanRepository = new LinhVucVanBanRepository();
        //readonly ILoaiVanBanRepository _loaiVanBanRepository = new LoaiVanBanRepository();
        //readonly IVanBanRepository _vanBanRepository = new VanBanRepository();

        readonly IHomeMenuRepository _categoryRepository = new HomeMenuRepository();
        readonly IVanBanRepository _vanbanRepository = new VanBanRepository();
        readonly ICoQuanBanHanhReporitory _coquanbanhanhRepository = new CoQuanBanHanhReporitory();
        readonly ILoaiVanBanRepository _loaivanbanRepository = new LoaiVanBanRepository();
        readonly ILinhVucVanBanRepository _linhvucvanbanRepository = new LinhVucVanBanRepository();


        public ActionResult Details(int id)
        {
            var objVanban = _vanbanRepository.Find(id);
            var lstCoquanbanhanh = _coquanbanhanhRepository.GetAll().ToList();
            TempData["Coquanbanhanh"] = lstCoquanbanhanh;
            var lstLoaivanban = _loaivanbanRepository.GetAll().ToList();
            TempData["Loaivanban"] = lstLoaivanban;
            var lstLinhvucvanban = _linhvucvanbanRepository.GetAll().ToList();
            TempData["Linhvucvanban"] = lstLinhvucvanban;
            var lstVanban = _vanbanRepository.GetAll().Where(g => g.ID != id && g.Status).OrderByDescending(g => g.NgayBanHanh).ToList();
            TempData["Vanban"] = lstVanban;

            // pulic key
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            if (objVanban != null)
            {
                ViewBag.Title = objVanban.TrichYeu;
            }
            if (Request["PrintView"] == "1") return View("PrintView", objVanban);
            return View(objVanban);
        }

        public ActionResult GetAllNewsByCat(string TrichYeu, string SoHieu, string Ngaybanhanh, int LinhVucVanBanId = 0, int CoQuanBanHanhID = 0, int LoaiVanBanId = 0, int Isnoibo = 0, int page = 1)
        {
            var lstLinhVucVB = _linhvucvanbanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["LinhVucVB"] = lstLinhVucVB;

            var lstCoquanbh = _coquanbanhanhRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["CoQuanBanHanh"] = Common.CreateLevel(lstCoquanbh);

            var lstLoaiVB = _loaivanbanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["lstLoaiVB"] = lstLoaiVB;

            var lstVanBan = _vanbanRepository.GetAll().Where(g => g.Status).OrderByDescending(g => g.NgayBanHanh).ToList();
            if (!string.IsNullOrEmpty(TrichYeu))
            {
                lstVanBan = lstVanBan.Where(g => HelperString.UnsignCharacter(g.TrichYeu.Trim().ToLower()).Contains(HelperString.UnsignCharacter(TrichYeu.ToLower().Trim()))).ToList();
            }
            var ngaybanhanh = HelperDateTime.ConvertDate(Ngaybanhanh);
            if (!string.IsNullOrEmpty(Ngaybanhanh))
            {
                lstVanBan = lstVanBan.Where(g => g.NgayBanHanh == ngaybanhanh).ToList();
            }
            if (!string.IsNullOrEmpty(SoHieu))
            {
                lstVanBan = lstVanBan.Where(g => HelperString.UnsignCharacter(g.SoHieu.Trim().ToLower()).Contains(HelperString.UnsignCharacter(SoHieu.ToLower().Trim()))).ToList();
            }
            if (LinhVucVanBanId != 0)
            {
                lstVanBan = lstVanBan.Where(g => g.LinhVucVanBanId == LinhVucVanBanId).ToList();
            }
            if (CoQuanBanHanhID != 0)
            {
                var fullcatid = Common.GetChild(lstCoquanbh,CoQuanBanHanhID);
                lstVanBan = lstVanBan.Where(g => fullcatid.Contains(g.CoQuanBanHanhId)).ToList();
            }
            if (LoaiVanBanId != 0)
            {
                lstVanBan = lstVanBan.Where(g => g.LoaiVanBanId == LoaiVanBanId).ToList();
            }
            var vitri = Convert.ToInt32(ConfigTextController.GetValueCFT("vitri_vbnoibo"));
            ViewBag.Vitri = vitri;
            /*if (Isnoibo == 1 || Request["Isnoibo"] == null)
                lstVanBan = lstVanBan.Where(s => s.isNoiBo == 1).ToList();
            else*/
                lstVanBan = lstVanBan.Where(s => s.isNoiBo != 1).ToList();
            var limit = Convert.ToInt32(ConfigTextController.GetValueCFT("limit_vanban"));
            ViewBag.limit = limit;
            ViewBag.page = page;
            return View(lstVanBan.ToPagedList(page, limit));
        }
        public ActionResult lstVanBan()
        {
            var lstLinhVucVB = _linhvucvanbanRepository.GetAll().Where(g => g.Status == true).ToList();
            TempData["LinhVucVB"] = lstLinhVucVB;
            var lstCoquanbh = _coquanbanhanhRepository.GetAll().Where(g => g.Status).ToList();
            TempData["CoQuanBanHanh"] = lstCoquanbh;
            var lstVanBan = _vanbanRepository.GetAll().Where(g => g.Status && g.IsRight == true).OrderByDescending(g => g.CreatedDate).ToList();
            TempData["lstVanBan"] = lstVanBan;
            return View(lstVanBan);
        }
        public ActionResult searchVanBan()
        {
            var lstLinhVucVB = _linhvucvanbanRepository.GetAll().Where(g => g.Status == true).OrderBy(g => g.Ordering).ToList();
            TempData["LinhVucVB"] = lstLinhVucVB;
            var lstCoquanbh = _coquanbanhanhRepository.GetAll().Where(g => g.Status).ToList();
            TempData["CoQuanBanHanh"] = Common.CreateLevel(lstCoquanbh);
            var lstLoaiVB = _loaivanbanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["lstLoaiVB"] = lstLoaiVB;
            return View();
        }
        public ActionResult Home()
        {
            var limit_vbhome = Convert.ToInt32(ConfigTextController.GetValueCFT("limit_vbhome"));
            var lstVanBan = _vanbanRepository.GetAll().Where(g => g.Status && g.IsHome == true && g.isNoiBo == 0).OrderByDescending(g => g.NgayBanHanh)/*.Take(limit_vbhome)*/.ToList();
            TempData["lstVanBan"] = lstVanBan;
            var lstVanBannoibo = _vanbanRepository.GetAll().Where(g => g.Status && g.IsHome == true && g.isNoiBo != 1).OrderByDescending(g => g.NgayBanHanh)/*.Take(limit_vbhome)*/.ToList();
            TempData["lstVanBannoibo"] = lstVanBannoibo;
            ViewBag.Vitri = Convert.ToInt32(ConfigTextController.GetValueCFT("vitri_vbnoibo"));
            return View(lstVanBan);
        }
    }
}

