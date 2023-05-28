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

namespace Web.Controllers
{
    public class CalendarController : BaseController
    {
        //
        private readonly ICalendarRepository _calendarRepository = new CalendarRepository();
        public ActionResult Index()
        {
            if (User == null)
                return Redirect("/User/Login");
            int maxid = _calendarRepository.GetAll().OrderByDescending(g => g.ID).Select(g => g.ID).FirstOrDefault();
            if (Request["id"] != null)
            {
                maxid = Convert.ToInt32(Request["id"]);
            }
            var objVanBan = _calendarRepository.Find(maxid);
            if (objVanBan == null) return View(new tbl_Calendar());

            var lstRelated =
                _calendarRepository.GetAll().Where(g => g.ID != maxid).OrderByDescending(g => g.ID).Take(20).ToList();
            ViewBag.RelatedVB = lstRelated;
            return View(objVanBan);
        }
    }
}
