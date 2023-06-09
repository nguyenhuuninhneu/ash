﻿using System;
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
    public class DocumentPublisherController : BaseController
    {
        readonly IDocumentPublisher_Repository rep_DocumentPublisher = new DocumentPublisher_Repository();
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
        public ActionResult ListData(string Name, int? Status, string CreatedDate, string LangCode,string KyHieu,string MaSo, int page = 1)
        {
            var allDocumentPublisher = rep_DocumentPublisher.GetAll();
            var lstDocumentPublisher = allDocumentPublisher.Where(g=>g.LangCode == LangCode).OrderBy(g => g.Ordering).ToList();
            
            Session["LangCode"] = LangCode;
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            
            foreach (var cat in lstDocumentPublisher)
            {
                AddParent(cat, lstDocumentPublisher, allDocumentPublisher);
                var lstChild = Common.GetChild(allDocumentPublisher, cat.ID);
                foreach (var item in lstChild.Where(p => p != cat.ID))
                {
                    _lstOtherDocumentPublisher.Add(rep_DocumentPublisher.Find(item));
                }
                _lstOtherDocumentPublisher.Add(cat);
            }
            lstDocumentPublisher.AddRange(_lstOtherDocumentPublisher);
            foreach (var item in lstDocumentPublisher)
            {
                var objParent = lstDocumentPublisher.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            if (Status != null)
            {
                lstDocumentPublisher = lstDocumentPublisher.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstDocumentPublisher = lstDocumentPublisher.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }
            if (!string.IsNullOrEmpty(KyHieu))
            {
                lstDocumentPublisher = lstDocumentPublisher.Where(g => HelperString.UnsignCharacter(g.KyHieu).Contains(HelperString.UnsignCharacter(KyHieu))).ToList();
            }
            if (!string.IsNullOrEmpty(MaSo))
            {
                lstDocumentPublisher = lstDocumentPublisher.Where(g => HelperString.UnsignCharacter(g.MaSo).Contains(HelperString.UnsignCharacter(MaSo))).ToList();
            }
            var lstLevel = Common.CreateLevel(lstDocumentPublisher.ToList()).Distinct();
            var total = lstLevel.Count();
            TempData["total"] = total;
            lstLevel = lstLevel.OrderByDescending(g => g.ID).ToList();
         
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DocumentPublisher/_ListData.cshtml", lstLevel)
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<document_publisher> _lstOtherDocumentPublisher = new List<document_publisher>();
        readonly List<int> _listDic = new List<int>();
        //Tìm cha
        public void AddParent(document_publisher cat, List<document_publisher> lstOtherDocumentPublisher, List<document_publisher> lstAllDocumentPublisher)
        {
            if (_listDic.Contains(cat.ID) || cat.ParentID == 0)
            {
                return;
            }
            _listDic.Add(cat.ID);
            var parrent = lstOtherDocumentPublisher.Find(g => g.ID == cat.ParentID);
            if (parrent != null)
            {
                return;
            }
            parrent = lstAllDocumentPublisher.Find(g => g.ID == cat.ParentID);
            _lstOtherDocumentPublisher.Add(parrent);
            AddParent(parrent, lstOtherDocumentPublisher, lstAllDocumentPublisher);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstDocumentPublisher = Common.CreateLevel(rep_DocumentPublisher.GetAll().ToList());
            TempData["DocumentPublisher"] = lstDocumentPublisher.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentPublisher/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]

        [HttpPost]
        public ActionResult Add(document_publisher obj)
        {

            try
            {

                if (obj.ParentID == null)
                {
                    obj.ParentID = 0;
                }
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = User.ID;
                rep_DocumentPublisher.Add(obj);
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
            var objDocumentPublisher = rep_DocumentPublisher.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            var lstDocumentPublisher = Common.CreateLevel(rep_DocumentPublisher.GetAll().Where(g=> g.ID != id).ToList());
            TempData["DocumentPublisher"] = lstDocumentPublisher.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentPublisher/_Edit.cshtml", objDocumentPublisher), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(document_publisher objDocPublisher)
        {
            try
            {
                if (objDocPublisher.ParentID == null)
                {
                    objDocPublisher.ParentID = 0;
                }
                objDocPublisher.CreatedBy = User.ID;
                rep_DocumentPublisher.Edit(objDocPublisher);
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
                var obj = rep_DocumentPublisher.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_DocumentPublisher.Edit(obj);

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
            var objDOcPublisher = rep_DocumentPublisher.Find(id);
            objDOcPublisher.Status = !objDOcPublisher.Status;
            rep_DocumentPublisher.Edit(objDOcPublisher);
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
                var lstDocumentPublisher = rep_DocumentPublisher.GetAll().ToList();
                var fullCatID = Common.GetChild(lstDocumentPublisher, id).Distinct().ToList();

                if (fullCatID != null)
                {
                    foreach (var item in fullCatID)
                    {
                        rep_DocumentPublisher.Delete(item);
                    }
                }
                rep_DocumentPublisher.Delete(id);
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
                    var alldocumentPublisher = rep_DocumentPublisher.GetAll().ToList();
                    var fullCatID = Common.GetChild(alldocumentPublisher, Convert.ToInt32(item)).Distinct();
                    if (fullCatID != null)
                    {
                        foreach (var item2 in fullCatID)
                        {
                            rep_DocumentPublisher.Delete(item2);
                            count++;
                        }
                    }
                    rep_DocumentPublisher.Delete(Convert.ToInt32(item));
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
            var lstData = rep_DocumentPublisher.GetAll();
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
