using Newtonsoft.Json.Linq;
using PublicService.Controllers;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class ContactController : BaseController
    {
        //
        // GET: /QA/
        readonly IContactReporitory _contactReporitory = new ContactReporitory();
        readonly IConfigNoteRepository RepNote = new ConfigNoteRepository();
        readonly IContactInfoRepository _contactInfoRepository = new ContactInfoRepository();
        private PortalNewsEntities dbcontext = new PortalNewsEntities();
        public ActionResult Index()
        {
            /*string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;*/
            var tblContactInfo = _contactInfoRepository.GetAll().FirstOrDefault();
            if (tblContactInfo != null)
                ViewBag.ContactInfo = tblContactInfo.NoiDung;
            var modelmap = dbcontext.tbl_ContactInfo.FirstOrDefault();
            TempData["Model"] = modelmap;
            return View();
        }

        [HttpPost]
        public ActionResult SendQuestion(tbl_Contact obj)
        {
            string privateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
            var response = Request["g-recaptcha-response"];
            string secretKey = privateKey;
            //var client = new WebClient();
            //var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            //var objCapchat = JObject.Parse(result);
            //var status = (bool)objCapchat.SelectToken("success");

            //if (status)
            //{
            try
            {
                obj.Status = false;
                obj.CreatedDate = DateTime.Now;
                _contactReporitory.Add(obj);
                // gui mail đến admin ( config trong phan luu y )
                var recall = new SendMailerController();
                var tblConfigLuuY = RepNote.GetAll().FirstOrDefault();
                if (tblConfigLuuY != null)
                {
                    var strEmail = tblConfigLuuY.EmailNhanYKien;
                    if (!string.IsNullOrEmpty(strEmail))
                    {
                        strEmail = strEmail.Replace(";", ",");
                        recall.DongGopYKienLienhe(strEmail);
                    }
                }
                return Redirect("/pages/lien-he.html?success=1");
            }
            catch (Exception)
            {
                return Redirect("/pages/lien-he.html?success=0");
            }
        }
        public ActionResult ListData(int page = 1)
        {
            var lstQa = _contactReporitory.GetAll().Where(g => g.Status);
            var totalQa = lstQa.Count();
            lstQa = lstQa.OrderByDescending(g => g.CreatedDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Contact/_ListData.cshtml", lstQa),
                totalPages = Math.Ceiling(((double)totalQa / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
