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
    public class DocumentTypeController : BaseController
    {
        readonly IDocumentType_Repository rep_DocumentType = new DocumentType_Repository();
        readonly ILanguagesRepository rep_Languages = new LanguagesRepository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
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
            var allDocumentType = rep_DocumentType.GetAll().Where(g => g.LangCode.Trim().ToLower() == LangCode.Trim().ToLower()).OrderBy(g => g.Ordering).ToList();

             Session["LangCode"] = LangCode;
            //Lấy tên ngôn ngữ
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var objLangCode = new  tbl_Languages();
            foreach (var item in allDocumentType)
            {
                objLangCode = new tbl_Languages();
                objLangCode = lstLanguage.FirstOrDefault(g => g.LangCode.Trim().ToLower() == item.LangCode.Trim().ToLower());
                if (objLangCode != null)
                    item.NgonNgu = objLangCode.Name;
            }
            var lstLevel = Common.CreateLevel(allDocumentType.ToList());
            var total = lstLevel.Count();
            TempData["total"] = total;
            var rowLimit = 10;
            lstLevel = lstLevel.Skip((page - 1) * Webconfig.RowLimit).Take(rowLimit).OrderByDescending(g => g.ID).ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DocumentType/_ListData.cshtml", lstLevel),
                totalPages = Math.Ceiling(((double)total / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<tbl_Document_Type> _lstOtherDocumentType = new List<tbl_Document_Type>();
        readonly List<int> _listDic = new List<int>();
        //Tìm cha
        public void AddParent(tbl_Document_Type cat, List<tbl_Document_Type> lstOtherDocumentType, List<tbl_Document_Type> lstallDocumentType)
        {
            if (_listDic.Contains(cat.ID) || cat.ParentID == 0)
            {
                return;
            }
            _listDic.Add(cat.ID);
            var parrent = lstOtherDocumentType.Find(g => g.ID == cat.ParentID);
            if (parrent != null)
            {
                return;
            }
            parrent = lstallDocumentType.Find(g => g.ID == cat.ParentID);
            _lstOtherDocumentType.Add(parrent);
            AddParent(parrent, lstOtherDocumentType, lstallDocumentType);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstDocumentType = Common.CreateLevel(rep_DocumentType.GetAll().ToList());
            TempData["DocumentType"] = lstDocumentType.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentType/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]

        [HttpPost]
        public ActionResult Add(tbl_Document_Type obj)
        {

            try
            {

                if (obj.ParentID == null)
                {
                    obj.ParentID = 0;
                }
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = User.ID;
                rep_DocumentType.Add(obj);
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
            var objDocumentType = rep_DocumentType.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstDocumentType = Common.CreateLevel(rep_DocumentType.GetAll().Where(g => g.ID != id).ToList());
            TempData["DocumentType"] = lstDocumentType.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentType/_Edit.cshtml", objDocumentType), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Document_Type objDocType)
        {
            try
            {
                if (objDocType.ParentID == null)
                {
                    objDocType.ParentID = 0;
                }
                objDocType.CreatedBy = User.ID;
                rep_DocumentType.Edit(objDocType);
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
                var obj = rep_DocumentType.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_DocumentType.Edit(obj);

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
            var objDOcType = rep_DocumentType.Find(id);
            objDOcType.Status = !objDOcType.Status;
            rep_DocumentType.Edit(objDOcType);
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
                var lstDocumentType = rep_DocumentType.GetAll().ToList();
                var fullCatID = Common.GetChild(lstDocumentType, id).Distinct().ToList();

                if (fullCatID != null)
                {
                    foreach (var item in fullCatID)
                    {
                        rep_DocumentType.Delete(item);
                    }
                }
                rep_DocumentType.Delete(id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa thành công",
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
                    var allDocumentType = rep_DocumentType.GetAll().ToList();
                    var fullCatID = Common.GetChild(allDocumentType, Convert.ToInt32(item)).Distinct();
                    if (fullCatID != null)
                    {
                        foreach (var item2 in fullCatID)
                        {
                            rep_DocumentType.Delete(item2);
                            count++;
                        }
                    }
                    rep_DocumentType.Delete(Convert.ToInt32(item));
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


        public ActionResult CheckTrung(string id, string name = "")
        {
            var trungTen = false;
            var lstData = rep_DocumentType.GetAll();
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
