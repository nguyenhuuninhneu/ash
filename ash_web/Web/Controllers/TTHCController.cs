using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Web.Controllers
{
    public class TTHCController : BaseController
    {           
        readonly ILinhVucTTHCRepository _linhVucTTHCRepository = new LinhVucTTHCRepository();
        readonly IDonViTTHCRepository _donViTTHCRepository = new DonViTTHCRepository();
        readonly ITTHCRepository _TTHCRepository = new TTHCRepository();

        public ActionResult GetALL(int catid = 0)
        {
            var langcode = GetLangCode();
            var lstDonVi = Common.CreateLevel(_donViTTHCRepository.GetAll().ToList());
            ViewBag.DonVi = lstDonVi;
            
            var lstLinhVuc = _linhVucTTHCRepository.GetAll().ToList();
            ViewBag.LinhVuc = lstLinhVuc;            

            int unit = !string.IsNullOrEmpty(Request["unit"]) ? Convert.ToInt32(Request["unit"]) : catid;

            int type = !string.IsNullOrEmpty(Request["type"]) ? Convert.ToInt32(Request["type"]) : 0;

            string tieude = !string.IsNullOrEmpty(Request["title"]) ? Request["title"] : "";

            ViewBag.DonViId = unit;
            ViewBag.LinhVucId = type;
            ViewBag.Tieude = tieude;

            var lstTTHC = _TTHCRepository.GetAll().Where(g=>g.Status == 1);

            if (tieude != "")
            {
                lstTTHC = lstTTHC.Where(g =>
                            (g.Tieude != null &&
                            HelperString.UnsignCharacter(g.Tieude)
                                .ToLower()
                                .Trim()
                                .Contains(HelperString.UnsignCharacter(tieude).ToLower().Trim())));
            }

            if (unit != 0)
            {
                lstTTHC = lstTTHC.Where(g => g.DonViId == unit);
            }

            if (type != 0)
            {
               lstTTHC = lstTTHC.Where(g => g.LinhVucId == type);
            }

            lstTTHC = lstTTHC.OrderByDescending(g => g.CreatedDate);

            int page = !string.IsNullOrEmpty(Request["page"]) ? Convert.ToInt32(Request["page"]) : 1;


            return View(lstTTHC.ToPagedList(page, Webconfig.RowLimit));
        }
               

        public ActionResult Detail(int id)
        {
            // pulic key
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;

            var objTTHC = _TTHCRepository.Find(id);
            TempData["Comment"] = _TTHCRepository.GetAllCMT().Where(g => g.TTHCID == id && g.CommentID == null && g.Status).OrderByDescending(g=>g.CreateDate).ToList();
            ViewBag.lstReply = _TTHCRepository.GetAllCMT().Where(g => g.TTHCID == id && g.CommentID != null && g.Status).ToList();
            _TTHCRepository.Edit(objTTHC);
            if (objTTHC != null)
            {
                ViewBag.Title = objTTHC.Tieude;
            }

            return View(objTTHC);
        }

        public ActionResult DonVi()
        {
            var langcode = GetLangCode();
            var lstDonViR = Common.CreateLevel(_donViTTHCRepository.GetAll().ToList());

            return View(lstDonViR);
        }

        public ActionResult ListXemnhieu()
        {
            var langcode = GetLangCode();
            var lstXemnhieu = _TTHCRepository.GetAll().Where(g => g.Status == 1).OrderByDescending(g => g.ViewCount).Take(6).ToList();
            ViewBag.lstXemnhieu = lstXemnhieu;
            return View(lstXemnhieu);
        }

        public ActionResult LinhVuc()
        {
            var langcode = GetLangCode();
            var lstLinhVucR = _linhVucTTHCRepository.GetAll().ToList();
            return View(lstLinhVucR);
        }

        [HttpPost]
        public ActionResult Comment(tbl_TTHCComment obj)
        {
            string privateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
            var response = Request["g-recaptcha-response"];
            string secretKey = privateKey;
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var objCapchat = JObject.Parse(result);
            var status = (bool)objCapchat.SelectToken("success");
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            if (status)
            {
                try
                {
                    obj.TTHCID = Convert.ToInt32(Request["TTHCID"]);
                    obj.FullName = Request["FullName"];
                    obj.Email = Request["Email"];
                    obj.NoiDung = Request["NoiDung"];
                    obj.Status = false;
                    obj.CreateDate = DateTime.Now;
                    _TTHCRepository.AddCMT(obj);
                    var title = _TTHCRepository.Find(obj.TTHCID).Tieude;
                    var reurl = "/pages/service/" + obj.TTHCID + "/" + HelperString.RemoveMark(title) + ".html?success=1";
                    return Redirect(reurl);
                }
                catch (Exception)
                {
                    var title = _TTHCRepository.Find(obj.TTHCID).Tieude;
                    var reurl = "/pages/service/" + obj.TTHCID + "/" + HelperString.RemoveMark(title) + ".html?success=0";
                    return Redirect(reurl);
                }
            }
            var titlex = _TTHCRepository.Find(obj.TTHCID).Tieude;
            return Redirect("/pages/service/" + obj.TTHCID + "/" + HelperString.RemoveMark(titlex) + ".html?captcha=0");
        }

        [HttpPost]
        public ActionResult ReplyComment(tbl_TTHCComment obj)
        {
            string privateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
            var response = Request["g-recaptcha-response"];
            string secretKey = privateKey;
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var objCapchat = JObject.Parse(result);
            var status = (bool)objCapchat.SelectToken("success");
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            if (status)
            {
                try
                {
                    obj.CommentID = Convert.ToInt32(Request["CommentID"]);
                    obj.TTHCID = Convert.ToInt32(Request["TTHCID"]);
                    obj.FullName = Request["FullName"];
                    obj.Email = Request["Email"];
                    obj.NoiDung = Request["NoiDung"];
                    obj.Status = false;
                    obj.CreateDate = DateTime.Now;
                    _TTHCRepository.AddCMT(obj);
                    var title = _TTHCRepository.Find(obj.TTHCID).Tieude;
                    var reurl = "/pages/service/" + obj.TTHCID + "/" + HelperString.RemoveMark(title) + ".html?success=1";
                    return Redirect(reurl);
                }
                catch (Exception)
                {
                    var title = _TTHCRepository.Find(obj.TTHCID).Tieude;
                    var reurl = "/pages/service/" + obj.TTHCID + "/" + HelperString.RemoveMark(title) + ".html?success=0";
                    return Redirect(reurl);
                }
            }
            var titlex = _TTHCRepository.Find(obj.TTHCID).Tieude;
            return Redirect("/pages/service/" + obj.TTHCID + "/" + HelperString.RemoveMark(titlex) + ".html?captcha=0");
        }
    }
}
