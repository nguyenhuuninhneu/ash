using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Model;
using Web.Core;
using Web.Repository;
using Web.Repository.Entity;
using PagedList;

namespace Web.Controllers
{
    public class NoiBoController : Controller
    {
        readonly IVanBanRepository _vanbanRepository = new VanBanRepository();
        readonly ICoQuanBanHanhReporitory _coquanbanhanhRepository = new CoQuanBanHanhReporitory();
        readonly ILinhVucVanBanRepository _linhvucvanbanRepository = new LinhVucVanBanRepository();
        readonly ILoaiVanBanRepository _loaivanbanRepository = new LoaiVanBanRepository();
        private readonly IDMChungRepository _repositorydonvi = new DMChungRepository();
        private IUserRepository _userservice = new UserRepository();
        private IChucVuRepository chucvuservcie = new ChucVuRepository();
        private ITaiLieuRepository tailieurepository = new TaiLieuRepository();
        private IDanhMucTaiLieuRepository danhmuctailieuservice = new DanhMucTaiLieuRepository();
        public ActionResult VanBan(string TrichYeu, string SoHieu, string Ngaybanhanh, int LinhVucVanBanId = 0, int CoQuanBanHanhID = 0, int LoaiVanBanId = 0, int page = 1)
        {
            var lstLinhVucVB = _linhvucvanbanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["LinhVucVB"] = lstLinhVucVB;

            var lstCoquanbh = _coquanbanhanhRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["CoQuanBanHanh"] = Common.CreateLevel(lstCoquanbh);

            var lstLoaiVB = _loaivanbanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["lstLoaiVB"] = lstLoaiVB;

            var lstVanBan = _vanbanRepository.GetAll().Where(g => g.Status && g.isNoiBo==1).OrderByDescending(g => g.CreatedDate).ToList();
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
                lstVanBan = lstVanBan.Where(g => g.CoQuanBanHanhId == CoQuanBanHanhID).ToList();
            }
            if (LoaiVanBanId != 0)
            {
                lstVanBan = lstVanBan.Where(g => g.LoaiVanBanId == LoaiVanBanId).ToList();
            }
            var limit = 15;
            ViewBag.limit = limit;
            ViewBag.page = page;
            return View(lstVanBan.ToPagedList(page, limit));
        }
        public ActionResult Danhba(string hoten, int donvi = 0, int chucvu = 0, int page = 1)
        {
            var lstuser = _userservice.GetAll().Where(s => s.Active == true).ToList();
            var lstdonvi = _repositorydonvi.GetAll().Where(s => s.Code == "tructhuoc" && s.Status == true).ToList();
            TempData["DonVi"] =Common.CreateLevel(lstdonvi);
            var lbc = _repositorydonvi.GetAll().Where(s=>s.Status == true).ToList();
            TempData["ABC"] = lbc;
            var lstchucvu = chucvuservcie.GetAll().Where(s => s.isPublish == true).ToList();
            TempData["Chucvu"] = lstchucvu;
            if (!string.IsNullOrEmpty(hoten))
            {
                lstuser = lstuser.Where(g => HelperString.UnsignCharacter(g.FullName.Trim().ToLower()).Contains(HelperString.UnsignCharacter(hoten.ToLower().Trim()))).ToList();
            }
            if (donvi != 0)
            {
                var lsChildId = lstdonvi.Where(s => s.ParentID == donvi).Select(s => s.ID).ToList();
                if (lsChildId != null && lsChildId.Count() > 0)
                {
                    var lsChildId1 = lstdonvi.Where(s => lsChildId.Contains(s.ParentID)).Select(s => s.ID).ToList();
                    if (lsChildId1 != null && lsChildId1.Count() > 0)
                    {
                        lstuser = lstuser.Where(s => lsChildId1.Contains(s.DonviId)).ToList();
                    }
                    else
                    {
                        lstuser = lstuser.Where(s => lsChildId.Contains(s.DonviId)).ToList();
                    }
                }
                else
                {
                    lstuser = lstuser.Where(s => s.DonviId == donvi).ToList();
                }
            }
            if (chucvu != 0)
            {
                lstuser = lstuser.Where(g => g.ChucVuId == chucvu).ToList();
            }
            return View(lstuser.ToPagedList(page, Webconfig.RowLimit));
        }
        public ActionResult TaiLieu()
        {
            var lstmodel = danhmuctailieuservice.GetAll().Where(s => s.Status == true && s.ParentID == 0).OrderBy(g=>g.Ordering).ToList();
            var lsttailieu = tailieurepository.GetAll().Where(s => s.Status == true).ToList();
            TempData["Tailieu"] = lsttailieu;
            return View(lstmodel);
        }
        public ActionResult FileTailieu(int id)
        {
            var lstmodel = danhmuctailieuservice.GetAll().Where(s => s.Status == true && s.ParentID == id).OrderBy(g => g.Ordering).ToList();
            TempData["condanhmuc"] = lstmodel;
            var lsttailieu = tailieurepository.GetAll().Where(s => s.Status == true && s.CatID == id).ToList();
            return View(lsttailieu);
        }
    }
}
