using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class VideoCategoryController : BaseController
    {
        readonly IVideoCategoryRepository _videoCategoryRepository = new VideoCategoryRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
       
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
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
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int PageElementId = 0, int page = 1)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var lstVideo = _videoCategoryRepository.GetAll();

           var LangCode = "vn";
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

            if (!string.IsNullOrEmpty(Name))
            {
                lstVideo =
                    lstVideo.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.ToLower().Trim())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim())));
            }
            lstVideo = lstVideo.Where(g => g.LangCode == LangCode).ToList();

             var totalVideo = lstVideo.Count();
            lstVideo = lstVideo.OrderByDescending(g => g.ID).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
            TempData["LSTVIDEO"] = lstVideo.ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/VideoCategory/_ListData.cshtml", lstVideo.OrderBy(x => x.Ordering)),
                totalPages = Math.Ceiling(((double)totalVideo / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult AddVideo()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            return Json(RenderViewToString("~/Areas/Admin/Views/VideoCategory/_Upload.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //ngôn ngữ
            var languages = "vn";
            if (Session["langCodeDefaut"] != null)
                languages = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            var objUser = _userAdminRepository.Find(User.ID);
            return Json(RenderViewToString("~/Areas/Admin/Views/VideoCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_VideoCategory obj)
        {
            //Thêm ngôn ngữ mặc định
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            obj.LangCode = langCode.ToString();

            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = User.Username;
            try
            {
                _videoCategoryRepository.Add(obj);
              
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
            var objUser = _userAdminRepository.Find(User.ID);
            var objVideo = _videoCategoryRepository.Find(id);
            //ngôn ngữ
            var languages = "vn";
            if (!string.IsNullOrEmpty(objVideo.LangCode))
                languages = objVideo.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            return Json(RenderViewToString("~/Areas/Admin/Views/VideoCategory/_Edit.cshtml", objVideo), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_VideoCategory obj)
        {
            var oldobj = _videoCategoryRepository.Find(obj.ID);
            oldobj.Images = obj.Images;
            oldobj.ModifiedBy = User.Username;
            oldobj.Name = obj.Name;
            oldobj.Url = obj.Url;
            oldobj.Status = obj.Status;
          
            oldobj.Ordering = obj.Ordering;
            try
            {
                _videoCategoryRepository.Edit(oldobj);
                LogController.AddLog(string.Format("Sửa Video: {0}", obj.Name), User.Username);
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
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _videoCategoryRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _videoCategoryRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
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
                var obj = _videoCategoryRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _videoCategoryRepository.Edit(obj);

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
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _videoCategoryRepository.Find(id);
                _videoCategoryRepository.Delete(id);
                if (obj.Type == 1)
                {
                    var file = Server.MapPath(obj.Url);
                    if (System.IO.File.Exists(file))
                    {
                        Common.TryToDelete(file);
                    }
                }
                LogController.AddLog(string.Format("Xóa Video: {0}", obj.Name), User.Username);
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
                    _videoCategoryRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
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
            return Json(new { status = true, msg = "Upload thành công", file = objInfo });
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
    }
}
