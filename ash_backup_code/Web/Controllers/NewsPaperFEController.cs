using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Admin.Controllers;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class NewsPaperFEController : BaseController
    {
        INewsPaperRepository RepNewsPaper = new NewsPaperRepository();
        IUserAdminRepository RepUser = new UserAdminRepository();
        public ActionResult ListDataFE(string title = "", int page = 1)
        {
            var lstNews = RepNewsPaper.GetAll().Where(g => g.Status == 1).OrderByDescending(g => g.CreateDate).ToList();
            if (!string.IsNullOrEmpty(title))
            {
                lstNews = lstNews.Where(g => HelperString.UnsignCharacter(g.Name.ToLower()).Contains(HelperString.UnsignCharacter(title.ToLower()))).ToList();
                ViewBag.Tieude = title;
            }
            if (lstNews.Any())
            {
                foreach (var item in lstNews)
                {
                    var obj = lstNews.FirstOrDefault(g => g.ID == item.CreateBy);
                    if (obj != null)
                    {
                        item.UserName = obj.UserName;
                    }
                }
            }
            return View(lstNews);
        }
        public ActionResult Detail(int id)
        {
            var obj = RepNewsPaper.Find(id);
            obj.UserName = RepUser.Find(obj.CreateBy).UserName;
            TempData["NewsPaper"] = obj;
            TempData["NewsID"] = id;
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            return View();
        }
        [HttpPost]
        public ActionResult Detail(tbl_NewsPaperComment obj)
        {
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
                    RepNewsPaper.AddCmt(obj);
                    return Redirect("/pages/dong-gop-y-kien/" + obj.NewsPaperID + ".html?success=1");
                }
                catch (Exception)
                {
                    return Redirect("/pages/dong-gop-y-kien/" + obj.NewsPaperID + ".html?success=0");
                }
            }
            return Redirect("/pages/dong-gop-y-kien/" + obj.NewsPaperID + ".html?captcha=0");
        }
    }
}
