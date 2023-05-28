using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class LeftMenuController : Controller
    {
        //
        // GET: /LeftMenu/
        readonly IHomeMenuRepository _homeMenuRepository = new HomeMenuRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        public string abc = "123432";
        public ActionResult Index(int id = 0, int catid = 0)
        {
            var lstLeftMenu = new List<tbl_HomeMenu>();

            if (id > 0)
            {
                var objNews = _newsRepository.Find(id);
                if (objNews != null)
                {
                    var lstFullHomeMenus = _homeMenuRepository.GetAll();
                   // var objMenu = _homeMenuRepository.GetAll().FirstOrDefault(g => g.CategoryId == objNews.CategoryId);
                  //  FindParentMenu(objMenu, lstFullHomeMenus, ref lstLeftMenu);
                    var objParent = lstLeftMenu.FirstOrDefault(g => g.ParentID == 0);
                    FindChildMenu(objParent, lstFullHomeMenus, ref lstLeftMenu);
                    lstLeftMenu = lstLeftMenu.Distinct().OrderBy(g => g.Ordering).ToList();
                }
            }
            else if (catid > 0)
            {
                var lstFullHomeMenus = _homeMenuRepository.GetAll();
                var objMenu = _homeMenuRepository.GetAll().FirstOrDefault(g => g.CategoryId == catid);
                FindParentMenu(objMenu, lstFullHomeMenus, ref lstLeftMenu);
                var objParent = lstLeftMenu.FirstOrDefault(g => g.ParentID == 0);
                FindChildMenu(objParent, lstFullHomeMenus, ref lstLeftMenu);
                lstLeftMenu = lstLeftMenu.Distinct().OrderBy(g => g.Ordering).ToList();
            }
            return View(lstLeftMenu.Where(g=>g.Status).ToList());
        }

        public void FindChildMenu(tbl_HomeMenu parent, List<tbl_HomeMenu> lstFullHomeMenus, ref List<tbl_HomeMenu> lstLeftMenu)
        {
            if (parent != null)
            {
                lstLeftMenu.Add(parent);
                var lstChild = lstFullHomeMenus.Where(g => g.ParentID == parent.ID).ToList();
                if (lstChild.Count > 0)
                {
                    foreach (var child in lstChild)
                    {
                        FindChildMenu(child, lstFullHomeMenus, ref lstLeftMenu);
                    }
                }
            }
            
        }

        public void FindParentMenu(tbl_HomeMenu child, List<tbl_HomeMenu> lstFullHomeMenus,
            ref List<tbl_HomeMenu> lstLeftMenu)
        {
            if (child != null)
            {
                lstLeftMenu.Add(child);
                if (child.ParentID > 0)
                {
                    var parent = lstFullHomeMenus.FirstOrDefault(g => g.ID == child.ParentID);
                    if (parent != null)
                    {
                        FindParentMenu(parent, lstFullHomeMenus, ref lstLeftMenu);
                    }
                }
            }
            
        }
    }
}
