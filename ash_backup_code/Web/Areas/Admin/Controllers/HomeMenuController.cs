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

namespace Web.Areas.Admin.Controllers
{
    public class HomeMenuController : BaseController
    {
        readonly IHomeMenuRepository _homeMenuRepository = new HomeMenuRepository();
        readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        //
        // GET: /Admin/HomeMenu/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int PageElementId = 0, int page = 1)
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstHomeMenu = _homeMenuRepository.GetAll().ToList();
            foreach (var item in lstHomeMenu)
            {
                var objParent = lstHomeMenu.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            var lstLevel = Common.CreateLevel(lstHomeMenu.ToList());
            if (!string.IsNullOrEmpty(Name))
            {
                lstLevel =
                    lstLevel.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.ToLower().Trim())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }
            
            lstLevel = PageElementId > 0 ? lstLevel.Where(g => g.PageElementId == PageElementId).ToList() : lstLevel.ToList();
            var lstLangs = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLangs;
            TempData["PageElement"] = _pageElementRepository.GetAll();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/HomeMenu/_ListData.cshtml", lstLevel),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Index")]
        public ActionResult GetAllByLangCode(string LangCode)
        {
            LangCode = LangCode ?? Webconfig.LangCodeVn;
            var lstHomeMenu = _homeMenuRepository.GetAll().ToList();
            var lstLevel = Common.CreateLevel(lstHomeMenu.ToList());
            return Json(lstLevel, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Index")]
        public ActionResult GetAllByPageElementId(int PageElementId = 0)
        {
            var lstHomeMenu = _homeMenuRepository.GetAll();
            if (PageElementId > 0)
            {
                lstHomeMenu = lstHomeMenu.Where(g => g.PageElementId == PageElementId).ToList();
            }
            var lstLevel = Common.CreateLevel(lstHomeMenu.ToList());
            return Json(lstLevel, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
          
            var lstHomeMenu = Common.CreateLevel(_homeMenuRepository.GetAll().OrderBy(g=>g.Ordering).ToList());
            TempData["HomeMenu"] = lstHomeMenu.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/HomeMenu/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_HomeMenu obj)
        {
            try
            {
                _homeMenuRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới menu trang chủ: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objHomeMenu = _homeMenuRepository.Find(id);
            var lstHomeMenu = Common.CreateLevel(_homeMenuRepository.GetAll().OrderBy(g=>g.Ordering).ToList());
            TempData["HomeMenu"] = lstHomeMenu.ToList();
          
            return Json(RenderViewToString("~/Areas/Admin/Views/HomeMenu/_Edit.cshtml", objHomeMenu), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_HomeMenu obj)
        {
            try
            {
                _homeMenuRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật menu trang chủ: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult EditComment(int id)
        {
            var obj = _homeMenuRepository.Find(id);
            if (obj.Status)
            {
                obj.Status = false;
            }
            else
            {
                obj.Status = true;
            }
            _homeMenuRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                // xoa tin bai
                //var lstNews = _newsRepository.GetAll().Where(g => g.CategoryId == id).Select(g => g.ID).ToArray();
                //if (lstNews != null)
                //{
                //    foreach (var itemNews in lstNews)
                //    {
                //        _newsRepository.Delete(itemNews);
                //    }
                //}
                // xoa danhh muc
                var obj = _homeMenuRepository.Find(id);
                _homeMenuRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa menu trang chủ: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa menu thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    // xoa tin bai
                    //var lstNews = _newsRepository.GetAll().Where(g => g.CategoryId == Convert.ToInt32(item)).Select(g => g.ID).ToArray();
                    //if (lstNews != null)
                    //{
                    //    foreach (var itemNews in lstNews)
                    //    {
                    //        _newsRepository.Delete(itemNews);
                    //    }
                    //}
                    // xoa danhh muc
                    _homeMenuRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} menu", count),
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
                var obj = _homeMenuRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _homeMenuRepository.Edit(obj);

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
