

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
    public class DonViFEController : Controller
    {
        private readonly IDMChungRepository _repository = new DMChungRepository();
        readonly IDMNhomRepository _dmnhomRepository = new DMNhomRepository();
        readonly IUserRepository _userRepository = new UserRepository();
        public ActionResult Index(string Ten, int CatID = 0, int page = 1)
        {
            var Code = Request["code"];
            var lstDMNhom = _dmnhomRepository.GetAll().Where(g => g.Code == Code).ToList();
            TempData["allDMNhom"] = lstDMNhom;
            var userlst = _userRepository.GetAll().Where(s => s.Active == true).ToList();
            TempData["USER"] = userlst;
            if (CatID > 0)
                lstDMNhom = lstDMNhom.Where(g => g.ID == CatID).ToList();
            TempData["lstDMNhom"] = lstDMNhom;
            var lstDMChung = _repository.GetAll().Where(g => g.Status && g.Code == Code).ToList();
            lstDMChung = Common.CreateLevel(lstDMChung);
            if (lstDMChung != null)
            {
                foreach (var item in lstDMChung)
                {
                    //hang
                    if (!string.IsNullOrEmpty(item.BCH))
                    {
                        var lstUser = _userRepository.GetAll().Where(g => g.Active).ToList();
                        var arrItem = item.BCH.Split(',');
                        if (arrItem.Length > 1)
                        {
                            for (int i = 0; i < arrItem.Length; i++)
                            {
                                if (i > 0)
                                {
                                    item.BCHName += ", ";
                                }
                                var FirstOrDefault = lstUser.FirstOrDefault(g => g.ID == Convert.ToInt32(arrItem[i]));
                                if (FirstOrDefault != null)
                                {
                                    item.BCHName += FirstOrDefault.FullName;
                                }
                            }
                        }
                        else
                        {
                            var FirstOrDefault = lstUser.FirstOrDefault(g => g.ID == Convert.ToInt32(item.BCH));
                            if (FirstOrDefault != null)
                            {
                                item.BCHName = FirstOrDefault.FullName;
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(Ten))
            {
                lstDMChung = lstDMChung.Where(g => HelperString.UnsignCharacter(g.Ten.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Ten.ToLower().Trim()))).ToList();
            }
            if (CatID != 0)
            {
                lstDMChung = lstDMChung.Where(g => g.CatID == CatID).ToList();
            }
           
            return View(lstDMChung.ToPagedList(page, Webconfig.RowLimit));
        }
        public ActionResult Details(int id, string code = "tructhuoc")
        {
            var objDMChung = _repository.Find(id);
            var lstDMNhom = _dmnhomRepository.GetAll().ToList();
            TempData["lstDMNhom"] = lstDMNhom;

            var lstDMChung = _repository.GetAll().Where(g => g.ID != id && g.Status).Where(g => g.Code == code).OrderBy(g => g.Ordering).ToList();
            if (lstDMChung != null)
            {
                foreach (var item in lstDMChung)
                {
                    //hang
                    if (!string.IsNullOrEmpty(item.BCH))
                    {
                        var lstUser = _userRepository.GetAll().Where(g => g.Active).ToList();
                        var arrItem = item.BCH.Split(',');
                        if (arrItem.Length > 1)
                        {
                            for (int i = 0; i < arrItem.Length; i++)
                            {
                                if (i > 0)
                                {
                                    item.BCHName += ", ";
                                }
                                var FirstOrDefault = lstUser.FirstOrDefault(g => g.ID == Convert.ToInt32(arrItem[i]));
                                if (FirstOrDefault != null)
                                {
                                    item.BCHName += FirstOrDefault.FullName;
                                }
                            }
                        }
                        else
                        {
                            var FirstOrDefault = lstUser.FirstOrDefault(g => g.ID == Convert.ToInt32(item.BCH));
                            if (FirstOrDefault != null)
                            {
                                item.BCHName = FirstOrDefault.FullName;
                            }
                        }
                    }
                }
            }
            TempData["lstDMChung"] = lstDMChung;

            // pulic key
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            if (objDMChung != null)
            {
                ViewBag.Title = objDMChung.Ten;
                if (!string.IsNullOrEmpty(objDMChung.BCH))
                {
                    var allUser = _userRepository.GetAll().Where(g => g.Active);
                    var TenHienThi = "";
                    var arrItem = objDMChung.BCH.Split(',');
                    if (arrItem.Length > 1)
                    {
                        for (int i = 0; i < arrItem.Length; i++)
                        {
                            if (i > 0)
                            {
                                TenHienThi += ", ";
                            }
                            var FirstOrDefault = allUser.FirstOrDefault(g => g.ID == Convert.ToInt32(arrItem[i]));
                            if (FirstOrDefault != null)
                            {
                                TenHienThi += FirstOrDefault.FullName;
                            }
                        }
                    }
                    else
                    {
                        var FirstOrDefault = allUser.FirstOrDefault(g => g.ID == Convert.ToInt32(objDMChung.BCH));
                        if (FirstOrDefault != null)
                        {
                            TenHienThi = FirstOrDefault.FullName;
                        }
                    }
                    objDMChung.BCHName = TenHienThi;
                }
            }
            if (Request["PrintView"] == "1") return View("PrintView", objDMChung);
            return View(objDMChung);
        }
    }
}
