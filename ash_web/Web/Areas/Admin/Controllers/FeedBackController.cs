using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        readonly IFeedBackReporitory _feedbackReporitory = new FeedBackReporitory();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {            
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataAdmin(string Status, int page = 1)
        {
            var lstFeedBack = _feedbackReporitory.GetAll();
            if (!string.IsNullOrEmpty(Status))
            {
                lstFeedBack = lstFeedBack.Where(g => g.Status == Convert.ToBoolean(Status));
            }
            var totalQa = lstFeedBack.Count();
            lstFeedBack = lstFeedBack.OrderByDescending(g => g.CreatedDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/FeedBack/_ListData.cshtml", lstFeedBack),
                totalPages = Math.Ceiling(((double)totalQa / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Answer(int id)
        {
            var objQuestion = _feedbackReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/FeedBack/_Answer.cshtml", objQuestion), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Edit")]
        public ActionResult Answer(tbl_QuestionAndAnswer obj)
        {
            try
            {
                var oldObj = _feedbackReporitory.Find(obj.ID);
                oldObj.Status = true;
                oldObj.Answer = obj.Answer;
                oldObj.UserNameOfAnswer = User.Username;
                oldObj.AnswerBy = User.ID;
                oldObj.AnswerDate = DateTime.Now;
                _feedbackReporitory.Edit(oldObj);
                LogController.AddLog(string.Format("Duyệt ý kiến độc giả: {0}", obj.Question), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Trả lời ý kiến độc giả thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Trả lời ý kiến độc giả thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _feedbackReporitory.Find(id);
                LogController.AddLog(string.Format("Xóa ý kiến độc giả: {0}", obj.Question), User.Username);
                _feedbackReporitory.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa ý kiến độc giả thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa ý kiến độc giả thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _feedbackReporitory.Find(Convert.ToInt32(item));
                    _feedbackReporitory.Delete(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa ý kiến độc giả: {0}", obj.Question), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} ý kiến độc giả", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
