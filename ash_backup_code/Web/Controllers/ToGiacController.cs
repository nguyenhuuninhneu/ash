using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using PublicService.Controllers;
using Web.Areas.Admin.Controllers;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class ToGiacController : BaseController
    {
        //
        // GET: /QA/
        readonly IToGiacRepository RepToGiac = new ToGiacRepository();
        readonly IConfigNoteRepository RepConfig = new ConfigNoteRepository();
        readonly ITruyNaRepository RepTruyNa = new TruyNaRepository();

        public ActionResult Index(int id = 0)
        {
            if (id != 0)
            {
                TempData["ToiPhamID"] = id;
                var ToiPham = RepTruyNa.Find(id);
                ViewBag.ToiPham = ToiPham;
            }
            TempData["Note"] = HttpUtility.HtmlDecode(RepConfig.GetAll().FirstOrDefault().LuuYToGiac);
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            return View();
        }
        [HttpPost]
        public ActionResult Index(tbl_ToGiac obj)
        {
            int? toipham_id = 0;
            if (obj.ToiPhamID > 0) toipham_id = obj.ToiPhamID;

            string privateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
            var response = Request["g-recaptcha-response"];
            string secretKey = privateKey;
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var objCapchat = JObject.Parse(result);
            var status = (bool)objCapchat.SelectToken("success");
            if (status)
            {
                try
                {
                    obj.CreateDate = DateTime.Now;
                    RepToGiac.Add(obj);
                    // gui mail đến admin ( config trong phan luu y )
                    var recall = new SendMailerController();
                    var tblConfigLuuY = RepConfig.GetAll().FirstOrDefault();
                    if (tblConfigLuuY != null)
                    {
                        var strEmail = tblConfigLuuY.EmailNhanYKien;
                        if (!string.IsNullOrEmpty(strEmail))
                        {
                            strEmail = strEmail.Replace(";", ",");
                            recall.ToGiacToiPham(strEmail);
                        }
                    }
                    //
                    return Redirect("/pages/to-giac-toi-pham/" + toipham_id + ".html?success=1");
                }
                catch (Exception)
                {
                    return Redirect("/pages/to-giac-toi-pham/" + toipham_id + ".html?success=1");
                }
            }
            return Redirect("/pages/to-giac-toi-pham/" + toipham_id + ".html?captcha=0");
        }

    }
}
