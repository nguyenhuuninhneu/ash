using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class VideoController : BaseController
    {
        readonly IVideoRepository _videoRepository = new VideoRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IVideoCategoryRepository _videocategoryRepository = new VideoCategoryRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IFieldFileRep _fieldsFile = new FieldFileRep();
        readonly IUserAdminRepository _repUserAdmin = new UserAdminRepository();
        //
        // GET: /Admin/Video/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            //var arrPageElement = objUser.PageElementID.Split(',').Select(int.Parse).ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            /*var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID));
            TempData["PageElement"] = lstpageElements.ToList();*/
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var lstCateVideo = _videocategoryRepository.GetAll().Where(g=>g.LangCode==langCode).ToList();
            TempData["lstCateVideo"] = lstCateVideo;
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, string CateVideo = "", int PageElementId = 0, int page = 1, string NgayDang = null, string NgayKet = null)
        {
            var lstVideo = _videoRepository.GetAll();

            LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var video in lstVideo)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(video.LangCode) && video.LangCode == language.Code)
                        video.NgonNgu = language.Name;
                }
            }

            var lstVideoCate = _videocategoryRepository.GetAll().ToList();
            TempData["lstCate"] = lstVideoCate;
            var objUser = _userAdminRepository.Find(User.ID);
            
            if (!string.IsNullOrEmpty(CateVideo))
            {
                lstVideo = lstVideo.Where(g => g.VideoCategoryId == CateVideo).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstVideo = lstVideo.Where(g => g.LangCode == LangCode);
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstVideo =
                    lstVideo.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Title.ToLower().Trim())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim())));
            }
            if (!string.IsNullOrEmpty(NgayDang) && !string.IsNullOrEmpty(NgayKet))
            {
                var ngaydang = HelperDateTime.ConvertDate(NgayDang);
                var ngayket = HelperDateTime.ConvertDate(NgayKet);
                lstVideo = lstVideo.Where(s => s.CreatedDate.Date >= ngaydang && s.CreatedDate.Date <= ngayket);
            }
            var totalVideo = lstVideo.Count();
            lstVideo = lstVideo.OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
            foreach (var item in lstVideo)
            {
                if((item.Type == 1 || item.Type == 3) && string.IsNullOrEmpty(item.Url))
                {
                    var file_video = _fieldsFile.GetFiles(item.ID).FirstOrDefault(g => g.Code == "video");
                    if (file_video != null)
                    {
                        item.Url = file_video.LinkFile;
                    }
                }
            }
            TempData["LSTVIDEO"] = lstVideo.ToList();
           
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Video/_ListData.cshtml", lstVideo),
                totalPages = Math.Ceiling(((double)totalVideo / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_Video>)TempData["LSTVIDEO"];
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                ws.Cells["A1:D1"].Merge = true;
                ws.Cells[1, 1].Value = "Danh sách Video";
                ws.Cells[2, 1].Value = "STT";
                ws.Cells[2, 2].Value = "Tên Video";
                ws.Cells[2, 3].Value = "Ngày Đăng";
                ws.Cells[2, 4].Value = "Người Đăng";
                ws.Cells["A1:F1"].Style.Font.Bold = true;
                ws.Cells["A1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:F2"].Style.Font.Bold = true;
                ws.Cells["A2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:F2"].Style.WrapText = true;
                ws.Cells["A:XFD"].Style.Font.Name = "Arial";

                ws.Row(1).Height = 30;
                ws.Row(2).Height = 30;
                ws.Column(2).Width = 70;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 30;
                int iRow = 2;
                int i = 0;
                foreach (var item in Data)
                {
                    i++;
                    iRow++;
                    ws.Cells[iRow, 1].Value = i.ToString();
                    ws.Cells[iRow, 2].Value = item.Title;
                    ws.Cells[iRow, 3].Value = string.Format("{0:dd/MM/yyyy}", item.CreatedDate);
                    ws.Cells[iRow, 4].Value = item.CreatedBy;
                    ws.Row(iRow).Height = 20;
                }
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("DS_Video-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //ngôn ngữ
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;
            //lấy danh mục

            var lstVideoCate = _videocategoryRepository.GetAll().Where(g=>g.LangCode==objLanguages.Code).ToList();
            TempData["lstCate"] = lstVideoCate;
            var objUser = _userAdminRepository.Find(User.ID);
            

            return Json(RenderViewToString("~/Areas/Admin/Views/Video/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddVideo()
        {

            var lstVideoCate = _videocategoryRepository.GetAll().ToList();
            TempData["lstCate"] = lstVideoCate;
            var objUser = _userAdminRepository.Find(User.ID);
            return Json(RenderViewToString("~/Areas/Admin/Views/Video/_Upload.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_Video obj)
        {
            //Thêm ngôn ngữ mặc định
           
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            obj.LangCode = langCode.ToString();
            var nhuanbut = Request["NewMoney"];
            var strNhuanBut = Common.RemoveChar(nhuanbut);
            obj.NewMoney = Convert.ToInt32(strNhuanBut);
            var url = Request["url_nguon_khac"];
            //loại bỏ giá trị null
            var arrVideoCateId = Request["VideoCategoryId"].Split(',')
                .Where(g => !string.IsNullOrEmpty(g)).ToArray();

            var videoCateId = string.Join(",", arrVideoCateId);
            if (!string.IsNullOrEmpty(url))
            {
                obj.Url = url;
            }
            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = User.Username;
            obj.CreateById = User.ID;
            obj.VideoCategoryId = videoCateId;
            if ((string.IsNullOrEmpty(obj.Url) && obj.Type != 1) || ((Request.Form.GetValues("linkFile") == null || (Request.Form.GetValues("linkFile") != null && String.IsNullOrEmpty(Request.Form.GetValues("linkFile")[0]))) && obj.Type == 1))
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Bạn chưa nhập url hoặc chưa upload file")
                }, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(obj.VideoCategoryId))
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Bạn chưa chọn danh mục ")
                }, JsonRequestBehavior.AllowGet);
            }
            try
            {

                _videoRepository.Add(obj);
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
                        var code = "video";
                        if (!string.IsNullOrEmpty(linkfile))
                        {
                            var namefile = Common.GetNameFile(linkfile.Split('-')[1]);
                            insert_filetoDb(namefile, linkfile, replaceName, obj.ID, code, size);
                        }
                    }
                }
                genVidoeFileJson();
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới video thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới video thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            
            var objVideo = _videoRepository.Find(id);
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objVideo.LangCode))
                langCode = objVideo.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if(objLanguages!=null)
                TempData["languages"] = objLanguages.Name;
            var lstVideoCate = _videocategoryRepository.GetAll().Where(g=>g.LangCode==langCode).ToList();
            TempData["lstCate"] = lstVideoCate;
            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "video");
            var arrLink = lstFile.Select(g => g.LinkFile);
            var arrName = lstFile.Select(g => g.ReplaceName);
            var arrSize = lstFile.Select(g => g.Size);
            TempData["group_link"] = string.Join(",", arrLink);
            TempData["group_name"] = string.Join("|", arrName);
            TempData["group_size"] = string.Join("|", arrSize);
            return Json(RenderViewToString("~/Areas/Admin/Views/Video/_Edit.cshtml", objVideo), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Video obj)
        {
            var type = Request["type-video"];
            var UrlOld = Request["urlyoutube"];
            var urlnguonkhac = Request["url_nguon_khac"];
            //loại bỏ giá trị bị null
            var arrVideoCateId = Request["VideoCategoryId"].Split(',')
                 .Where(g => !string.IsNullOrEmpty(g)).ToArray();

            var nhuanbut = Request["NewMoney"];
            var strNhuanBut = Common.RemoveChar(nhuanbut);
            obj.NewMoney = Convert.ToInt32(strNhuanBut);
            var videoCateId = string.Join(",", arrVideoCateId);
            obj.VideoCategoryId = videoCateId;
            if (type == "1")
            {
                obj.Url = obj.Url;
                obj.Type = 1;
            }
            else if (type == "2")
            {
                obj.Url = UrlOld.Replace(",", "");
                obj.Type = 2;
            }
            else if (type == "3")
            {
                obj.Url = urlnguonkhac.Replace(",", "");
                obj.Type = 3;
            }
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            try
            {
                if (string.IsNullOrEmpty(obj.VideoCategoryId))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Bạn chưa chọn danh mục ")
                    }, JsonRequestBehavior.AllowGet);
                }
                if ((string.IsNullOrEmpty(obj.Url) && obj.Type != 1) || ((Request.Form.GetValues("linkFile") == null || (Request.Form.GetValues("linkFile") != null && String.IsNullOrEmpty(Request.Form.GetValues("linkFile")[0]))) && obj.Type == 1))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Bạn chưa nhập url hoặc chưa chọn file upload")
                    }, JsonRequestBehavior.AllowGet);
                }
                _videoRepository.Edit(obj);

                //Ghifile
                var code = "video";
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
                LogController.AddLog(string.Format("Sửa Video: {0}", obj.Title), User.Username);
                genVidoeFileJson();
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật Video thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật Video thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = _videoRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _videoRepository.Edit(obj);
                    genVidoeFileJson();
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật vị trí thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
            var objVideo = _videoRepository.Find(id);

            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.Code == "video" && g.FieldID == id).ToList();
            TempData["lstFile"] = lstFile;
            return Json(RenderViewToString("~/Areas/Admin/Views/Video/Detail.cshtml", objVideo), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _videoRepository.Find(id);
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
                _videoRepository.Delete(id);

                if (obj.Type == 1)
                {
                    var file = Server.MapPath(obj.Url);
                    if (System.IO.File.Exists(file))
                    {
                        Common.TryToDelete(file);
                    }
                }
                LogController.AddLog(string.Format("Xóa Video: {0}", obj.Title), User.Username);
                genVidoeFileJson();
            }
            catch (Exception)
            {

                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa Video thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa Video thành công",
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
                    //xóa file trên sever
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
                    _videoRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            genVidoeFileJson();
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} video", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Upload()
        {
            if (Request.Files.Count <= 0)
                return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var videoExtention = new[] { "avi", "mp4", "flv", "wmv", "mov" };
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            var section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            double maxFileSize = 4 * 2048 * 2048;
            if (section != null)
            {
                maxFileSize = section.MaxRequestLength;
            }
            var now = DateTime.Now;
            var objInfo = new FileInfo();
            try
            {

                //  Get all files from Request object  
                var files = Request.Files[0];
                var path = ConfigUpload.TargetUpload;
                var fileData = files;
                var extention = GetExtention(fileData.FileName);
                if (!videoExtention.Contains(extention))
                {
                    return Json(new { status = false, msg = "Video upload không đúng định dạng cho phép." });
                }
                #region KIỂM TRA KÍCH THƯỚC FILE
                var fileSize = fileData.ContentLength;
                if (fileSize > (maxFileSize))
                {
                    return Json(new { status = false, msg = "tập tin này vượt quá dung lượng cho phép" });
                }
                if (fileSize == 0)
                {
                    return Json(new { status = false, msg = "kiểm tra lại tập tin này" });
                }
                #endregion
                #region TẠO THƯ MỤC CHỨA FILES UPLOAD
                path = string.Format("{0}/{1}", path, "Videos");
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                path = string.Format("{0}/{1}/{2}", path, now.Year, now.Month);
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    Directory.CreateDirectory(Server.MapPath(path));
                }
                #endregion
                #region COPY FILE VÀO THƯ MỤC

                var newName = string.Format("{0}-{1}", HelperEncryptor.Md5Hash(DateTime.Now.ToString()), fileData.FileName);
                fileData.SaveAs(string.Format("{0}/{1}", Server.MapPath(path), newName));
                #endregion
                objInfo = new FileInfo
                {
                    FileName = string.Format("{0}/{1}", path, newName),
                    FileNameOriginal = fileData.FileName,
                    FileSize = fileData.ContentLength < 1024 ? string.Format("{0} Bytes", (fileData.ContentLength)) : string.Format("{0} KB", (fileData.ContentLength / 1024))
                };
            }
            catch (Exception)
            {
                return Json(new { status = false, msg = "Upload không thành công", file = objInfo });
            }
            return Json(new { status = true, msg = "Upload thành công ", file = objInfo });
        }
        public ActionResult ThongKe(string tuNgay, string denNgay)
        {
            var lstVideo = _videoRepository.GetAll().Where(g => g.Status).OrderByDescending(g=>g.CreatedDate).ToList();
            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {
                lstVideo = lstVideo.Where(g => g.CreatedDate.Date >= HelperDateTime.ConvertDateTime(tuNgay) && g.CreatedDate.Date <= HelperDateTime.ConvertDateTime(denNgay)).ToList();
            }

            var objUser = new tbl_UserAdmin();
            foreach (var item in lstVideo)
            {
                objUser = new tbl_UserAdmin();
                if (item.CreateById.HasValue)
                    objUser = _repUserAdmin.Find(item.CreateById.Value);
                if (objUser != null)
                    item.TenNguoiKhoiTao = objUser.FullName;

            }

            Session["tuNgay"] = tuNgay;
            Session["denNgay"] = denNgay;
            Session["lstVideo"] = lstVideo;
            return View(lstVideo);
        }
        public string ExportThongKe()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_Video>)Session["lstVideo"];
                var tuNgay = Session["tuNgay"];
                var denNgay = Session["denNgay"];
                var strNgay = "";
                if (tuNgay != null && denNgay != null)
                    strNgay = "(" + tuNgay + " - " + denNgay + ")";
                TempData.Keep();
                var ws = package.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1:B1"].Merge = true;
                ws.Cells["A2:B2"].Merge = true;
                ws.Cells["C1:E1"].Merge = true;
                ws.Cells["C2:E2"].Merge = true;

                ws.Cells["A4:E4"].Merge = true;
                ws.Cells["A5:E5"].Merge = true;
                ws.Cells["D6:E6"].Merge = true;

                ws.Cells["A1"].Value = "BAN TUYÊN GIÁO TRUNG ƯƠNG";
                ws.Cells["A2"].Value = "TẠP CHÍ TUYÊN GIÁO";
                ws.Cells["C1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                ws.Cells["C2"].Value = "Độc lập - Tự do - Hạnh phúc";
                ws.Cells["A4"].Value = "BÁO CÁO TÌNH HÌNH NHUẬN BÚT";
                ws.Cells["A5"].Value = strNgay;
                ws.Cells["D6"].Value = "Ngày giờ xuất báo cáo: " + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);

                ws.Cells["A7"].Value = "STT";
                ws.Cells["B7"].Value = "Tên video";
                ws.Cells["C7"].Value = "Người khởi tạo";
                ws.Cells["D7"].Value = "Ngày đăng";
                ws.Cells["E7"].Value = "Nhuận bút";


                ws.Cells["A7:E7"].Style.Font.Bold = true;
                ws.Cells["A1:E6"].Style.Font.Size = 13;
                ws.Cells["A7:E7"].Style.Font.Size = 12;

                ws.Cells["A1:A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A7:E7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A7:E7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["C1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["C2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A4"].Style.Font.Bold = true;

                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A1:E6"].Style.Font.Name = "Times New Roman";

                ws.Row(7).Height = 30;
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;

                ws.Column(5).Style.Numberformat.Format = "#,#";
                int iRow = 7;
                int stt = 0;
                var objUser = new tbl_UserAdmin();
                foreach (var item in Data)
                {
                    objUser = new tbl_UserAdmin();

                    if (item.CreateById.HasValue)
                        objUser = _repUserAdmin.Find(item.CreateById.Value);
                    if (objUser != null)
                        item.TenNguoiKhoiTao = objUser.FullName;
                    stt++;
                    iRow++;
                    ws.Cells["A" + iRow].Value = stt;
                    ws.Cells["B" + iRow].Value = item.Title;
                    ws.Cells["C" + iRow].Value = item.TenNguoiKhoiTao;
                    ws.Cells["D" + iRow].Value = HelperDateTime.ConvertDateToString(item.CreatedDate);
                    ws.Cells["E" + iRow].Value = item.NewMoney;
                    ws.Cells["E" + iRow].Style.Numberformat.Format = "#,#";

                    ws.Cells["A7" + ":E" + iRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":E" + iRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":E" + iRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":E" + iRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //Căn vị trí
                    ws.Cells["A" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["D8:D" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["D8:D" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B8" + ":E" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7" + ":E" + iRow].Style.Font.Size = 11;
                    ws.Cells["A8" + ":E" + iRow].Style.WrapText = true;
                    ws.Cells["A8:E" + iRow].Style.Font.Name = "Times New Roman";

                }
                ws.Cells["D" + (Data.Count + 8) + ":E" + (Data.Count + 8)].Merge = true;
                ws.Cells[Data.Count + 8, 3].Value = "Tổng cộng: ";
                ws.Cells[Data.Count + 8,3].Style.Font.Bold = true;
                ws.Cells[Data.Count + 8, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells[Data.Count + 8, 4].Formula = string.Format("=SUM(E8:E{0})", iRow);
                ws.Cells[Data.Count + 8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[Data.Count + 8, 4].Style.Numberformat.Format = "#,#";
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("NhuanButVideo-{0}-{1}-{2}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        public class FileInfo
        {
            public string FileName { get; set; }
            public string FileSize { get; set; }
            public string FileNameOriginal { get; set; }
        }
        public string GetExtention(string file)
        {
            if (!string.IsNullOrEmpty(file))
            {
                var arr = file.Split('.');
                if (arr.Length > 0)
                {
                    return arr.Last();
                }
            }
            return "";
        }
        [HttpPost]
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _videoRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _videoRepository.Edit(obj);
            genVidoeFileJson();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
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
        public void delete_filetoDb(int FieldID = 0, string code = "tapchi")
        {
            if (FieldID > 0)
            {
                var allFileByField = _fieldsFile.GetData().Where(g => g.FieldID == FieldID && g.Code == code).Select(g => g.Id).ToList();
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
        public void genVidoeFileJson()
        {
            var lstVideo = _videoRepository.GetAll().Where(g => g.Status && g.IsHome).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            foreach (var item in lstVideo.Where(p => p.Type == 1))
            {
                //get file
                var lstFile = _fieldsFile.GetData().FirstOrDefault(g => g.FieldID == item.ID && g.Code == "video");
                if (lstFile != null)
                {
                    item.Url = lstFile.LinkFile;
                }
            }
            if (lstVideo.Count == 0)
            {
                lstVideo = _videoRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).ToList();
            }
            lstVideo = lstVideo.Take(2).ToList();      
            Common.genCommonFileJson(lstVideo, "Video.json");
        }
    }
}
