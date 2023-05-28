using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class SlideImagesController : BaseController
    {
        //
        // GET: /SlideImages/
        readonly ISlideImagesRepository _slideImagesRepository = new SlideImagesRepository();
        public ActionResult HomeSlideImages()
        {
            var pageElementId = GetPageElementId();
            var lstData = _slideImagesRepository.GetAll().Where(g => g.Type == (int)Webconfig.SlideImages.Giua && g.Status).OrderByDescending(g => g.CreatedDate).Take(5).ToList();
            return View(lstData);
        }

        public ActionResult HomeSmallSlideImages()
        {
            var pageElementId = GetPageElementId();
            var lstData = _slideImagesRepository.GetAll().Where(g => g.Type == (int)Webconfig.SlideImages.Phai1 && g.Status).OrderByDescending(g => g.CreatedDate).Take(5).ToList();
            return View(lstData);
        }
    }
}
