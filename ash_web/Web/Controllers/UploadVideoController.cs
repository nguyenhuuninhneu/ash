﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Web.Core;
namespace Web.Controllers
{
    public class UploadVideoController : Controller
    {

        [HttpPost]
        public JsonResult Upload()
        {
            if (Request.Files.Count <= 0) return Json(new { status = false, msg = "Bạn chưa chọn file." });
            var lstFile = new List<FileInfo>();
            
            var videoExtention = new[] { "avi", "mp4", "flv", "wmv", "mov" };
          
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
            var section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            double maxFileSize = 50 * 2048 * 2048;
            if (section != null)
            {
                maxFileSize = section.MaxRequestLength;
            }
            var now = DateTime.Now;
            try
            {

                var files = Request.Files;
                
                for (var i = 0; i < files.Count; i++)
                {
                    var path = ConfigUpload.TargetUpload;
                    var fileData = files[i];
                    if (fileData == null) continue;
                    var extention = GetExtention(fileData.FileName);

                    #region KIỂM TRA KÍCH THƯỚC FILE
                    var fileSize = fileData.ContentLength;
                    if (!videoExtention.Contains(extention))
                    {
                        return Json(new { status = false, msg = "Video upload không đúng định dạng cho phép." });
                    }
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

                    var newName = string.Format("{0}-{1}", HelperEncryptor.Md5Hash(DateTime.Now.ToString()), HelperString.UnsignCharacter(fileData.FileName));
                    fileData.SaveAs(string.Format("{0}/{1}", Server.MapPath(path), newName));
                    #endregion

                    var objInfo = new FileInfo
                    {
                        FileName = string.Format("{0}/{1}", path, newName),
                        FileNameOriginal = HelperString.UnsignCharacter(fileData.FileName),
                        FileSize = fileData.ContentLength < 1024 ? string.Format("{0} Bytes", (fileData.ContentLength)) : string.Format("{0} KB", (fileData.ContentLength / 1024))
                    };
                    lstFile.Add(objInfo);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = false, msg = "Upload không thành công", files = lstFile });
            }
            return Json(new { status = true, msg = "Upload thành công", files = lstFile });
        }
        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
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
        public class FileInfo
        {
            public string FileName { get; set; }
            public string FileSize { get; set; }
            public string FileNameOriginal { get; set; }
        }
    }
}
