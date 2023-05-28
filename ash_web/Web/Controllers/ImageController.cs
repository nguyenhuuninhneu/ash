using System;
using System.Collections.Generic; 
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using Newtonsoft.Json;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;
using Web.Core;
using Web.Areas.Admin.Controllers;  
using Web.Model.CustomModel;

namespace Web.Controllers
{
    public class ImageController : BaseController
    { 
        readonly IImagesRepository _imgRepository = new ImagesRepository(); 
        readonly IImageCategoryRepository _imgCateRepository = new ImageCategoryRepository();
        readonly ILimitRepository _limitRepository = new LimitRepository();

        public ActionResult ThuVienAnh()
        { 

            var langCode = "vn";
            if(Session["langCodeHome"]!=null)
            {
                langCode = Session["langCodeHome"].ToString();
            }
            InitPageElementId();

            var file = Common.GetPath("~/Content/Json/img_home.json");
            var data = System.IO.File.ReadAllText(file);
            var lstcatImg = JsonConvert.DeserializeObject<List<tbl_ImagesCategory>>(data); 
            TempData["LstCateImg"] = lstcatImg;
            return View();
        }
        public ActionResult CateImg()
        {
            return View();
        }
        public ActionResult DetailImg(int id)
        {
            var objCateImg = _imgCateRepository.Find(id);
            if (objCateImg != null)
            {
                objCateImg.ViewNumber = objCateImg.ViewNumber + 1;
                _imgCateRepository.Edit(objCateImg);
            }
           
            var lstImg = _imgRepository.GetAll().ToList();
            lstImg = lstImg.Where(x => x.ImageCategoryId == id && x.Status).ToList();
            TempData["LstImg"] = lstImg;
            TempData["id"] = id;
            return View(objCateImg);
        }
        public ActionResult DetailImgAjax(int id = 0)
        {
            var langCode = "vn";
            if (Session["langCodeHome"] != null)
            {
                langCode = Session["langCodeHome"].ToString();
            }
            var otheralbum = _imgCateRepository.GetAll().Where(g => g.Status && g.ID != id &&g.LangCode==langCode).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).ToList(); 
            return View(otheralbum); 
        }
    }
}
