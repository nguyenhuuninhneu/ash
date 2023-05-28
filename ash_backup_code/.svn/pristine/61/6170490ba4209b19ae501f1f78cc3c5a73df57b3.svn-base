using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class CategoryConfigController : BaseController
    {
        readonly ICategoryRepository _categoryRepository = new CategoryRepository();
       
        const string Keycache = "keycategory";
        //
        // GET: /Category/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int page = 1)
        {
           
            var lstCateMainMenu = _categoryRepository.GetAll().Where(g => g.Position != null);
            if (lstCateMainMenu != null)
            {
                lstCateMainMenu = lstCateMainMenu.Where(g => g.Position.Trim().Contains("1")).OrderBy(x => x.Ordering).ToList();
            }
            foreach (var item in lstCateMainMenu)
            {
                var objParent = lstCateMainMenu.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
           

            if (!string.IsNullOrEmpty(Name))
            {
                lstCateMainMenu =
                    lstCateMainMenu.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }
            
            var totalCategory = lstCateMainMenu.Count();
            lstCateMainMenu = lstCateMainMenu.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/CategoryConfig/_ListData.cshtml", lstCateMainMenu),
                totalPages = Math.Ceiling(((double)totalCategory / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CategoryBody()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult CategoryBody(string Name, int page = 1)
        {

            var lstCateMainMenu = _categoryRepository.GetAll().Where(g => g.Position != null);
            if (lstCateMainMenu != null)
            {
                lstCateMainMenu = lstCateMainMenu.Where(g => g.Position.Trim().Contains("3")).OrderBy(x => x.Ordering).ToList();
            }
            foreach (var item in lstCateMainMenu)
            {
                var objParent = lstCateMainMenu.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }


            if (!string.IsNullOrEmpty(Name))
            {
                lstCateMainMenu =
                    lstCateMainMenu.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }

            var totalCategory = lstCateMainMenu.Count();
            lstCateMainMenu = lstCateMainMenu.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/CategoryConfig/_ListDataBody.cshtml", lstCateMainMenu),
                totalPages = Math.Ceiling(((double)totalCategory / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryHomeRight()
        {
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult CategoryHomeRight(string Name, int page = 1)
        {

            var lstCateMainMenu = _categoryRepository.GetAll().Where(g => g.Position != null);
            if (lstCateMainMenu != null)
            {
                lstCateMainMenu = lstCateMainMenu.Where(g => g.Position.Trim().Contains("2")).OrderBy(x => x.Ordering).ToList();
            }
            foreach (var item in lstCateMainMenu)
            {
                var objParent = lstCateMainMenu.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }


            if (!string.IsNullOrEmpty(Name))
            {
                lstCateMainMenu =
                    lstCateMainMenu.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }

            var totalCategory = lstCateMainMenu.Count();
            lstCateMainMenu = lstCateMainMenu.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/CategoryConfig/_ListDataHomeRight.cshtml", lstCateMainMenu),
                totalPages = Math.Ceiling(((double)totalCategory / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CategoryFooterMenu()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult CategoryFooterMenu(string Name, int page = 1)
        {

            var lstCateMainMenu = _categoryRepository.GetAll().Where(g => g.Position != null);
            if (lstCateMainMenu != null)
            {
                lstCateMainMenu = lstCateMainMenu.Where(g => g.Position.Trim().Contains("4")).OrderBy(x => x.Ordering).ToList();
            }
            foreach (var item in lstCateMainMenu)
            {
                var objParent = lstCateMainMenu.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }


            if (!string.IsNullOrEmpty(Name))
            {
                lstCateMainMenu =
                    lstCateMainMenu.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }

            var totalCategory = lstCateMainMenu.Count();
            lstCateMainMenu = lstCateMainMenu.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/CategoryConfig/_ListDataHomeRight.cshtml", lstCateMainMenu),
                totalPages = Math.Ceiling(((double)totalCategory / Webconfig.RowLimit)),
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
                var obj = _categoryRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _categoryRepository.Edit(obj);

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
 
    }
}
