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
    public class DocumentPriceController : BaseController
    {
        readonly IDocumentPrice_Repository rep_DocumentPrice = new DocumentPrice_Repository();
        readonly ILanguagesRepository rep_Languages = new LanguagesRepository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
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
        public ActionResult ListData(string Name, int? Status, string CreatedDate,string LangCode,string Price_min = "",string Price_max = "", int page = 1)
        {
            var lstDocumentPrice = rep_DocumentPrice.GetAll().OrderBy(g=>g.Ordering).ToList();
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            if (Status != null)
            {
                lstDocumentPrice = lstDocumentPrice.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstDocumentPrice = lstDocumentPrice.Where(g => HelperString.UnsignCharacter(g.LangCode).Contains(HelperString.UnsignCharacter(LangCode))).ToList();
                Session["LangCode"] = LangCode;
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstDocumentPrice = lstDocumentPrice.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }

            if (!string.IsNullOrEmpty(Price_min))
            {
                var giaMin = Convert.ToInt32(RemoveChar(Request["Price_min"]));
                lstDocumentPrice = lstDocumentPrice.Where(g => g.Price_min >= giaMin).ToList();

            }
            if (!string.IsNullOrEmpty(Price_max))
            {
                var giaMax = Convert.ToInt32(RemoveChar(Request["Price_max"]));
                lstDocumentPrice = lstDocumentPrice.Where(g => g.Price_max <= giaMax).ToList();
            }

            if (!string.IsNullOrEmpty(Price_min) && !string.IsNullOrEmpty(Price_max))
            {
                var giaMin = Convert.ToInt32(RemoveChar(Request["Price_min"]));
                var giaMax = Convert.ToInt32(RemoveChar(Request["Price_max"]));
                lstDocumentPrice = lstDocumentPrice.Where(g => g.Price_max <= giaMax && g.Price_min >= giaMin).ToList();
            }
            if (!string.IsNullOrEmpty(Price_min) && !string.IsNullOrEmpty(Price_max))
            {
                var giaMin = Convert.ToInt32(RemoveChar(Request["Price_min"]));
                var giaMax = Convert.ToInt32(RemoveChar(Request["Price_max"]));
               
                if (giaMax < giaMin)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = string.Format("Giá tối đa phải lớn hơn giá tối thiểu")
                    }, JsonRequestBehavior.AllowGet);
                }

            }

            //if (lstDocumentPrice.Count() == 0 )
            //{
            //    return Json(new
            //    {
            //        IsSuccess = true,
            //        Messenger = string.Format("Không có dữ liệu!")
            //    }, JsonRequestBehavior.AllowGet);
            //}
            var total = lstDocumentPrice.Count();
            TempData["total"] = total;
            var rowLimit = 10;
            lstDocumentPrice = lstDocumentPrice.OrderByDescending(g => g.ID).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DocumentPrice/_ListData.cshtml", lstDocumentPrice),
                
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]

        public ActionResult Add()
        {
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentPrice/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]

        [HttpPost]
        public ActionResult Add(document_price obj)
        {

            try
            {

                var priceMin = Convert.ToInt32(RemoveChar(Request["Price_min"]));
                var priceMax = Convert.ToInt32(RemoveChar(Request["Price_max"]));
                if (priceMin >= priceMax)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Giá tối đa phải lớn hơn giá tối thiểu!")
                    }, JsonRequestBehavior.AllowGet);
                }
                obj.Price_min = priceMin;
                obj.Price_max = priceMax;
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = User.ID;
                rep_DocumentPrice.Add(obj);
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
            var objDocumentPrice = rep_DocumentPrice.Find(id);
            var lstLanguage = rep_Languages.GetAll().ToList();
            TempData["languages"] = lstLanguage;
            return Json(RenderViewToString("~/Areas/Admin/Views/DocumentPrice/_Edit.cshtml", objDocumentPrice), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(document_price objDocPrice)
        {
            try
            {
                var priceMin = Convert.ToInt32(RemoveChar(Request["Price_min"]));
                var priceMax = Convert.ToInt32(RemoveChar(Request["Price_max"]));
                if (priceMin >= priceMax)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Giá tối đa phải lớn hơn giá tối thiểu!")
                    }, JsonRequestBehavior.AllowGet);
                }
                objDocPrice.Price_min = priceMin;
                objDocPrice.Price_max = priceMax;
                objDocPrice.CreatedBy = User.ID;
                rep_DocumentPrice.Edit(objDocPrice);
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
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = rep_DocumentPrice.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_DocumentPrice.Edit(obj);

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
            var objDOcPrice = rep_DocumentPrice.Find(id);
            objDOcPrice.Status = !objDOcPrice.Status;
            rep_DocumentPrice.Edit(objDOcPrice);
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
                rep_DocumentPrice.Delete(id);
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
                    rep_DocumentPrice.Delete(Convert.ToInt32(item));
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


        public ActionResult CheckTrung(string id,string name = "")
        {
            var trungTen = false;
            var lstData = rep_DocumentPrice.GetAll();
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
