using DevExpress.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class UploadController : BaseController
    {
        public ActionResult index()
        {
            return View();
        }

        public ActionResult UploadUpload()
        {
            UploadControlExtension.GetUploadedFiles("Upload", UploadControllerUploadSettings.UploadValidationSettings, UploadControllerUploadSettings.FileUploadComplete);
            return null;
        }

        public ActionResult HtmlEditor3Partial()
        {
            return PartialView("~/Areas/Admin/Views/Upload/_HtmlEditor3Partial.cshtml");
        }
        public ActionResult HtmlEditor3PartialImageSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedImage("HtmlEditor3", UploadControllerHtmlEditor3Settings.ImageSelectorSettings);
            return null;
        }
        public ActionResult HtmlEditor3PartialFlashSelectorUpload()
        {
            HtmlEditorExtension.SaveUploadedFlash("HtmlEditor3", UploadControllerHtmlEditor3Settings.FlashSelectorSettings);
            return null;
        }
        public ActionResult HtmlEditor3PartialImageUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditor3", UploadControllerHtmlEditor3Settings.ImageUploadValidationSettings, UploadControllerHtmlEditor3Settings.ImageUploadDirectory);
            return null;
        }

        public ActionResult HtmlEditor3PartialFlashUpload()
        {
            HtmlEditorExtension.SaveUploadedFile("HtmlEditor3", UploadControllerHtmlEditor3Settings.FlashUploadValidationSettings, UploadControllerHtmlEditor3Settings.FlashUploadDirectory);
            return null;
        }
    }
    public class UploadControllerHtmlEditor3Settings
    {
        public const string ImageUploadDirectory = "~/Content/UploadImages/";
        public const string ImageSelectorThumbnailDirectory = "~/Content/Thumb/";

        public const string FlashUploadDirectory = "~/Content/UploadFlash/";
        public const string FlashSelectorThumbnailDirectory = "~/Content/ThumbFlash/";

        public static DevExpress.Web.UploadControlValidationSettings ImageUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif", ".png" },
            MaxFileSize = 4000000
        };

        public static DevExpress.Web.UploadControlValidationSettings FlashUploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".swf" },
            MaxFileSize = 4000000
        };

        static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings imageSelectorSettings;
        static DevExpress.Web.Mvc.MVCxHtmlEditorFlashSelectorSettings flashSelectorSettings;
        public static DevExpress.Web.Mvc.MVCxHtmlEditorFlashSelectorSettings FlashSelectorSettings
        {
            get {
                if (flashSelectorSettings != null)
                {
                    flashSelectorSettings.Enabled = true;
                    flashSelectorSettings.UploadSettings.Enabled = true;
                }
                return flashSelectorSettings;
            }            
        }
        public static DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings ImageSelectorSettings
        {
            get
            {
                if (imageSelectorSettings == null)
                {
                    imageSelectorSettings = new DevExpress.Web.Mvc.MVCxHtmlEditorImageSelectorSettings(null);
                    imageSelectorSettings.Enabled = true;
                    imageSelectorSettings.UploadCallbackRouteValues = new { Controller = "Upload", Action = "HtmlEditor3PartialImageSelectorUpload" };
                    imageSelectorSettings.CommonSettings.RootFolder = ImageUploadDirectory;
                    imageSelectorSettings.CommonSettings.ThumbnailFolder = ImageSelectorThumbnailDirectory;
                    imageSelectorSettings.CommonSettings.AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".jpe", ".gif" };
                    imageSelectorSettings.UploadSettings.Enabled = true;
                }
                return imageSelectorSettings;
            }
        }
    }    

    public class UploadControllerUploadSettings
    {
        public static DevExpress.Web.UploadControlValidationSettings UploadValidationSettings = new DevExpress.Web.UploadControlValidationSettings()
        {
            AllowedFileExtensions = new string[] { ".jpg", ".jpeg" },
            MaxFileSize = 4000000
        };
        public static void FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            if (e.UploadedFile.IsValid)
            {
                // Save uploaded file to some location
            }
        }
    }


}
