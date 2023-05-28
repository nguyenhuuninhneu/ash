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
    public class DonViTTHCController : BaseController
    {
        readonly IDonViTTHCRepository _donvitthcRepository = new DonViTTHCRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();        
        

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {                       
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages;
            
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {   
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();           
            var lstDonViTTHC = _donvitthcRepository.GetAll();

            foreach (var item in lstDonViTTHC)
            {
                var objParent = lstDonViTTHC.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }

            var lstDonViTTHCOrder = new List<tbl_DonViTTHC>();
            //sắp xếp lại danh mục
            foreach (var item in lstDonViTTHC.Where(g => g.ParentID == 0).OrderBy(g => g.Ordering))
            {
                FindChild(item, ref lstDonViTTHCOrder, lstDonViTTHC);
            }

            if (!string.IsNullOrEmpty(Name))
            {
                lstDonViTTHCOrder =
                    lstDonViTTHCOrder.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower())
                                .Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstDonViTTHCOrder = lstDonViTTHCOrder.Where(g => g.LangCode == LangCode).ToList();
            }
            
            var totalDonViTTHC = lstDonViTTHCOrder.Count;
            lstDonViTTHCOrder = lstDonViTTHCOrder.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DonViTTHC/_ListData.cshtml", lstDonViTTHCOrder)
            }, JsonRequestBehavior.AllowGet);
        }

        public void FindChild(tbl_DonViTTHC donViParent, ref List<tbl_DonViTTHC> lstDonViOrder, List<tbl_DonViTTHC> lstfull)
        {
            var lstChild = lstfull.Where(g => g.ParentID == donViParent.ID).ToList();
            lstDonViOrder.Add(donViParent);
            if (!lstChild.Any()) return;
            foreach (var item in lstChild)
            {
                FindChild(item, ref lstDonViOrder, lstfull);
            }
        }

        public ActionResult GetAllByLangCode(string LangCode)
        {
            var lstDonViTTHC = _donvitthcRepository.GetAll();            
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstDonViTTHC = lstDonViTTHC.Where(g => g.LangCode == LangCode).ToList();
            }

            var lstlstDonViTTHCLevel = Common.CreateLevel(lstDonViTTHC);
            return Json(lstlstDonViTTHCLevel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCatbyLang(string langcode)
        {
            var lang = Request.QueryString["lang"] ?? Webconfig.LangCodeVn;
            var lstDonViTTHC = _donvitthcRepository.GetAll();
            if (!string.IsNullOrEmpty(lang))
            {
                lstDonViTTHC = lstDonViTTHC.Where(g => g.LangCode == lang).ToList();
            }
            var lstLevel = Common.CreateLevel(lstDonViTTHC.ToList());
            return Json(lstLevel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {            
            var lstDonViTTHC = Common.CreateLevel(_donvitthcRepository.GetAll());
            TempData["DonViTTHC"] = lstDonViTTHC.ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();            
            return Json(RenderViewToString("~/Areas/Admin/Views/DonViTTHC/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_DonViTTHC obj)
        {
            try
            {
                _donvitthcRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới đơn vị TTHC: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới đơn vị TTHC thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới đơn vị TTHC thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {           
            var objDonViTTHC = _donvitthcRepository.Find(id);
            /*var allDonViTTHC = _donvitthcRepository.GetAll();
            var allParentID = Common.GetAllParent(allDonViTTHC, id);*/
            var lstDonViTTHC = Common.CreateLevel(_donvitthcRepository.GetAll());
            lstDonViTTHC = lstDonViTTHC.Where(g => g.ID != id).ToList();
            TempData["DonViTTHC"] = lstDonViTTHC.ToList();
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();            
            return Json(RenderViewToString("~/Areas/Admin/Views/DonViTTHC/_Edit.cshtml", objDonViTTHC), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_DonViTTHC obj)
        {
            try
            {
                _donvitthcRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật đơn vị TTHC: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật đơn vị TTHC thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật đơn vị TTHC thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _donvitthcRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _donvitthcRepository.Edit(obj);
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
                var obj = _donvitthcRepository.Find(id);
                _donvitthcRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa đơn vị TTHC: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa đơn vị TTHC thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa đơn vị TTHC thành công",
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
                    _donvitthcRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} đơn vị TTHC", count),
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
                var obj = _donvitthcRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _donvitthcRepository.Edit(obj);

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