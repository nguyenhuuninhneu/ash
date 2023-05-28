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
    public class AdvertCategoryController : BaseController
    {
        readonly IAdvertCategory_Repository rep_AdvertCategory = new AdvertCategory_Repository();
        readonly ILanguagesRepository rep_Languages = new LanguagesRepository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
        readonly IModule_Repository rep_Module = new Module_Repository();
        readonly IAdvert_Category_Module_Repository rep_AdvertCategory_Module = new Advert_Category_Module_Repository();
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
            var allAdvertCategory = rep_AdvertCategory.GetAll();
            var lstAdvertCategory = allAdvertCategory.Where(g => g.LangCode == LangCode).ToList();

            Session["LangCode"] = LangCode;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;

            foreach (var cat in lstAdvertCategory)
            {
                AddParent(cat, lstAdvertCategory, allAdvertCategory);
                var lstChild = Common.GetChild(allAdvertCategory, cat.ID);
                foreach (var item in lstChild.Where(p => p != cat.ID))
                {
                    _lstOtherAdvertCategory.Add(rep_AdvertCategory.Find(item));
                }
                _lstOtherAdvertCategory.Add(cat);
            }
            lstAdvertCategory.AddRange(_lstOtherAdvertCategory);
            foreach (var item in lstAdvertCategory)
            {
                var objParent = lstAdvertCategory.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            if (Status != null)
            {
                lstAdvertCategory = lstAdvertCategory.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstAdvertCategory = lstAdvertCategory.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }

            var lstLevel = Common.CreateLevel(lstAdvertCategory.OrderByDescending(g => g.ID).ToList()).Distinct();
            var total = lstLevel.Count();
            TempData["total"] = total;
            //var rowLimit = 10;
            //lstLevel = lstLevel.Skip((page - 1) * rowLimit).Take(rowLimit).ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/AdvertCategory/_ListData.cshtml", lstLevel.ToList()),
                //totalPages = Math.Ceiling(((double)total / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<advert_category> _lstOtherAdvertCategory = new List<advert_category>();
        readonly List<int> _listDic = new List<int>();
        //Tìm cha
        public void AddParent(advert_category cat, List<advert_category> lstOtherAdvertCategory, List<advert_category> lstAllAdvertCategory)
        {
            if (_listDic.Contains(cat.ID) || cat.ParentID == 0)
            {
                return;
            }
            _listDic.Add(cat.ID);
            var parrent = lstOtherAdvertCategory.Find(g => g.ID == cat.ParentID);
            if (parrent != null)
            {
                return;
            }
            parrent = lstAllAdvertCategory.Find(g => g.ID == cat.ParentID);
            _lstOtherAdvertCategory.Add(parrent);
            AddParent(parrent, lstOtherAdvertCategory, lstAllAdvertCategory);
        }



        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            var lstAdvertCategory = Common.CreateLevel(rep_AdvertCategory.GetAll().ToList());
            TempData["AdvertCategory"] = lstAdvertCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/AdvertCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(advert_category obj,advert_category_module objadvertcategorymodule)
        {

            try
            {
                
                
                if (obj.ParentID == null)
                {
                    obj.ParentID = 0;
                }
                obj.ListCheck = Request["ListCheck"];
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = User.ID;
                rep_AdvertCategory.Add(obj);
                var getIDSupCate = rep_AdvertCategory.GetAll().LastOrDefault().ID;
                objadvertcategorymodule.AdvertID = getIDSupCate;
                var listCheck = Request["ListCheck"].Split(',');
                if (!listCheck.Contains(""))
                {
                    for (int i = 0; i < listCheck.Count(); i++)
                    {
                        objadvertcategorymodule.ModuleID = Convert.ToInt32(listCheck[i]);
                        rep_AdvertCategory_Module.Add(objadvertcategorymodule);
                    }
                }
                else
                {
                    objadvertcategorymodule.ModuleID = 0;
                    rep_AdvertCategory_Module.Add(objadvertcategorymodule);
                }
               
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
        public ActionResult Edit(int id)
        {
            var objAdvertCategory = rep_AdvertCategory.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstAdvertCategoryModule = rep_AdvertCategory_Module.GetAll().Where(g=>g.AdvertID == id).ToList();
            TempData["lstAdvertCategoryModule"] = lstAdvertCategoryModule;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            if (objAdvertCategory != null && !string.IsNullOrEmpty(objAdvertCategory.ListCheck))
                TempData["ListCheck"] = objAdvertCategory.ListCheck.Split(',');
            var lstAdvertCategory = Common.CreateLevel(rep_AdvertCategory.GetAll().Where(g => g.ID != id).ToList());
            TempData["AdvertCategory"] = lstAdvertCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/AdvertCategory/_Edit.cshtml", objAdvertCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(advert_category objAdvertCategory, advert_category_module objadvertcategorymodule)
        {
            try
            {
                objAdvertCategory.ListCheck = Request["ListCheck"];
                var getID = objAdvertCategory.ID;
                var lstSCM = rep_AdvertCategory_Module.GetAll().Where(g => g.AdvertID == getID);
                foreach (var item in lstSCM)
                {
                    rep_AdvertCategory_Module.Delete(item.ID);
                }
                objadvertcategorymodule.AdvertID = getID;
                var listCheck = Request["ListCheck"].Split(',');
                if (!listCheck.Contains(""))
                {
                    for (int i = 0; i < listCheck.Count(); i++)
                    {
                        objadvertcategorymodule.ModuleID = Convert.ToInt32(listCheck[i]);
                        rep_AdvertCategory_Module.Add(objadvertcategorymodule);
                    }
                }
                else
                {
                    objadvertcategorymodule.ModuleID = 0;
                    rep_AdvertCategory_Module.Add(objadvertcategorymodule);
                }
                if (objAdvertCategory.ParentID == null)
                {
                    objAdvertCategory.ParentID = 0;
                }
                objAdvertCategory.CreatedBy = User.ID;
                rep_AdvertCategory.Edit(objAdvertCategory);
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
                var obj = rep_AdvertCategory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_AdvertCategory.Edit(obj);

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
            var objAdvertCategory = rep_AdvertCategory.Find(id);
            objAdvertCategory.Status = !objAdvertCategory.Status;
            rep_AdvertCategory.Edit(objAdvertCategory);
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
               
                var lstAdvertCategory = rep_AdvertCategory.GetAll().ToList();
                var lstAdvertCategoryModule = rep_AdvertCategory_Module.GetAll().Where(g => g.AdvertID == id);
                foreach (var itemSCM in lstAdvertCategoryModule)
                {
                    rep_AdvertCategory_Module.Delete(itemSCM.ID);
                }
                var fullCatID = Common.GetChild(lstAdvertCategory, id).Distinct().ToList();

                if (fullCatID != null)
                {
                    foreach (var item in fullCatID)
                    {
                        var lstAdvertCategoryModuleChild = rep_AdvertCategory_Module.GetAll().Where(g => g.AdvertID == item);
                        foreach (var itemChild in lstAdvertCategoryModuleChild)
                        {
                            rep_AdvertCategory_Module.Delete(itemChild.ID);
                        }
                        rep_AdvertCategory.Delete(item);
                        
                        
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
                    var allAdvertCategory = rep_AdvertCategory.GetAll().ToList();
                    var fullCatID = Common.GetChild(allAdvertCategory, Convert.ToInt32(item)).Distinct();
                    var lstAdvertCategoryModule = rep_AdvertCategory_Module.GetAll().Where(g => g.AdvertID == Convert.ToInt32(item));
                    foreach (var itemSCM in lstAdvertCategoryModule)
                    {
                        rep_AdvertCategory_Module.Delete(itemSCM.ID);
                    }
                    if (fullCatID != null)
                    {
                        foreach (var item2 in fullCatID)
                        {
                            var lstAdvertCategoryModuleChild = rep_AdvertCategory_Module.GetAll().Where(g => g.AdvertID == item2);
                            foreach (var itemChild in lstAdvertCategoryModuleChild)
                            {
                                rep_AdvertCategory_Module.Delete(itemChild.ID);
                            }
                            rep_AdvertCategory.Delete(item2);
                            
                            
                            count++;
                        }
                    }
                    rep_AdvertCategory.Delete(Convert.ToInt32(item));
                    
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
            var lstData = rep_AdvertCategory.GetAll();
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
