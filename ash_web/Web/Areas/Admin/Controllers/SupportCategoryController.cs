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
    public class SupportCategoryController : BaseController
    {
        readonly ISupportCategory_Repository rep_SupportCategory = new SupportCategory_Repository();
        readonly ILanguagesRepository rep_Languages = new LanguagesRepository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
        readonly IModule_Repository rep_Module = new Module_Repository();
        readonly ISupport_Category_Module_Repository rep_SupportCategory_Module = new Support_Category_Module_Repository();
        //
        // GET: /Admin/Customer/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            var nameOfController = rep_AdminMenu.GetAll().FirstOrDefault(g => g.Url.Contains(controllerName))?.Name;

            TempData["controllerName"] = nameOfController;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int? Status, string CreatedDate, string LangCode, int page = 1)
        {
            var allSupportCategory = rep_SupportCategory.GetAll();
            var lstSupportCategory = allSupportCategory.Where(g => g.LangCode == LangCode).ToList();

            Session["LangCode"] = LangCode;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;

            foreach (var cat in lstSupportCategory)
            {
                AddParent(cat, lstSupportCategory, allSupportCategory);
                var lstChild = Common.GetChild(allSupportCategory, cat.ID);
                foreach (var item in lstChild.Where(p => p != cat.ID))
                {
                    _lstOtherSupportCategory.Add(rep_SupportCategory.Find(item));
                }
                _lstOtherSupportCategory.Add(cat);
            }
            lstSupportCategory.AddRange(_lstOtherSupportCategory);
            foreach (var item in lstSupportCategory)
            {
                var objParent = lstSupportCategory.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            if (Status != null)
            {
                lstSupportCategory = lstSupportCategory.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstSupportCategory = lstSupportCategory.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }

            var lstLevel = Common.CreateLevel(lstSupportCategory.OrderByDescending(g => g.ID).ToList()).Distinct();
            var total = lstLevel.Count();
            TempData["total"] = total;
            //var rowLimit = 10;
            //lstLevel = lstLevel.Skip((page - 1) * rowLimit).Take(rowLimit).ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/SupportCategory/_ListData.cshtml", lstLevel.ToList()),
                //totalPages = Math.Ceiling(((double)total / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<support_category> _lstOtherSupportCategory = new List<support_category>();
        readonly List<int> _listDic = new List<int>();
        //Tìm cha
        public void AddParent(support_category cat, List<support_category> lstOtherSupportCategory, List<support_category> lstAllSupportCategory)
        {
            if (_listDic.Contains(cat.ID) || cat.ParentID == 0)
            {
                return;
            }
            _listDic.Add(cat.ID);
            var parrent = lstOtherSupportCategory.Find(g => g.ID == cat.ParentID);
            if (parrent != null)
            {
                return;
            }
            parrent = lstAllSupportCategory.Find(g => g.ID == cat.ParentID);
            _lstOtherSupportCategory.Add(parrent);
            AddParent(parrent, lstOtherSupportCategory, lstAllSupportCategory);
        }



        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            var lstSupportCategory = Common.CreateLevel(rep_SupportCategory.GetAll().ToList());
            TempData["SupportCategory"] = lstSupportCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/SupportCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(support_category obj,support_category_module objsupportcategorymodule)
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
                rep_SupportCategory.Add(obj);
                var getIDSupCate = rep_SupportCategory.GetAll().LastOrDefault().ID;
                objsupportcategorymodule.SupportID = getIDSupCate;
                var listCheck = Request["ListCheck"].Split(',');
                if (!listCheck.Contains(""))
                {
                    for (int i = 0; i < listCheck.Count(); i++)
                    {
                        objsupportcategorymodule.ModuleID = Convert.ToInt32(listCheck[i]);
                        rep_SupportCategory_Module.Add(objsupportcategorymodule);
                    }
                }
                else
                {
                    objsupportcategorymodule.ModuleID = 0;
                    rep_SupportCategory_Module.Add(objsupportcategorymodule);
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
            var objSupportCategory = rep_SupportCategory.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstSupportCategoryModule = rep_SupportCategory_Module.GetAll().Where(g=>g.SupportID == id).ToList();
            TempData["lstSupportCategoryModule"] = lstSupportCategoryModule;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            if (objSupportCategory != null && !string.IsNullOrEmpty(objSupportCategory.ListCheck))
                TempData["ListCheck"] = objSupportCategory.ListCheck.Split(',');
            var lstSupportCategory = Common.CreateLevel(rep_SupportCategory.GetAll().Where(g => g.ID != id).ToList());
            TempData["SupportCategory"] = lstSupportCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/SupportCategory/_Edit.cshtml", objSupportCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(support_category objDocCategory, support_category_module objsupportcategorymodule)
        {
            try
            {
                objDocCategory.ListCheck = Request["ListCheck"];
                var getID = objDocCategory.ID;
                var lstSCM = rep_SupportCategory_Module.GetAll().Where(g => g.SupportID == getID);
                foreach (var item in lstSCM)
                {
                    rep_SupportCategory_Module.Delete(item.ID);
                }
                objsupportcategorymodule.SupportID = getID;
                var listCheck = Request["ListCheck"].Split(',');
                if (!listCheck.Contains(""))
                {
                    for (int i = 0; i < listCheck.Count(); i++)
                    {
                        objsupportcategorymodule.ModuleID = Convert.ToInt32(listCheck[i]);
                        rep_SupportCategory_Module.Add(objsupportcategorymodule);
                    }
                }
                else
                {
                    objsupportcategorymodule.ModuleID = 0;
                    rep_SupportCategory_Module.Add(objsupportcategorymodule);
                }
                if (objDocCategory.ParentID == null)
                {
                    objDocCategory.ParentID = 0;
                }
                objDocCategory.CreatedBy = User.ID;
                rep_SupportCategory.Edit(objDocCategory);
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
                var obj = rep_SupportCategory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_SupportCategory.Edit(obj);

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
            var objSupportCategory = rep_SupportCategory.Find(id);
            objSupportCategory.Status = !objSupportCategory.Status;
            rep_SupportCategory.Edit(objSupportCategory);
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
               
                var lstSupportCategory = rep_SupportCategory.GetAll().ToList();
                var lstSupportCategoryModule = rep_SupportCategory_Module.GetAll().Where(g => g.SupportID == id);
                foreach (var itemSCM in lstSupportCategoryModule)
                {
                    rep_SupportCategory_Module.Delete(itemSCM.ID);
                }
                var fullCatID = Common.GetChild(lstSupportCategory, id).Distinct().ToList();

                if (fullCatID != null)
                {
                    foreach (var item in fullCatID)
                    {
                        var lstSupportCategoryModuleChild = rep_SupportCategory_Module.GetAll().Where(g => g.SupportID == item);
                        foreach (var itemChild in lstSupportCategoryModuleChild)
                        {
                            rep_SupportCategory_Module.Delete(itemChild.ID);
                        }
                        rep_SupportCategory.Delete(item);
                        
                        
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
                    var alldocumentCategory = rep_SupportCategory.GetAll().ToList();
                    var fullCatID = Common.GetChild(alldocumentCategory, Convert.ToInt32(item)).Distinct();
                    var lstSupportCategoryModule = rep_SupportCategory_Module.GetAll().Where(g => g.SupportID == Convert.ToInt32(item));
                    foreach (var itemSCM in lstSupportCategoryModule)
                    {
                        rep_SupportCategory_Module.Delete(itemSCM.ID);
                    }
                    if (fullCatID != null)
                    {
                        foreach (var item2 in fullCatID)
                        {
                            var lstSupportCategoryModuleChild = rep_SupportCategory_Module.GetAll().Where(g => g.SupportID == item2);
                            foreach (var itemChild in lstSupportCategoryModuleChild)
                            {
                                rep_SupportCategory_Module.Delete(itemChild.ID);
                            }
                            rep_SupportCategory.Delete(item2);
                            
                            
                            count++;
                        }
                    }
                    rep_SupportCategory.Delete(Convert.ToInt32(item));
                    
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
            var lstData = rep_SupportCategory.GetAll();
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
