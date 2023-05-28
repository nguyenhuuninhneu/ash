using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{ 
    public class TopicController : BaseController
    {
        readonly ITopicRepository _topicRes = new TopicRepository();
        public ActionResult Detail(int id)
        {
            var objtopic = _topicRes.Find(id); 
            return View(objtopic);
        }

        public ActionResult ListData(int id, int page = 1)
        {
            var allcomment = _topicRes.GetAllCMT().Where(g => g.TopicID == id && g.Status).OrderBy(g=>g.CreateDate).ToList(); 
            var lstcomment = allcomment.Where(g => (g.CommentID == 0 || g.CommentID == null) && !String.IsNullOrEmpty(g.TraLoi));
            var totalComment = lstcomment.ToList().Count;
            var rowLimit = 10;
            var lstcomments = lstcomment.Skip((page - 1) * rowLimit).Take(rowLimit);
            return Json(new
            {
                viewContent = RenderViewToString("~/Views/Topic/ListData.cshtml", lstcomments),
                totalPages = Math.Ceiling(((double)totalComment / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddComment(int idnews, string fullname, string email, string content, string imgcode)
        {
            var thongbao = "";
            var thongbaocapcha = "true";
            try
            {
                if (imgcode == Session["CaptchaImageText"].ToString())
                {
                    var objcmt = new tbl_TopicComment();
                    objcmt.TopicID = idnews;
                    objcmt.CommentID = 0;
                    objcmt.FullName = fullname;
                    objcmt.Email = email;
                    objcmt.NoiDung = content;
                    objcmt.CreateDate = DateTime.Now;
                    objcmt.Status = false; 
                    objcmt.IsNew = true; 
                    objcmt.IsView = false; 
                    _topicRes.AddCMT(objcmt);
                    thongbao = "success";
                }
                else
                {
                    thongbaocapcha = "false";
                }
            }
            catch (Exception e)
            {
                thongbao = "error";
            }

            return Json(new
            {
                thongbao,
                thongbaocapcha
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
