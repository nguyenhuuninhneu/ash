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
    public class RecruitCategoryController : BaseController
    {
        readonly IRecruitCategory_Repository rep_RecruitCategory = new RecruitCategory_Repository();
        readonly ILanguagesRepository rep_Languages = new LanguagesRepository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
        readonly ISupportCategory_Repository rep_SupportCategory = new SupportCategory_Repository();
        readonly IAdvertCategory_Repository rep_AdvertCategory = new AdvertCategory_Repository();
        readonly IDocumentCategory_Repository rep_DocumentCategory = new DocumentCategory_Repository();
        readonly IModule_Repository rep_Module = new Module_Repository();
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
        //
        // GET: /Admin/Customer/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            var nameOfController = rep_AdminMenu.GetAll().FirstOrDefault(g => g.Url.Contains(controllerName)).Name;

            TempData["controllerName"] = nameOfController;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int? Status, string CreatedDate, string LangCode, int page = 1)
        {
            var allRecruitCategory = rep_RecruitCategory.GetAll();
            var lstRecruitCategory = allRecruitCategory.Where(g => g.LangCode == LangCode).ToList();

            Session["LangCode"] = LangCode;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;

            foreach (var cat in lstRecruitCategory)
            {
                AddParent(cat, lstRecruitCategory, allRecruitCategory);
                var lstChild = Common.GetChild(allRecruitCategory, cat.ID);
                foreach (var item in lstChild.Where(p => p != cat.ID))
                {
                    _lstOtherRecruitCategory.Add(rep_RecruitCategory.Find(item));
                }
                _lstOtherRecruitCategory.Add(cat);
            }
            lstRecruitCategory.AddRange(_lstOtherRecruitCategory);
            foreach (var item in lstRecruitCategory)
            {
                var objParent = lstRecruitCategory.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            if (Status != null)
            {
                lstRecruitCategory = lstRecruitCategory.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstRecruitCategory = lstRecruitCategory.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }

            var lstLevel = Common.CreateLevel(lstRecruitCategory.OrderByDescending(g => g.ID).ToList()).Distinct();
            var total = lstLevel.Count();
            TempData["total"] = total;
            //var rowLimit = 10;
            //lstLevel = lstLevel.Skip((page - 1) * rowLimit).Take(rowLimit).ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/RecruitCategory/_ListData.cshtml", lstLevel.ToList()),
                //totalPages = Math.Ceiling(((double)total / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<recruit_category> _lstOtherRecruitCategory = new List<recruit_category>();
        readonly List<int> _listDic = new List<int>();
        //Tìm cha
        public void AddParent(recruit_category cat, List<recruit_category> lstOtherRecruitCategory, List<recruit_category> lstAllRecruitCategory)
        {
            if (_listDic.Contains(cat.ID) || cat.ParentID == 0)
            {
                return;
            }
            _listDic.Add(cat.ID);
            var parrent = lstOtherRecruitCategory.Find(g => g.ID == cat.ParentID);
            if (parrent != null)
            {
                return;
            }
            parrent = lstAllRecruitCategory.Find(g => g.ID == cat.ParentID);
            _lstOtherRecruitCategory.Add(parrent);
            AddParent(parrent, lstOtherRecruitCategory, lstAllRecruitCategory);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstSupport = rep_SupportCategory.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstSupport"] = lstSupport;
            var lstAdvert = rep_AdvertCategory.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstAdvert"] = lstAdvert;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TempData["controllerName"] = controllerName;
            var lstRecruitCategory = Common.CreateLevel(rep_RecruitCategory.GetAll().ToList());
            TempData["RecruitCategory"] = lstRecruitCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/RecruitCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(recruit_category obj, FormCollection frm)
        {

            try
            {
                var radio = Request["view_more_detail"];
                if (radio != "" || radio != null)
                {
                    obj.view_more_detail = radio;
                }
                if (obj.ParentID == null)
                {
                    obj.ParentID = 0;
                }
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = User.ID;
                rep_RecruitCategory.Add(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            //LogController.AddLog(string.Format("Thêm mới tài liệu : {0}", obj.Name), User.Username);

            return Json(new
            {
                issuccess = true,
                messenger = "thêm mới tài liệu thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id, string support_1, string advert_1, string display_1)
        {
            var objRecruitCategory = rep_RecruitCategory.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstSupport = rep_SupportCategory.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstSupport"] = lstSupport;
            var lstAdvert = rep_AdvertCategory.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstAdvert"] = lstAdvert;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            Session["view_more"] = objRecruitCategory.view_more_detail;
            var a = Session["view_more"];
            if (string.IsNullOrEmpty(support_1))
            {
                TempData["support_1"] = "0";
            }
            else
            {
                TempData["support_1"] = support_1;
            }
            if (string.IsNullOrEmpty(advert_1))
            {
                TempData["advert_1"] = "0";
            }
            else
            {
                TempData["advert_1"] = advert_1;
            }
            if (string.IsNullOrEmpty(display_1))
            {
                TempData["display_1"] = "0";
            }
            else
            {
                TempData["display_1"] = display_1;
            }
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            TempData["controllerName"] = controllerName;
            var lstRecruitCategory = Common.CreateLevel(rep_RecruitCategory.GetAll().Where(g => g.ID != id).ToList());
            TempData["RecruitCategory"] = lstRecruitCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/RecruitCategory/_Edit.cshtml", objRecruitCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(recruit_category objRecruitCategory)
        {
            try
            {
                if (objRecruitCategory.ParentID == null)
                {
                    objRecruitCategory.ParentID = 0;
                }
                objRecruitCategory.CreatedBy = User.ID;
                objRecruitCategory.view_more_detail = Request["view_more_detail"];
                rep_RecruitCategory.Edit(objRecruitCategory);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult UpdatePosition(string value)
        {
            if (value.Contains("undefined"))
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Cập nhật vị trí thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = rep_RecruitCategory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_RecruitCategory.Edit(obj);

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
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult EditStatus(int id)
        {
            var objRecruitCategory = rep_RecruitCategory.Find(id);
            objRecruitCategory.Status = !objRecruitCategory.Status;
            rep_RecruitCategory.Edit(objRecruitCategory);
            return Json(new
            {
                IsSuccess = true,
                Messenger = string.Format("Thay đổi trạng thái thành công")
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var lstRecruitCategory = rep_RecruitCategory.GetAll().ToList();
                var fullCatID = Common.GetChild(lstRecruitCategory, id).Distinct().ToList();

                if (fullCatID != null)
                {
                    foreach (var item in fullCatID)
                    {
                        rep_RecruitCategory.Delete(item);
                    }
                }
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

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
                    var allcontactCategory = rep_RecruitCategory.GetAll().ToList();
                    var fullCatID = Common.GetChild(allcontactCategory, Convert.ToInt32(item)).Distinct();
                    if (fullCatID != null)
                    {
                        foreach (var item2 in fullCatID)
                        {
                            rep_RecruitCategory.Delete(item2);
                            count++;
                        }
                    }
                    rep_RecruitCategory.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bản ghi", count),
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CheckTrung(string id = "", string name = "")
        {
            var trungTen = false;
            var lstData = rep_RecruitCategory.GetAll();
            if (id == "")
            {
                lstData = lstData.Where(g => g.Name == name).ToList();
            }
            else
            {
                lstData = lstData.Where(g => g.Name == name && g.ID != Convert.ToInt32(id)).ToList();
            }


            if (lstData.Count > 0)
            {
                trungTen = true;
            }
            else
            {
                trungTen = false;
            }
            //Form thêm mới
            return Json(new
            {
                trungTen = trungTen
            }, JsonRequestBehavior.AllowGet);
        }

    }
}
