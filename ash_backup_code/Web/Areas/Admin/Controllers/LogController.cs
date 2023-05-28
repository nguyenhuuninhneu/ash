using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class ThongKeContent
    {
        public DateTime Date { get; set; }
        public int IE { get; set; }
        public int Firefox { get; set; }
        public int Chrome { get; set; }
        public int Safari { get; set; }
        public int Opera { get; set; }
        public int BrowserKhac { get; set; }
        public int Mobile { get; set; }
        public int Tablet { get; set; }
        public int Desktop { get; set; }
        public int Tong { get; set; }
    }
    public class LogController : BaseController
    {
        //
        // GET: /Admin/Log/
        readonly static ILogRepository LogRepository = new LogRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ThongKe()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(string search, string fromdate, string todate, int day = 7, int page = 1)
        {
            //Mặc định là 7 ngày gần nhất
            var fromDate = DateTime.Now.AddDays(-10).Date;
            var toDate = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(fromdate))
            {
                fromDate = HelperDateTime.ConvertDateTime(fromdate);
            }
            if (!string.IsNullOrEmpty(todate))
            {
                toDate = HelperDateTime.ConvertDateTime(todate);
            }
            var lstLog = LogRepository.GetAll().Where(g => g.CreatedDate >= fromDate && g.CreatedDate <= toDate).OrderByDescending(g => g.CreatedDate).ToList();

            var lstDetails = new List<tbl_ContentLogs>();
            foreach (var log in lstLog)
            {
                var contenLogs = JsonConvert.DeserializeObject<List<tbl_ContentLogs>>(log.ContentJson).OrderByDescending(g => g.ActionTime);//HelperXml.DeserializeXml2List<tbl_ContentLogs>(log.Contents).OrderByDescending(g => g.ActionTime);
                foreach (var item in contenLogs)
                {
                    item.ParentID = log.ID;
                }
                lstDetails.AddRange(contenLogs);
            }
            if (!string.IsNullOrEmpty(search))
            {
                lstDetails =
                    lstDetails.Where(
                        g => HelperString.UnsignCharacter(g.ActionBy.ToLower().Trim()).Contains(search.ToLower().Trim())).ToList();
            }
            var totalLog = lstDetails.Count;
            lstDetails = lstDetails.OrderByDescending(g => g.ActionTime).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            TempData["LogVinh"] = lstDetails;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Log/_ListData.cshtml", lstDetails),
                totalPages = Math.Ceiling(((double)totalLog / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataThongke(string search, string fromdate, string todate, int day = 7, int page = 1)
        {
            //Mặc định là 7 ngày gần nhất
            var fromDate = DateTime.Now.AddDays(-10).Date;
            var toDate = DateTime.Now.AddDays(1).Date;
            if (!string.IsNullOrEmpty(fromdate))
            {
                fromDate = HelperDateTime.ConvertDateTime(fromdate);
            }
            if (!string.IsNullOrEmpty(todate))
            {
                toDate = HelperDateTime.ConvertDateTime(todate);
            }
            var lstLog = LogRepository.GetAll().Where(g => g.CreatedDate >= fromDate && g.CreatedDate <= toDate).OrderByDescending(g => g.CreatedDate).ToList();

            var lstDetails = new List<tbl_ContentLogs>();
            foreach (var log in lstLog)
            {
                var contenLogs = JsonConvert.DeserializeObject<List<tbl_ContentLogs>>(log.ContentJson).Where(p => !String.IsNullOrEmpty(p.DeviceType) && !String.IsNullOrEmpty(p.BrowserType)).OrderByDescending(g => g.ActionTime);//HelperXml.DeserializeXml2List<tbl_ContentLogs>(log.Contents).OrderByDescending(g => g.ActionTime);
                foreach (var item in contenLogs)
                {
                    item.ParentID = log.ID;
                }
                lstDetails.AddRange(contenLogs);
            }
            var lstDate = lstDetails.Select(p => p.ActionTime.Date).Distinct();
            var lstData = new List<ThongKeContent>();
            foreach (var item in lstDate)
            {
                var lstdetailsbyDate = lstDetails.Where(p => p.ActionTime.Date == item);
                var obj = new ThongKeContent()
                {
                    Date = item.Date,
                    IE = lstdetailsbyDate.Count(p => p.BrowserType.ToLower() == "ie" || p.BrowserType.ToLower() == "edge"),
                    Firefox = lstdetailsbyDate.Count(p => p.BrowserType.ToLower() == "firefox"),
                    Chrome = lstdetailsbyDate.Count(p => p.BrowserType.ToLower() == "chrome"),
                    Safari = lstdetailsbyDate.Count(p => p.BrowserType.ToLower() == "safari"),
                    Opera = lstdetailsbyDate.Count(p => p.BrowserType.ToLower() == "opera"),
                    Mobile = lstdetailsbyDate.Count(p => p.DeviceType.ToLower() == "mobile"),
                    Tablet = lstdetailsbyDate.Count(p => p.DeviceType.ToLower() == "tablet"),
                    Desktop = lstdetailsbyDate.Count(p => p.DeviceType.ToLower() == "desktop"),
                    Tong = lstdetailsbyDate.Count()
                };
                obj.BrowserKhac = obj.Tong - obj.IE - obj.Firefox - obj.Chrome - obj.Safari - obj.Opera;
                lstData.Add(obj);
            }
            var totalLog = lstData.Count;
            lstData = lstData.OrderByDescending(g => g.Date).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Log/_ListDataThongKe.cshtml", lstData),
                totalPages = Math.Ceiling(((double)totalLog / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_ContentLogs>)TempData["LogVinh"];
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                ws.Cells["A1:D1"].Merge = true;
                ws.Cells[1, 1].Value = "Lịch Sử Truy Cập";
                ws.Cells[2, 1].Value = "STT";
                ws.Cells[2, 2].Value = "Người Dùng";
                ws.Cells[2, 3].Value = "Hoạt Động";
                ws.Cells[2, 4].Value = "Thời Gian";
                ws.Cells["A1:D1"].Style.Font.Bold = true;
                ws.Cells["A1:D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:D2"].Style.Font.Bold = true;
                ws.Cells["A2:D2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:D2"].Style.WrapText = true;
                ws.Cells["A:XFD"].Style.Font.Name = "Arial";
                ws.Row(1).Height = 30;
                ws.Row(2).Height = 30;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 75;
                ws.Column(4).Width = 30;
                int iRow = 2;
                int i = 0;
                foreach (var item in Data)
                {
                    i++;
                    iRow++;
                    ws.Cells[iRow, 1].Value = i.ToString();
                    ws.Cells[iRow, 2].Value = item.ActionBy;
                    ws.Cells[iRow, 3].Value = item.Contents;
                    ws.Cells[iRow, 4].Value = string.Format("{0: dd/MM/yyyy HH:mm:ss}", item.ActionTime);
                    ws.Row(iRow).Height = 20;
                }
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("LichSu_NguoiDung-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        public static void AddLog(string actionName, string username)
        {
            var now = DateTime.Now.Date;
            var objLog = LogRepository.GetAll().FirstOrDefault(g => g.CreatedDate == now);
            var newLog = new tbl_ContentLogs
            {
                ID = Guid.NewGuid(),
                Contents = actionName,
                ActionBy = username,
                ActionTime = DateTime.Now,
            };
            List<tbl_ContentLogs> lstContentLogs;
            if (objLog != null)
            {
                lstContentLogs = JsonConvert.DeserializeObject<List<tbl_ContentLogs>>(objLog.ContentJson);//HelperXml.DeserializeXml2List<tbl_ContentLogs>(objLog.Contents);
                lstContentLogs.Add(newLog);
                var xmlContentLogs = JsonConvert.SerializeObject(lstContentLogs);//HelperXml.SerializeList2Xml(lstContentLogs);
                objLog.ContentJson = xmlContentLogs;
                LogRepository.Edit(objLog);
            }
            else
            {
                lstContentLogs = new List<tbl_ContentLogs> { newLog };
                objLog = new tbl_Logs
                {
                    CreatedDate = DateTime.Now.Date,
                    ContentJson = JsonConvert.SerializeObject(lstContentLogs)//HelperXml.SerializeList2Xml(lstContentLogs),
                };
                LogRepository.Add(objLog);
            }

        }
        public static void AddLog(string actionName, string username, string BrowserType, string DeviceType)
        {
            var now = DateTime.Now.Date;
            var objLog = LogRepository.GetAll().FirstOrDefault(g => g.CreatedDate == now);
            var newLog = new tbl_ContentLogs
            {
                ID = Guid.NewGuid(),
                Contents = actionName,
                ActionBy = username,
                ActionTime = DateTime.Now,
                BrowserType = BrowserType,
                DeviceType = DeviceType
            };
            List<tbl_ContentLogs> lstContentLogs;
            if (objLog != null)
            {
                lstContentLogs = JsonConvert.DeserializeObject<List<tbl_ContentLogs>>(objLog.ContentJson);//HelperXml.DeserializeXml2List<tbl_ContentLogs>(objLog.Contents);
                lstContentLogs.Add(newLog);
                var xmlContentLogs = JsonConvert.SerializeObject(lstContentLogs);//HelperXml.SerializeList2Xml(lstContentLogs);
                objLog.ContentJson = xmlContentLogs;
                LogRepository.Edit(objLog);
            }
            else
            {
                lstContentLogs = new List<tbl_ContentLogs> { newLog };
                objLog = new tbl_Logs
                {
                    CreatedDate = DateTime.Now.Date,
                    ContentJson = JsonConvert.SerializeObject(lstContentLogs)//HelperXml.SerializeList2Xml(lstContentLogs),
                };
                LogRepository.Add(objLog);
            }

        }
    }
}
