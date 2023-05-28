using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{

    public class DocumentCategoryController : BaseController
    {
        readonly IDocumentCategory_Repository rep_DocumentCategory = new DocumentCategory_Repository();
        readonly ILanguagesRepository rep_Languages = new LanguagesRepository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
        readonly ISupportCategory_Repository rep_SupportCategory = new SupportCategory_Repository();
        readonly IAdvertCategory_Repository rep_AdvertCategory = new AdvertCategory_Repository();
        readonly IModule_Repository rep_Module = new Module_Repository();
        readonly PortalNewsEntities _entities = new PortalNewsEntities();
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
            var allDocumentCategory = rep_DocumentCategory.GetAll();
            var lstDocumentCategory = allDocumentCategory.Where(g => g.LangCode == LangCode).ToList();

            Session["LangCode"] = LangCode;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;

            foreach (var cat in lstDocumentCategory)
            {
                AddParent(cat, lstDocumentCategory, allDocumentCategory);
                var lstChild = Common.GetChild(allDocumentCategory, cat.ID);
                foreach (var item in lstChild.Where(p => p != cat.ID))
                {
                    _lstOtherDocumentCategory.Add(rep_DocumentCategory.Find(item));
                }
                _lstOtherDocumentCategory.Add(cat);
            }
            lstDocumentCategory.AddRange(_lstOtherDocumentCategory);
            foreach (var item in lstDocumentCategory)
            {
                var objParent = lstDocumentCategory.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            if (Status != null)
            {
                lstDocumentCategory = lstDocumentCategory.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstDocumentCategory = lstDocumentCategory.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }

            var lstLevel = Common.CreateLevel(lstDocumentCategory.OrderByDescending(g => g.ID).ToList()).Distinct();
            var total = lstLevel.Count();
            TempData["total"] = total;
            //var rowLimit = 10;
            //lstLevel = lstLevel.Skip((page - 1) * rowLimit).Take(rowLimit).ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DocumentCategory/_ListData.cshtml", lstLevel.ToList()),
                //totalPages = Math.Ceiling(((double)total / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<document_category> _lstOtherDocumentCategory = new List<document_category>();
        readonly List<int> _listDic = new List<int>();
        //Tìm cha
        public void AddParent(document_category cat, List<document_category> lstOtherDocumentCategory, List<document_category> lstAllDocumentCategory)
        {
            if (_listDic.Contains(cat.ID) || cat.ParentID == 0)
            {
                return;
            }
            _listDic.Add(cat.ID);
            var parrent = lstOtherDocumentCategory.Find(g => g.ID == cat.ParentID);
            if (parrent != null)
            {
                return;
            }
            parrent = lstAllDocumentCategory.Find(g => g.ID == cat.ParentID);
            _lstOtherDocumentCategory.Add(parrent);
            AddParent(parrent, lstOtherDocumentCategory, lstAllDocumentCategory);
        }

        [HttpGet]
        public ActionResult ListChildCreate(int id, string name, string code = "")
        {

            if (name.Contains("HoTroTrucTuyen"))
            {
                var lstAll = rep_SupportCategory.GetAll().ToList();
                var lstChildSupport = new List<support_category>();
                if (id.ToString() == "")
                {
                    lstChildSupport = null;
                }
                else
                {
                    //lstChildSupport = lstChildSupport.Where(g => g.ParentID == id).ToList();
                    var lstInt = Common.GetChild(lstAll, id).ToList();
                    lstChildSupport = Common.CreateLevelOutID(lstAll.Where(g => lstInt.Contains(g.ID) && g.ID != id).ToList(), id);
                }
                TempData["lstChildSupport"] = lstChildSupport;
                return Json(RenderViewToString("~/Views/PartialCategory/ListChild_1.cshtml"), JsonRequestBehavior.AllowGet);
            }
            else if (name.Contains("QuangCao"))
            {
                var lstAll = rep_AdvertCategory.GetAll().ToList();
                var lstChildAdvert = new List<advert_category>();
                if (id.ToString() == "")
                {
                    lstChildAdvert = null;
                }
                else
                {
                    var lstInt = Common.GetChild(lstAll, id).ToList();
                    lstChildAdvert = Common.CreateLevelOutID(lstAll.Where(g => lstInt.Contains(g.ID) && g.ID != id).ToList(), id);
                }
                TempData["lstChildAdvert"] = lstChildAdvert;
                return Json(RenderViewToString("~/Views/PartialCategory/ListChild_2.cshtml"), JsonRequestBehavior.AllowGet);
            }
            else if (name.Contains("Module"))
            {
                var tablename = code + "_category";
                var lstAll = rep_DocumentCategory.GetAllByTableName(tablename).ToList();
                var lstLevel = new List<document_category>();
                var lst = lstAll.Where(g => g.ParentID == 0).ToList();
                if (lst.Count() > 0 )
                {
                   
                    lstLevel = Common.CreateLevel(lstAll).ToList();
                }
                else
                {
                    lstLevel = lst.ToList();
                }
                
                TempData["lstChildContact"] = lstLevel;
                TempData["code"] = code;
                return Json(RenderViewToString("~/Views/PartialCategory/ListChild_3.cshtml"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("");
            }
        }
        [HttpPost]
        public ActionResult ListChildEdit(int id,string name, string code = "", int? idForm = 0,string support_1 = "",string advert_1="",string display_1="",string codeCurrent ="")
        {
            if (name.Contains("HoTroTrucTuyen"))
            {
                var lstAll = rep_SupportCategory.GetAll().ToList();
                var lstChildSupport = new List<support_category>();
                if (id.ToString() == "")
                {
                    lstChildSupport = null;
                }
                else
                {
                    var lstInt = Common.GetChild(lstAll, id).ToList();
                    lstChildSupport = Common.CreateLevelOutID(lstAll.Where(g => lstInt.Contains(g.ID) && g.ID != id).ToList(), id);
                }

                var getItemCategory = rep_DocumentCategory.GetAllByTableName(codeCurrent + "_category").FirstOrDefault(g => g.ID == idForm);
                TempData["lstChildSupport"] = lstChildSupport;
                TempData["getItemCategory"] = getItemCategory;
                return Json(RenderViewToString("~/Views/PartialCategory/ListChild_1.cshtml"), JsonRequestBehavior.AllowGet);
            }
            else if (name.Contains("QuangCao"))
            {
                var lstAll = rep_AdvertCategory.GetAll().ToList();
                var lstChildAdvert = new List<advert_category>();
                if (id.ToString() == "")
                {
                    lstChildAdvert = null;
                }
                else
                {
                    var lstInt = Common.GetChild(lstAll, id).ToList();
                    lstChildAdvert = Common.CreateLevelOutID(lstAll.Where(g => lstInt.Contains(g.ID) && g.ID != id).ToList(), id);
                }

                var getItemCategory = rep_DocumentCategory.GetAllByTableName(codeCurrent + "_category").FirstOrDefault(g => g.ID == idForm);
                TempData["lstChildAdvert"] = lstChildAdvert;
                TempData["getItemCategory"] = getItemCategory;
                return Json(RenderViewToString("~/Views/PartialCategory/ListChild_2.cshtml"), JsonRequestBehavior.AllowGet);
            }
            else if (name.Contains("Module"))
            {
                var tablename = code + "_category";
                var lstChildContact = rep_DocumentCategory.GetAllByTableName(tablename).ToList();
                var getItemCategory = rep_DocumentCategory.GetAllByTableName(codeCurrent + "_category").FirstOrDefault(g => g.ID == idForm);
                var lstLevel = new List<document_category>();
                var lst = lstChildContact.Where(g => g.ParentID == 0).ToList();
                if (lst.Count() > 0)
                {
                    lstLevel = Common.CreateLevel(lstChildContact).ToList();
                }
                else
                {
                    lstLevel = lst.ToList();
                }

                TempData["lstChildContact"] = lstLevel;
                TempData["getItemCategory"] = getItemCategory;
                TempData["code"] = code;
                return Json(RenderViewToString("~/Views/PartialCategory/ListChild_3.cshtml"), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("");
            }

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
            var lstDocumentCategory = Common.CreateLevel(rep_DocumentCategory.GetAll().ToList());
            TempData["DocumentCategory"] = lstDocumentCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(document_category obj, FormCollection frm)
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
                rep_DocumentCategory.Add(obj);
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
        public ActionResult Edit(int id,string support_1, string advert_1, string display_1)
        {
            var objDocumentCategory = rep_DocumentCategory.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstSupport = rep_SupportCategory.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstSupport"] = lstSupport;
            var lstAdvert = rep_AdvertCategory.GetAll().Where(g => g.ParentID == 0).ToList();
            TempData["lstAdvert"] = lstAdvert;
            var lstModule = rep_Module.GetAll().ToList();
            TempData["lstModule"] = lstModule;
            Session["view_more"] = objDocumentCategory.view_more_detail;
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
            var lstDocumentCategory = Common.CreateLevel(rep_DocumentCategory.GetAll().Where(g => g.ID != id).ToList());
            TempData["DocumentCategory"] = lstDocumentCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentCategory/_Edit.cshtml", objDocumentCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(document_category objDocCategory)
        {
            try
            {
                if (objDocCategory.ParentID == null)
                {
                    objDocCategory.ParentID = 0;
                }
                objDocCategory.CreatedBy = User.ID;
                objDocCategory.view_more_detail = Request["view_more_detail"];
                rep_DocumentCategory.Edit(objDocCategory);
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
                var obj = rep_DocumentCategory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_DocumentCategory.Edit(obj);

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
            var objDOcCategory = rep_DocumentCategory.Find(id);
            objDOcCategory.Status = !objDOcCategory.Status;
            rep_DocumentCategory.Edit(objDOcCategory);
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
                var lstDocumentCategory = rep_DocumentCategory.GetAll().ToList();
                var fullCatID = Common.GetChild(lstDocumentCategory, id).Distinct().ToList();

                if (fullCatID != null)
                {
                    foreach (var item in fullCatID)
                    {
                        rep_DocumentCategory.Delete(item);
                    }
                }
                rep_DocumentCategory.Delete(id);
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
                    var alldocumentCategory = rep_DocumentCategory.GetAll().ToList();
                    var fullCatID = Common.GetChild(alldocumentCategory, Convert.ToInt32(item)).Distinct();
                    if (fullCatID != null)
                    {
                        foreach (var item2 in fullCatID)
                        {
                            rep_DocumentCategory.Delete(item2);
                            count++;
                        }
                    }
                    rep_DocumentCategory.Delete(Convert.ToInt32(item));
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
            var lstData = rep_DocumentCategory.GetAll();
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
        public static string RemoveChar(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Trim();
                var charsToRemove = new string[] { ".", "," };

                foreach (var c in charsToRemove)
                {
                    str = str.Replace(c, string.Empty);
                }
            }
            return str;
        }

    }
}
