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
    public class ModuleController : BaseController
    {
        readonly IModule_Repository rep_Module = new Module_Repository();
        readonly IAdminMenuRepository rep_AdminMenu = new AdminMenuRepository();
        //
        // GET: /Admin/Customer/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();

            var nameOfController = rep_AdminMenu.GetAll().FirstOrDefault(g => g.Url.Contains(controllerName)).Name;

            TempData["controllerName"] = nameOfController;
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int? Status, string CreatedDate, int page = 1)
        {
            var lstModule = rep_Module.GetAll().OrderBy(g => g.Ordering).ToList();

            if (Status != null)
            {
                lstModule = lstModule.Where(g => g.Status == Convert.ToBoolean(Status)).ToList();
            }

            if (!string.IsNullOrEmpty(Name))
            {
                lstModule = lstModule.Where(g => HelperString.UnsignCharacter(g.Name).Contains(HelperString.UnsignCharacter(Name))).ToList();
            }

            
            var total = lstModule.Count();
            TempData["total"] = total;
            var rowLimit = 10;
            lstModule = lstModule.Skip((page - 1) * rowLimit).Take(rowLimit).OrderByDescending(g => g.ID).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Module/_ListData.cshtml", lstModule),
                totalPages = Math.Ceiling(((double)total / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]

        public ActionResult Add()
        {

            return Json(RenderViewToString("~/Areas/Admin/Views/Module/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]

        [HttpPost]
        public ActionResult Add(module obj)
        {

            try
            {

                
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = User.ID;
                rep_Module.Add(obj);
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
            var objModule = rep_Module.Find(id);

            return Json(RenderViewToString("~/Areas/Admin/Views/Module/_Edit.cshtml", objModule), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(module objModule)
        {
            try
            {
               
                objModule.CreatedBy = User.ID;
                rep_Module.Edit(objModule);
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
                var obj = rep_Module.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    rep_Module.Edit(obj);

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
            var objModule = rep_Module.Find(id);
            objModule.Status = !objModule.Status;
            rep_Module.Edit(objModule);
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
                rep_Module.Delete(id);
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
                    rep_Module.Delete(Convert.ToInt32(item));
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
            var lstData = rep_Module.GetAll();
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

        public ActionResult CheckVietLienKhongDau(string id, string code = "")
        {
            var Check = false;
            if (HelperString.CheckVietLienKhongDau(code)== false)
            {
                Check = true;
            }
            else
            {
                Check = false;
            }
            //Form thêm mới
            return Json(new
            {
                Check = Check
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckTrungCode(string id, string code = "")
        {
          var trungMa = false;
            var lstData = rep_Module.GetAll();
            if (id == "")
            {
                lstData = lstData.Where(g => g.Code == code).ToList();
            }
            else
            {
                lstData = lstData.Where(g => g.Code == code && g.ID != Convert.ToInt32(id)).ToList();
            }

            if (lstData.Count > 0)
            {
                trungMa = true;
            }
            else
            {
                trungMa = false;
            }
            //Form thêm mới
            return Json(new
            {
                trungMa = trungMa
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
