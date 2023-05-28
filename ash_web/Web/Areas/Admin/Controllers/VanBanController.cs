using OfficeOpenXml;
using OfficeOpenXml.Style;
using PublicService.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class VanBanController : BaseController
    {
        //
        // GET: /Admin/VanBan/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly ICoQuanBanHanhReporitory _coQuanBanHanhReporitory = new CoQuanBanHanhReporitory();
        readonly ILoaiVanBanRepository _loaiVanBanRepository = new LoaiVanBanRepository();
        readonly ILinhVucVanBanRepository _linhVucVanBanRepository = new LinhVucVanBanRepository();
        readonly IVanBanRepository _vanBanRepository = new VanBanRepository();
        readonly IUserRepository _userRepository = new UserRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IGroupCusRepository _groupcusRepository = new CusGroupRepository();
        readonly IChucVuRepository _chucvuRepository = new ChucVuRepository();
        private string SITE_URL = ConfigurationManager.ConnectionStrings["SITE_URL"]?.ToString();
        readonly IFieldFileRep _fieldsFile = new FieldFileRep();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string TrichYeu, string SoHieu, string LangCode, int page = 1, string NgayDang = null, string NgayKet = null)
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstCqbh = _coQuanBanHanhReporitory.GetAll();
            var lstLoaiVanBan = _loaiVanBanRepository.GetAll();
            var lstLinhVuc = _linhVucVanBanRepository.GetAll();
            TempData["CoQuanBanHanh"] = lstCqbh;
            TempData["LoaiVanBan"] = lstLoaiVanBan;
            TempData["LinhVuc"] = lstLinhVuc;

            var lstVanBan = _vanBanRepository.GetAll().OrderByDescending(g => g.CreatedDate).ToList();

            if (!string.IsNullOrEmpty(TrichYeu))
            {
                lstVanBan =
                    lstVanBan.Where(
                        g => HelperString.UnsignCharacter(g.TrichYeu.ToLower().Trim()).Contains(HelperString.UnsignCharacter(TrichYeu.ToLower().Trim()))).ToList();
            }
            if (!string.IsNullOrEmpty(SoHieu))
            {
                lstVanBan =
                    lstVanBan.Where(
                        g => HelperString.UnsignCharacter(g.SoHieu.ToLower().Trim()).Contains(HelperString.UnsignCharacter(SoHieu.ToLower().Trim()))).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstVanBan = lstVanBan.Where(g => g.LangCode == LangCode).ToList();
            }
            if (!string.IsNullOrEmpty(NgayDang) && !string.IsNullOrEmpty(NgayKet))
            {
                var ngaydang = HelperDateTime.ConvertDate(NgayDang);
                var ngayket = HelperDateTime.ConvertDate(NgayKet);
                lstVanBan = lstVanBan.Where(s => s.CreatedDate.Date >= ngaydang && s.CreatedDate.Date <= ngayket).ToList();
            }
            TempData["LstVB"] = lstVanBan;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/VanBan/_ListData.cshtml", lstVanBan),
            }, JsonRequestBehavior.AllowGet);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_VanBan>)TempData["LstVB"];
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                ws.Cells["A1:I1"].Merge = true;
                ws.Cells[1, 1].Value = "Danh Sách Văn Bản";
                ws.Cells[2, 1].Value = "STT";
                ws.Cells[2, 2].Value = "Trích yếu";
                ws.Cells[2, 3].Value = "Số ký hiệu";
                ws.Cells[2, 4].Value = "Loại Văn Bản";
                ws.Cells[2, 5].Value = "Lĩnh vực";
                ws.Cells[2, 6].Value = "Ngày hiệu lực";
                ws.Cells[2, 7].Value = "Người ký";
                ws.Cells[2, 8].Value = "Ngày Đăng";
                ws.Cells[2, 9].Value = "Người Đăng";
                ws.Cells["A1:I1"].Style.Font.Bold = true;
                ws.Cells["A1:I1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:I2"].Style.Font.Bold = true;
                ws.Cells["A2:I2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2:I2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:I2"].Style.WrapText = true;
                ws.Cells["A:XFD"].Style.Font.Name = "Arial";
                ws.Row(1).Height = 30;
                ws.Row(2).Height = 30;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 20;
                ws.Column(8).Width = 20;
                ws.Column(9).Width = 20;
                int iRow = 2;
                int i = 0;
                foreach (var item in Data)
                {
                    var userfname = _userAdminRepository.Find(Convert.ToInt32(item.ManCreate));
                    var linhvucmodel = _linhVucVanBanRepository.Find(item.LinhVucVanBanId);
                    var locaimodel = _loaiVanBanRepository.Find(item.LoaiVanBanId);
                    i++;
                    iRow++;
                    ws.Cells[iRow, 1].Value = i.ToString();
                    ws.Cells[iRow, 2].Value = item.TrichYeu;
                    ws.Cells[iRow, 3].Value = item.SoHieu;
                    ws.Cells[iRow, 4].Value = (locaimodel!=null)?locaimodel.Name:"Chưa cập nhật";
                    ws.Cells[iRow, 5].Value = (linhvucmodel != null) ? linhvucmodel.Name : "Chưa cập nhật";
                    ws.Cells[iRow, 6].Value = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    ws.Cells[iRow, 7].Value = item.NguoiKy;
                    ws.Cells[iRow, 8].Value = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    ws.Cells[iRow, 9].Value = (userfname != null) ? userfname.FullName : "Chưa cập nhật";
                    ws.Row(iRow).Height = 20;
                    //ws.Cells["B" + iRow].Style.Font.Bold = true;
                }
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("DS_VanBan-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var lstCqbh = _coQuanBanHanhReporitory.GetAll().Where(g => g.Status).ToList();
            TempData["CoQuanBanHanh"] = Common.CreateLevel(lstCqbh);
            var lstLinhVuc = _linhVucVanBanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["LinhVuc"] = lstLinhVuc;
            var lstLoaiVanBan = _loaiVanBanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["LoaiVanBan"] = lstLoaiVanBan;
            return Json(RenderViewToString("~/Areas/Admin/Views/VanBan/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_VanBan obj, string NgayHieuLuc, string NgayBanHanh)
        {
            try
            {
                var ngayHieuLuc = DateTime.MinValue;
                if (!string.IsNullOrEmpty(NgayHieuLuc))
                {
                    ngayHieuLuc = HelperDateTime.ConvertDateTime(NgayHieuLuc);
                }
                obj.NgayHieuLuc = ngayHieuLuc;
                var ngaybanhanh = DateTime.MinValue;
                if (!string.IsNullOrEmpty(NgayBanHanh))
                {
                    ngaybanhanh = HelperDateTime.ConvertDateTime(NgayBanHanh);
                }
                obj.NgayBanHanh = ngaybanhanh;
                obj.CreatedDate = DateTime.Now;
                
                _vanBanRepository.Add(obj);
                //GhiFile
                var arrLinkFile = Request.Form.GetValues("linkFile");
                var arrNameFile = Request.Form.GetValues("abcd"); // set ten file
                var arrSize = Request.Form.GetValues("sizeFile");
                if (arrLinkFile != null)
                {
                    int numfile = arrLinkFile.Count();
                    for (int i = 0; i < numfile; i++)
                    {
                        var linkfile = arrLinkFile[i];
                        var replaceName = arrNameFile[i];
                        var size = arrSize[i];
                        var code = "vanban";
                        if (!string.IsNullOrEmpty(linkfile))
                        {
                            var namefile = Common.GetNameFile(linkfile.Split('-')[1]);
                            insert_filetoDb(namefile, linkfile, replaceName, obj.ID, code, size);
                        }
                    }
                }
                LogController.AddLog(string.Format("Thêm mới văn bản: {0}", obj.TrichYeu), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới văn bản thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var lstCqbh = _coQuanBanHanhReporitory.GetAll().Where(g => g.Status).ToList();
            TempData["CoQuanBanHanh"] = Common.CreateLevel(lstCqbh);
            var lstLinhVuc = _linhVucVanBanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["LinhVuc"] = lstLinhVuc;
            var lstLoaiVanBan = _loaiVanBanRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            TempData["LoaiVanBan"] = lstLoaiVanBan;
            var objVanBan = _vanBanRepository.Find(id);
            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "vanban");
            var arrLink = lstFile.Select(g => g.LinkFile);
            var arrName = lstFile.Select(g => g.ReplaceName);
            var arrSize = lstFile.Select(g => g.Size);
            TempData["group_link"] = string.Join(",", arrLink);
            TempData["group_name"] = string.Join("|", arrName);
            TempData["group_size"] = string.Join("|", arrSize);
            return Json(RenderViewToString("~/Areas/Admin/Views/VanBan/_Edit.cshtml", objVanBan), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_VanBan obj, string NgayHieuLuc, string NgayBanHanh)
        {
            try
            {
                var ngayHieuLuc = DateTime.MinValue;
                if (!string.IsNullOrEmpty(NgayHieuLuc))
                {
                    ngayHieuLuc = HelperDateTime.ConvertDateTime(NgayHieuLuc);
                }
                obj.NgayHieuLuc = ngayHieuLuc;
                var ngaybanhanh = DateTime.MinValue;
                if (!string.IsNullOrEmpty(NgayBanHanh))
                {
                    ngaybanhanh = HelperDateTime.ConvertDateTime(NgayBanHanh);
                }
                obj.NgayBanHanh = ngaybanhanh;
              
                _vanBanRepository.Edit(obj);
                //Ghifile
                var code = "vanban";
                delete_filetoDb(obj.ID, code);
                var arrLinkFile = Request.Form.GetValues("linkFile");
                var arrNameFile = Request.Form.GetValues("abcd"); // set ten file
                var arrSize = Request.Form.GetValues("sizeFile");
                if (arrLinkFile != null)
                {
                    int numfile = arrLinkFile.Count();
                    for (int i = 0; i < numfile; i++)
                    {
                        var linkfile = arrLinkFile[i];
                        var replaceName = arrNameFile[i];
                        var size = arrSize[i];
                        if (!string.IsNullOrEmpty(linkfile))
                        {
                            var namefile = Common.GetNameFile(linkfile.Split('-')[1]);
                            insert_filetoDb(namefile, linkfile, replaceName, obj.ID, code, size);
                        }
                    }
                }

                LogController.AddLog(string.Format("Cập nhật văn bản: {0}", obj.TrichYeu), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật văn bản thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _vanBanRepository.Find(id);
                //xóa file 
                var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "vanban").ToList();
                foreach (var file in lstFile)
                {
                    //xóa db
                    _fieldsFile.DeletePermanently(file.Id);
                    //xóa server
                    var nameFile = file.LinkFile.Substring(file.LinkFile.LastIndexOf('/') + 1, file.LinkFile.Length - file.LinkFile.LastIndexOf('/') - 1);
                    if (nameFile != "")
                    {
                        var link = Common.GetPath("~/Images/", nameFile.Split('/').Last());
                        System.IO.File.Delete(link);
                    }
                }
                _vanBanRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa văn bản: {0}", obj.TrichYeu), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa văn bản thành công",
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
                    //Xóa file
                    var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == Convert.ToInt16(item) && g.Code == "vanban").ToList();
                    foreach (var file in lstFile)
                    {
                        //xóa db
                        _fieldsFile.DeletePermanently(file.Id);
                        //xóa server
                        var nameFile = file.LinkFile.Substring(file.LinkFile.LastIndexOf('/') + 1, file.LinkFile.Length - file.LinkFile.LastIndexOf('/') - 1);
                        if (nameFile != "")
                        {
                            var link = Common.GetPath("~/Images/", nameFile.Split('/').Last());
                            System.IO.File.Delete(link);
                        }
                    }
                    _vanBanRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} văn bản", count),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            var objVanBan = _vanBanRepository.Find(id);
            if (objVanBan.CoQuanBanHanhId != 0)
            {
                var objCoQuanBanHanh = _coQuanBanHanhReporitory.Find(objVanBan.CoQuanBanHanhId);
                objVanBan.CoQuanBanHanh = objCoQuanBanHanh.Name;
            }
            if (objVanBan.LoaiVanBanId != 0)
            {
                var objLoaiVB = _loaiVanBanRepository.Find(objVanBan.LoaiVanBanId);
                objVanBan.LoaiVanBan = objLoaiVB.Name;
            }
            if (objVanBan.LinhVucVanBanId != 0)
            {
                var objLinhVucVB = _linhVucVanBanRepository.Find(objVanBan.LinhVucVanBanId);
                objVanBan.LinhVucVanBan = objLinhVucVB.Name;
            }
            
            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.Code == "vanban" && g.FieldID == id).ToList();
            TempData["lstFile"] = lstFile;
            return Json(RenderViewToString("~/Areas/Admin/Views/VanBan/Detail.cshtml", objVanBan), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult SendMailOrSMS(string lstid = "", int isSMS = 0)
        {
            var lstId = Request["lstid"];
            var lstUser = _userRepository.GetAll().Where(g => g.Active).ToList();
            if (lstUser != null)
            {
                foreach (var item in lstUser)
                {
                    var itemChucvu = _chucvuRepository.Find(Convert.ToInt32(item.ChucVuId));
                    if (itemChucvu != null)
                    {
                        item.ChucVuName = itemChucvu.Name;
                    }
                }
            }
            TempData["lstUser"] = lstUser;
            var lstGroup = _groupcusRepository.Getall().Where(g => g.Status).ToList();
            TempData["lstGroup"] = lstGroup;
            TempData["id"] = lstid;

            if (isSMS == 1)
                return Json(RenderViewToString("~/Areas/Admin/Views/VanBan/SendSMS.cshtml"), JsonRequestBehavior.AllowGet);
            return Json(RenderViewToString("~/Areas/Admin/Views/VanBan/SendMail.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMail(string lstid, string email_input, string tenchude, string noidung)
        {
            var nhomnguoinhan = Request["nhomnguoinhan"];
            var nguoinhan = Request["nguoinhan"];
            var result_str = GetStrSend(nhomnguoinhan, nguoinhan, email_input, "Email");
            if (!string.IsNullOrEmpty(result_str))
            {
                var mail = new SendMailerController();
                mail.SendMailVanBan(result_str, lstid, tenchude, noidung);
            }
            return Redirect("/Admin/VanBan?success=1");
        }
        [Authorize]
        [HttpPost]
        public ActionResult SendSMS(string lstid, string sms_input, string noidung)
        {
            var nhomnguoinhan = Request["nhomnguoinhan"];
            var nguoinhan = Request["nguoinhan"];
            noidung = noidung + ". So hieu van ban: ";
            if (!string.IsNullOrEmpty(lstid))
            {
                var lstVanban = _vanBanRepository.GetAll().Where(g => g.Status).ToList();
                var arrItem = lstid.Split(',');
                if (arrItem.Length > 0)
                {
                    for (int i = 0; i < arrItem.Length; i++)
                    {
                        var objVanBan = lstVanban.FirstOrDefault(g => g.ID == Convert.ToInt32(arrItem[i]));
                        objVanBan.IsSendSMS = true;
                        _vanBanRepository.Edit(objVanBan);
                        noidung += HelperString.UnsignCharacter(objVanBan.SoHieu);
                        if (i != arrItem.Length - 1)
                        {
                            noidung += ", ";
                            //noidung += "<a href = '" + SITE_URL + "VanBanFE/details/" + objVanBan.ID + "' target = '_blank' >" + objVanBan.SoHieu + "</a>" + ".";
                        }
                    }
                }
            }
            noidung = HelperString.UnsignCharacter(noidung);
            var result_str = GetStrSend(nhomnguoinhan, nguoinhan, sms_input, "Phone");
            if (!string.IsNullOrEmpty(result_str))
            {
                var arrSMS = result_str.Split(',');
                for (int j = 0; j < arrSMS.Length; j++)
                {
                    int k = j + 1;
                    if (!string.IsNullOrEmpty(arrSMS[j]))
                    {
                        var itemSMS = arrSMS[j].Trim();
                        itemSMS = 84 + itemSMS.TrimStart('0');
                        smsViettel(itemSMS, noidung, k);
                    }
                }
            }
            return Redirect("/Admin/VanBan?success=1");
        }
        [Authorize]
        public string smsViettel(string phone = "84962240485", string content = "Ban nhan duoc van ban so ky hieu: kh012345", int requestID = 1)
        {
            var url = "http://125.235.4.202:8985/WS?WSDL";
            HttpWebRequest request = CreateWebRequest(@"http://125.235.4.202:8985/WS?WSD");
            var stringXml = @"<?xml version=""1.0"" encoding=""utf-8""?>"
                            + "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:impl=\"http://impl.bulkSms.ws/\">"
                            + "<soapenv:Header/>"
                            + "<soapenv:Body>"
                            + "<impl:wsCpMt>"
                            + "<User>bulk_cdytvn</User>"
                            + "<Password>147a@258</Password>"
                            + "<CPCode>CDYTVN</CPCode>"
                            + "<RequestID>" + requestID + "</RequestID>"
                            + "<UserID>" + phone + "</UserID>"
                            + "<ReceiverID>" + phone + "</ReceiverID>"
                            + "<ServiceID>CDYTVietnam</ServiceID>"
                            + "<CommandCode>bulksms</CommandCode>"
                            + "<Content>" + content + "</Content>"
                            + "<ContentType>F</ContentType>"
                            + "</impl:wsCpMt>"
                            + "</soapenv:Body>"
                            + "</soapenv:Envelope>";

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(stringXml.Trim());

            Stream stream = request.GetRequestStream();
            stream.Write(data, 0, data.Length);

            WebResponse response = request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream(), encoding);
            string soapResult = rd.ReadToEnd();
            return soapResult;
        }

        public static HttpWebRequest CreateWebRequest(string url, string header = "SOAP:Action", string method = "POST", string accept = "text/xml", string contentType = "")
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Headers.Add("SOAPAction", url);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = accept;
            webRequest.Method = method;
            return webRequest;
        }

        public string GetStrSend(string nhomnguoinhan = "", string nguoinhan = "", string email_input = "", string fieldname = "Email")
        {
            var strget = "";
            var lstGroup = _groupcusRepository.Getall().Where(g => g.Status).ToList();
            var lstUser = _userRepository.GetAll().Where(g => g.Active).ToList();

            //if (!string.IsNullOrEmpty(nhomnguoinhan))
            //{
            //    var arrItem = nhomnguoinhan.Split(',');
            //    if (arrItem.Length > 0)
            //    {
            //        for (int i = 0; i < arrItem.Length; i++)
            //        {
            //            var FirstOrDefault = lstGroup.FirstOrDefault(g => g.ID == Convert.ToInt32(arrItem[i]));
            //            if (FirstOrDefault != null)
            //            {
            //                var arrayUserID = lstUser.Where(g => !string.IsNullOrEmpty(g.GroupUserID) && g.GroupUserID.Split(',').Select(Int32.Parse).Contains(FirstOrDefault.ID)).Select(g => g.ID).ToArray();
            //                for (int j = 0; j < arrayUserID.Length; j++)
            //                {
            //                    var objUser = lstUser.FirstOrDefault(g => g.ID == Convert.ToInt32(arrayUserID[j]));
            //                    if (objUser != null)
            //                    {
            //                        var itemget = Common.GetValueByName(objUser, fieldname).ToString();
            //                        if (!string.IsNullOrEmpty(itemget))
            //                        {
            //                            if (j > 0)
            //                            {
            //                                strget += ",";
            //                            }
            //                            strget += itemget;
            //                        }
            //                    }
            //                }
            //            }
            //            strget += ",";
            //        }
            //    }
            //}
            if (!string.IsNullOrEmpty(nhomnguoinhan) && !string.IsNullOrEmpty(nguoinhan))
            {
                nguoinhan = nhomnguoinhan + "," + nguoinhan;
            }
            else if (!string.IsNullOrEmpty(nhomnguoinhan) && string.IsNullOrEmpty(nguoinhan))
            {
                nguoinhan = nhomnguoinhan;
            }

            if (!string.IsNullOrEmpty(nguoinhan))
            {
                var arrNguoinhan = nguoinhan.Split(',');
                if (arrNguoinhan.Length > 0)
                {
                    for (int i = 0; i < arrNguoinhan.Length; i++)
                    {
                        var FirstOrDefault = lstUser.FirstOrDefault(g => g.ID == Convert.ToInt32(arrNguoinhan[i]));
                        if (FirstOrDefault != null)
                        {
                            var itemget = Common.GetValueByName(FirstOrDefault, fieldname).ToString();
                            if (!string.IsNullOrEmpty(itemget))
                            {
                                if (i > 0)
                                {
                                    strget += ",";
                                }
                                strget += itemget;
                            }
                        }
                    }
                    strget += ",";
                }
            }

            if (!string.IsNullOrEmpty(email_input))
            {
                var arrEmailInput = email_input.Split(',');
                for (int j = 0; j < arrEmailInput.Length; j++)
                {
                    if (!string.IsNullOrEmpty(arrEmailInput[j]))
                    {
                        if (j > 0)
                        {
                            strget += ",";
                        }
                        strget += email_input.Trim();
                    }
                }
            }
            return strget;
        }
        public void insert_filetoDb(string namefile = "", string linkfile = "", string replaceName = "", int FieldID = 0, string code = "vanban", string size = "")
        {
            if (string.IsNullOrEmpty(replaceName))
                replaceName = namefile;
            var fobj = new tbl_fieldfiles();
            fobj.NameFile = namefile;
            fobj.LinkFile = linkfile;
            fobj.ReplaceName = replaceName;
            fobj.Size = size;
            fobj.FieldID = FieldID;
            fobj.Code = code;
            fobj.Status = 1;
            fobj.CreateDate = DateTime.Now;
            _fieldsFile.Create(fobj);
        }
        public void delete_filetoDb(int FieldID = 0, string code = "vanban")
        {
            if (FieldID > 0)
            {
                var allFileByField = _fieldsFile.GetData().Where(g => g.FieldID == FieldID && g.Code == code).Select(g => g.Id);
                if (allFileByField != null)
                {
                    var itemId = 0;
                    foreach (var item in allFileByField)
                    {
                        itemId = Convert.ToInt32(item);
                        _fieldsFile.DeletePermanently(itemId);
                    }
                }
            }
        }
    }
}
