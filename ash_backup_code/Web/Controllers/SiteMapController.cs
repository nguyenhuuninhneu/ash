using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;
using Web.Core;

namespace Web.Controllers
{
    public class SiteMapController : BaseController
    {
        readonly IHomeMenuRepository _homeMenuRepository = new HomeMenuRepository();
       
        public ActionResult Index()
        {
            var lstData = _homeMenuRepository.GetAll().Where(g => g.Status).OrderBy(g => g.Ordering).ToList();
            return View(lstData);
        }
       
    }
}
