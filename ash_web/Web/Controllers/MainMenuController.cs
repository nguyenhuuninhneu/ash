using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class MainMenuController : BaseController
    {
        //
        // GET: /MainMenu/
        readonly IHomeMenuRepository _homeMenuRepository = new HomeMenuRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        private PortalNewsEntities dbcontext = new PortalNewsEntities();
        public ActionResult HomeMenu()
        {
            var lstuser = dbcontext.tbl_User.OrderByDescending(s => s.ID).ToList();
            TempData["User"] = lstuser;
            var Module = Request.RequestContext.RouteData.Values["controller"].ToString();
            int actMenuId = 0;
            var strUrl = "";
            switch (Module.ToLower())
            {
                case "news":
                    actMenuId = getActiveNews();
                    break;
                case "tthc":
                    strUrl = "/pages/services/thu-tuc-hanh-chinh.html";
                    actMenuId = getActiveMenu(strUrl);
                    break;
                default:
                    strUrl = Request.Url.AbsolutePath;
                    actMenuId = getActiveMenu(strUrl);
                    break;
            }
            ViewBag.actMenuId = actMenuId;
            var lstData = _homeMenuRepository.GetAll().Where(g => g.Status && g.IsMenu).OrderBy(g => g.Ordering).ToList();
            // ngay thang
            DateTime day = DateTime.Now;
            string dayS = day.DayOfWeek.ToString();
            int dayI = ((int)day.DayOfWeek); // Do DayOfWeek là kiểu enum => Chỉ số index: đếm từ 0
            // Để lấy giá trị kiểu int thì cậu phải ép kiểu về int
            string daynow = "";
            switch (dayI)
            {
                case 0:
                    daynow = "Chủ nhật";
                    break;
                case 1:
                    daynow = "Thứ hai";
                    break;
                case 2:
                    daynow = "Thứ ba";
                    break;
                case 3:
                    daynow = "Thứ tư";
                    break;
                case 4:
                    daynow = "Thứ năm";
                    break;
                case 5:
                    daynow = "Thứ sáu";
                    break;
                case 6:
                    daynow = "Thứ bảy";
                    break;
            }
            daynow = daynow + ", " + day.ToString("dd/MM/yyyy");
            ViewBag.daynow = daynow;
            // end ngay thang
            return View(lstData);
        }

        public int getActiveMenu(string strUrl = "")
        {
            int actMenuId = 0;
            if (strUrl != "")
            {
                var objMenu = _homeMenuRepository.GetAll().FirstOrDefault(g => g.Url == strUrl);
                if (objMenu != null)
                {
                    actMenuId = objMenu.ID;
                }
            }
            return actMenuId;
        }

        public int getActiveNews(int newsid = 0, int catid = 0)
        {
            int menuId = 0;
            int cid = 0;
            var Action = Request.RequestContext.RouteData.Values["action"].ToString();
            if (Action == "GetAllNewsByCat")
            {
                if (Request.RequestContext.RouteData.Values["catid"] != null)
                    cid = Convert.ToInt32(Request.RequestContext.RouteData.Values["catid"].ToString());
                if (!string.IsNullOrEmpty(Request["catid"]))
                    cid = Convert.ToInt32(Request["catid"]);
            }

            if (Action == "Details")
            {
                int id = Convert.ToInt32(Request["id"]);
                if (Request.RequestContext.RouteData.Values["id"] != null)
                    id = Convert.ToInt32(Request.RequestContext.RouteData.Values["id"].ToString());
                var objNews = _newsRepository.Find(id);
                //if (objNews != null)
                //{
                //    cid = objNews.CategoryId;
                //}
            }

            menuId = FindParent(cid);

            return menuId;
        }

        public int FindParent(int cid = 0)
        {
            if (cid != 0)
            {
                var objCategory = _homeMenuRepository.Find(cid);
                if (objCategory != null)
                {
                    if (objCategory.ParentID == 0)
                    {
                        return cid;
                    }
                    else
                    {
                        return FindParent(objCategory.ParentID);
                    }

                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }


        }

    }
}
