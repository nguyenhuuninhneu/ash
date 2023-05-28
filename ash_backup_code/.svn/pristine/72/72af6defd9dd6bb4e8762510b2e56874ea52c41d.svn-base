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
    public class QAController : BaseController
    {
        //
        // GET: /QA/
        readonly IQAReporitory _qaReporitory = new QAReporitory();
        readonly IConfigNoteRepository RepNote = new ConfigNoteRepository();
        readonly IQALinhVucRepository _QALinhVucReporitory = new QALinhVucRepository();
        private PortalNewsEntities dbcontext = new PortalNewsEntities();

        public ActionResult Index(int LinhVucID = 0, string keyword = "", int page = 1)
        {
            ViewBag.keyword = keyword;
            ViewBag.LinhVucID = LinhVucID;
            ViewBag.countList = _qaReporitory.GetAll().Count(g => g.Status);
            // linh vuc cau hoi
            ViewBag.LinhVuc = _QALinhVucReporitory.GetAll().Where(g=>g.isPublish==true).OrderBy(g => g.Ordering).ThenByDescending(g=>g.ID).ToList();
            return View();
        }
        public ActionResult ListData(int LinhVucID = 0, string keyword = "", int page = 1)
        {            
            var lstQa = _qaReporitory.GetAll().Where(g => g.Status);
            if (!string.IsNullOrEmpty(keyword))
            {
                lstQa =
                    lstQa.Where(
                        g =>
                            (g.Question != null && HelperString.UnsignCharacter(g.Question.Trim().ToLower()).Contains(HelperString.UnsignCharacter(keyword.Trim().ToLower()))) 
                            || (g.FullNameOfQuestion != null && HelperString.UnsignCharacter(g.FullNameOfQuestion.Trim().ToLower()).Contains(HelperString.UnsignCharacter(keyword.Trim().ToLower())))
                            || (g.Answer != null && HelperString.UnsignCharacter(g.Answer.Trim().ToLower()).Contains(HelperString.UnsignCharacter(keyword.Trim().ToLower())))
                            );
            }
            if (LinhVucID > 0)
            {
                lstQa = lstQa.Where(g => g.LinhVucID == LinhVucID);
            }
            var totalQa = lstQa.Count();
            lstQa = lstQa.OrderByDescending(g => g.CreatedDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();

            var lstQaQuestion = _qaReporitory.GetAll().ToList();
            foreach (var item in lstQaQuestion)
            {
                var obj = _QALinhVucReporitory.GetAll().FirstOrDefault(x => x.ID == item.LinhVucID);
                if(obj!=null)
                {
                    item.LinhVuc = obj.Name;
                }
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/QA/_ListData.cshtml", lstQa),
                totalPages = Math.Ceiling(((double)totalQa / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            var lstQa = _qaReporitory.Find(id);
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/QA/_DetailsData.cshtml", lstQa),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Lienhe()
        {
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            TempData["Note"] = HttpUtility.HtmlDecode(RepNote.GetAll().FirstOrDefault().LuuYHoiDap);
            // linh vuc cau hoi
            var modelmap = dbcontext.tbl_ContactInfo.FirstOrDefault();
            TempData["Model"] = modelmap;
            ViewBag.LinhVuc = _QALinhVucReporitory.GetAll().OrderBy(g => g.Ordering).ToList();
            return View();
        }
        public ActionResult SendQuestion()
        {
            string publicKey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            ViewBag.PublicKey = publicKey;
            TempData["Note"] = HttpUtility.HtmlDecode(RepNote.GetAll().FirstOrDefault().LuuYHoiDap);
            // linh vuc cau hoi
            var modelmap = dbcontext.tbl_ContactInfo.FirstOrDefault();
            TempData["Model"] = modelmap;
            ViewBag.LinhVuc = _QALinhVucReporitory.GetAll().OrderBy(g => g.Ordering).ToList();
            //lấy ra danh sách các lĩnh vực 
            var lstLinhVuc = _QALinhVucReporitory.GetAll().Where(g => g.isPublish == true).OrderBy(g => g.Ordering).ThenByDescending(g => g.ID).ToList();
            TempData["lstLinhVuc"] = lstLinhVuc;
            return View();
        }
        [HttpPost]
        public ActionResult SendQuestion(tbl_QuestionAndAnswer obj)
        {
            /*string privateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];
            var response = Request["g-recaptcha-response"];
            string secretKey = privateKey;
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var objCapchat = JObject.Parse(result);
            var status = (bool)objCapchat.SelectToken("success");

            if (status)
            {*/
                try
                {
                    obj.Status = false;
                    obj.CreatedDate = DateTime.Now;
                    _qaReporitory.Add(obj);
                    // gui mail đến admin ( config trong phan luu y )
                    var recall = new SendMailerController();
                    var tblConfigLuuY = RepNote.GetAll().FirstOrDefault();
                    if (tblConfigLuuY != null)
                    {
                        var strEmail = tblConfigLuuY.EmailNhanYKien;
                        if (!string.IsNullOrEmpty(strEmail))
                        {
                            strEmail = strEmail.Replace(";", ",");
                            recall.DongGopYKien(strEmail);
                        }
                    }
                    //
                    return Redirect("/pages/gui-cau-hoi.html?success=1");
                }
                catch (Exception)
                {
                    return Redirect("/pages/gui-cau-hoi.html?success=1");
                }
            /*}*/
            return Redirect("/pages/gui-cau-hoi.html?captcha=0");
        }
    }
}
