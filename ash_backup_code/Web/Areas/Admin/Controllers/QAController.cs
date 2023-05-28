using System.Globalization;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using OfficeOpenXml.Style;
using PublicService.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;
using WebGrease.Css.Extensions;

namespace Web.Areas.Admin.Controllers
{
    public class QAController : BaseController
    {
        readonly IQAReporitory _qaReporitory = new QAReporitory();
        readonly IUserAdminRepository _userrepository = new UserAdminRepository();
        readonly IQALinhVucRepository _QALinhVucReporitory = new QALinhVucRepository();
        readonly IConfigNoteRepository _configNoteRepository = new ConfigNoteRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstQALinhVuc = _QALinhVucReporitory.GetAll().OrderBy(g => g.Ordering).ToList();
            ViewBag.lstQALinhVuc = lstQALinhVuc;
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataAdmin(string Title, string Status, string TuNgay, string DenNgay, int linhvuc = 0, int page = 1)
        {
            var lstQa = _qaReporitory.GetAll();
            if (!string.IsNullOrEmpty(Status))
            {
                lstQa = lstQa.Where(g => g.Status == Convert.ToBoolean(Status));
            }
            if (!string.IsNullOrEmpty(Title))
            {
                lstQa =
                    lstQa.Where(
                        g =>
                            HelperString.UnsignCharacter(g.FullNameOfQuestion.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Title.Trim().ToLower())) || (g.Question != null && HelperString.UnsignCharacter(g.Question.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Title.Trim().ToLower()))));
            }
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstQa = lstQa.Where(g => g.CreatedDate.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstQa = lstQa.Where(g => g.CreatedDate.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            foreach (var item in lstQa)
            {
                if(item.LinhVucID > 0)
                    item.LinhVuc = _QALinhVucReporitory.Find(item.LinhVucID).Name;
            }
            TempData["lstQA"] = lstQa.ToList();
            var totalQa = lstQa.Count();
            lstQa = lstQa.OrderByDescending(g => g.CreatedDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/QA/_ListData.cshtml", lstQa),
                totalPages = Math.Ceiling(((double)totalQa / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Answer(int id)
        {
            var objQuestion = _qaReporitory.Find(id);
            var lstQALinhVuc = _QALinhVucReporitory.GetAll().OrderBy(g => g.Ordering).ToList();
            TempData["lstQALinhVuc"] = lstQALinhVuc;
            return Json(RenderViewToString("~/Areas/Admin/Views/QA/_Answer.cshtml", objQuestion), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Edit")]
        public ActionResult Answer(tbl_QuestionAndAnswer obj)
        {
            try
            {
                var oldObj = _qaReporitory.Find(obj.ID);
                oldObj.Status = obj.Status;
                oldObj.Answer = obj.Answer;
                oldObj.UserNameOfAnswer = obj.UserNameOfAnswer;
                oldObj.AnswerBy = User.ID;
                oldObj.AnswerDate = DateTime.Now;
                var recall = new SendMailerController();
                if (!string.IsNullOrEmpty(oldObj.Email) && oldObj.Status && !string.IsNullOrEmpty(oldObj.Answer))
                {
                    recall.Reply(oldObj.Email);
                }
                _qaReporitory.Edit(oldObj);
                LogController.AddLog(string.Format("Duyệt câu hỏi: {0}", obj.Question), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Trả lời câu hỏi thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Trả lời câu hỏi thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _qaReporitory.Find(id);
                LogController.AddLog(string.Format("Xóa câu hỏi: {0}", obj.Question), User.Username);
                _qaReporitory.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa câu hỏi thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa câu hỏi thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _qaReporitory.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _qaReporitory.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
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
                    var obj = _qaReporitory.Find(Convert.ToInt32(item));
                    _qaReporitory.Delete(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa câu hỏi: {0}", obj.Question), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} câu hỏi", count),
            }, JsonRequestBehavior.AllowGet);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                ws.Cells["A1:J1"].Merge = true;
                ws.Cells[1, 1].Value = "Danh sách câu hỏi - trả lời";
                ws.Cells[2, 1].Value = "STT";
                ws.Cells[2, 2].Value = "Họ tên";
                ws.Cells[2, 3].Value = "Điện thoại";
                ws.Cells[2, 4].Value = "Địa chỉ";
                ws.Cells[2, 5].Value = "Email";
                ws.Cells[2, 6].Value = "Thời gian";
                ws.Cells[2, 7].Value = "Câu hỏi";
                ws.Cells[2, 8].Value = "Trả lời";
                ws.Cells[2, 9].Value = "Người trả lời";
                ws.Cells[2, 10].Value = "Ngày trả lời";
                ws.Cells["A1:J1"].Style.Font.Bold = true;
                ws.Cells["A1:j1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:j1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:j2"].Style.Font.Bold = true;
                ws.Cells["A2:j2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2:j2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:j2"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells["A2:j2"].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells["A2:j2"].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells["A2:j2"].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells.Style.WrapText = true;
                ws.Cells.AutoFitColumns();
                ws.Cells["A:XFD"].Style.Font.Name = "Arial";
                ws.Row(1).Height = 30;
                ws.Row(2).Height = 30;
                ws.Column(2).Width = 35;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 35;
                ws.Column(5).Width = 15;
                ws.Column(6).Width = 25;
                ws.Column(7).Width = 40;
                ws.Column(8).Width = 25;
                ws.Column(9).Width = 25;
                ws.Column(10).Width = 25;
                int iRow = 2;
                int i = 0;
                var lstData = (List<tbl_QuestionAndAnswer>) TempData["lstQA"];
                foreach (var item in lstData)
                {
                    i++;
                    iRow++;
                    string strCol = "A" + iRow + ":D" + iRow;
                    ws.Cells[iRow, 1].Value = i.ToString();
                    ws.Cells[iRow, 2].Value = item.FullNameOfQuestion;
                    ws.Cells[iRow, 3].Value = item.Phone;
                    ws.Cells[iRow, 4].Value = item.DiaChi;
                    ws.Cells[iRow, 5].Value = item.Email;
                    ws.Cells[iRow, 6].Value = string.Format("{0:dd/MM/yyyy HH:mm}", item.CreatedDate);
                    ws.Cells[iRow, 7].Value = item.Question;
                    ws.Cells[iRow, 8].Value = item.Answer;
                    ws.Cells[iRow, 9].Value = item.UserNameOfAnswer;
                    ws.Cells[iRow, 10].Value = string.Format("{0:dd/MM/yyyy HH:mm}", item.AnswerDate);

                    ws.Row(iRow).Height = 20;
                    ws.Cells["B" + iRow].Style.Font.Bold = true;
                    ws.Cells["A" + iRow + ":j" + iRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A" + iRow + ":j" + iRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A" + iRow + ":j" + iRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A" + iRow + ":j" + iRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                }
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("DanhsachCauHoiTraLoi-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
    }
}
