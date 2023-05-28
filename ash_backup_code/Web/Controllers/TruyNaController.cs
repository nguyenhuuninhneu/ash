using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Information;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class TruyNaController : BaseController
    {
        ITruyNaRepository _repTruyNa = new TruyNaRepository();
        public ActionResult Index(string title, string[] nc, int IsDinhNa = 0)
        {
            var lstTruyNa = _repTruyNa.GetAll().OrderBy(g=>g.IsDinhNa).ThenByDescending(g=>g.ID).ToList();
            var searchTruyNa = new List<tbl_truyna>();
            var searchTruyNa_item = new List<tbl_truyna>();
            if (!string.IsNullOrEmpty(title) && nc!= null)
            {
                if (nc.Contains("hoten"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.Name != null &&
                         HelperString.UnsignCharacter(g.Name)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("tenkhac"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.TenKhac != null &&
                         HelperString.UnsignCharacter(g.TenKhac)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("gioitinh"))
                {
                    if (HelperString.UnsignCharacter(title).ToLower().Trim() == "nam")
                        searchTruyNa_item = lstTruyNa.Where(g => g.GioiTinh == 1).ToList();
                    if (HelperString.UnsignCharacter(title).ToLower().Trim() == "nu")
                        searchTruyNa_item = lstTruyNa.Where(g => g.GioiTinh == 2).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("namsinh"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.NamSinh != null &&
                         HelperString.UnsignCharacter(g.NamSinh)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("hokhau"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.HoKhauTT != null &&
                         HelperString.UnsignCharacter(g.HoKhauTT)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("tamtru"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.NoiDKTT != null &&
                         HelperString.UnsignCharacter(g.NoiDKTT)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("quoctich"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.QuocTich != null &&
                         HelperString.UnsignCharacter(g.QuocTich)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("dantoc"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.DanToc != null &&
                         HelperString.UnsignCharacter(g.DanToc)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("tencha"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.TenCha != null &&
                         HelperString.UnsignCharacter(g.TenCha)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                if (nc.Contains("tenme"))
                {
                    searchTruyNa_item = lstTruyNa.Where(g =>
                        (g.TenMe != null &&
                         HelperString.UnsignCharacter(g.TenMe)
                             .ToLower()
                             .Trim()
                             .Contains(HelperString.UnsignCharacter(title).ToLower().Trim()))).ToList();
                    searchTruyNa.AddRange(searchTruyNa_item);
                }
                ViewBag.Tieude = title;
            }
            else if(IsDinhNa == 0)
                searchTruyNa = lstTruyNa;
            if (IsDinhNa > 0)
            {
                if (IsDinhNa == 2)
                    searchTruyNa_item = lstTruyNa.Where(g => g.IsDinhNa == false).ToList();
                if (IsDinhNa == 1)
                    searchTruyNa_item = lstTruyNa.Where(g => g.IsDinhNa).ToList();
                searchTruyNa.AddRange(searchTruyNa_item);
            }
            return View(searchTruyNa);
        }
        public ActionResult Right()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var objTruyNa = _repTruyNa.Find(id);
            var lstRelatedNews = _repTruyNa.GetAll().Where(g => g.ID != id).Take(5).ToList();
            ViewBag.lstRelatedNews = lstRelatedNews;
            return View(objTruyNa);
        }
    }
}
