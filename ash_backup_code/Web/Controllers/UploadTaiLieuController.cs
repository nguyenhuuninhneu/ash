using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Web.Repository;
using Web.Repository.Entity;
using Web.Model;

namespace Web.Controllers
{
    public class UploadTaiLieuController : Controller
    {

        ITaiLieuRepository FieldsFile = new TaiLieuRepository();
        string PathUpload = "/Content/Upload/";

        public ActionResult Index(int id, bool Multiple = false, string type = "document")
        {
            ViewBag.Multiple = Multiple;
            ViewBag.Type = type;
            var lstFile = new List<tbl_TaiLieu>();
            if (id > 0)
            {
                lstFile = FieldsFile.GetAll().Where(g => g.ID == id).ToList();
            }
            ViewBag.lstFiles = lstFile;
            ViewBag.so = lstFile.Count();
            ViewBag.ID = id;
            return View();
        }

        public ActionResult Edit(int id, bool Multiple = false, string type = "document")
        {
            ViewBag.Multiple = Multiple;
            ViewBag.type = type;
            var lstFile = FieldsFile.GetAll().Where(g => g.ID == id).ToList();
            ViewBag.lstFiles = lstFile;
            ViewBag.so = lstFile.Count();
            return View();
        }

        public ActionResult ChooseFile(string name = "")
        {
            ViewBag.name = name;
            return View();
        }

        public JsonResult UploadFileCreate()
        {
            string icon = "";
            var Jsn = new List<Object>();
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                if (hpf.ContentLength == 0)
                    continue;
                if (hpf.ContentLength > 52428800)
                    continue;
                var FileName = hpf.FileName.Split('.');
                var filetype = FileName[FileName.Length - 1];
                filetype = filetype.ToLower();
                if (filetype != "jpg" && filetype != "gif" && filetype != "png" && filetype != "jpeg" &&
                    filetype != "doc" && filetype != "docx" && filetype != "pdf" && filetype != "xls" &&
                    filetype != "xlsx" && filetype != "rar" && filetype != "zip")
                    continue;
                DateTime dt = DateTime.Now; // Or whatever
                string timeStr = dt.ToString("yyyyMMddHHmm");
                var localName = Path.GetFileName(hpf.FileName);
                var temp_name = localName;
                var arr_replace_name = temp_name.Split('.');
                var replace_name = "";
                var number = arr_replace_name.Count();
                var i = 0;
                if (number == 2)
                {
                    replace_name = arr_replace_name[0];
                }
                else
                {
                    for (i = 0; i < number - 2; i++)
                    {
                        replace_name += arr_replace_name[i];
                    }
                }
                localName = Web.Repository.Entity.Utilities.RemoveMark(localName);
                var fileName = "[" + timeStr + "]" + localName.Replace(" ", "-");
                var path = Path.Combine(Server.MapPath("~/Content/Upload"), fileName);
                //Xử lý icon
                var ext = Path.GetExtension(hpf.FileName).ToLower();
                switch (ext)
                {
                    case ".doc":
                    case ".docx":
                        icon = "/Images/IconFile/WordIcon.png";
                        break;
                    case ".xls":
                    case ".xlsx":
                        icon = "/Images/IconFile/ExcelIcon.png";
                        break;
                    case ".zip":
                    case ".rar":
                        icon = "/Images/IconFile/ZipIcon.png";
                        break;
                    case ".pdf":
                        icon = "/Images/IconFile/PDFIcon.png";
                        break;
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                        icon = PathUpload + fileName;
                        break;
                    default:
                        icon = "/Images/IconFile/unknown.png";
                        break;
                }
                //Hết xử lý icon
                hpf.SaveAs(path);
                Jsn = new List<Object> { new { Url = Server.UrlEncode(PathUpload + fileName), Name = localName, Img = icon, replace_name = replace_name } };
            }
            return Json(Jsn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult defaultvz(string namefield)
        {
            ViewBag.namefield = namefield;
            return View();
        }
        public ActionResult defaultvzMultiple(string namefield)
        {
            ViewBag.namefield = namefield;
            return View();
        }
    }
}