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
    public class ModuleCateController : BaseController
    {
        readonly IModuleCateRepository _categoryRepository = new ModuleCateRepository(); 
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
     
        //
        // GET: /ModuleCate/
        [Authorize(Roles = "Index")]
        public ActionResult Index(int page = 1, string Name = "")
        {
            var code = "";
            if (!string.IsNullOrEmpty(Request["Code"]))
            {
                code = Request["Code"];
            }
            TempData["code"] = code;

            var lstLevel = new List<tbl_ModuleCate>();
            var LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            var lstModuleCate = _categoryRepository.GetAll().Where(g => g.LangCode.Trim() == LangCode.Trim() && g.Code == code).OrderBy(g => g.Ordering).ToList();

            if (!string.IsNullOrEmpty(Name))
            {
                lstModuleCate =
                    lstModuleCate.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();

                var lst = lstModuleCate.Where(x => x.ParentID == 0).ToList();
                if (lst.Count > 0)
                    lstLevel = Common.CreateLevel(lstModuleCate.ToList());
                else
                    lstLevel = lstModuleCate;
            }
            else
            {
                lstLevel = Common.CreateLevel(lstModuleCate.ToList());
            }


            var totalModuleCate = lstModuleCate.Count;
            TempData["totalCate"] = totalModuleCate;
            lstLevel = lstLevel.Skip((page - 1) * 20).Take(20).ToList();

            var totalPages = Math.Ceiling(((double) totalModuleCate / 20));
            TempData["totalPages"] = totalPages;

            return View(lstLevel);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add(string code)
        {
            //ngôn ngữ
            var languages = "vn";
            if (Session["langCodeDefaut"] != null)
                languages = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;
 
            var lstModuleCate = Common.CreateLevel(_categoryRepository.GetAll().Where(g=>g.LangCode.Trim() == languages.Trim() && g.Code == code).OrderBy(g=>g.Ordering).ToList());
            TempData["ModuleCate"] = lstModuleCate.ToList();
            TempData["code"] = code;

            var objLast = _categoryRepository.GetAll().OrderByDescending(g => g.Ordering).FirstOrDefault();
            TempData["objLast"] = objLast;

            return Json(RenderViewToString("~/Areas/Admin/Views/ModuleCate/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_ModuleCate obj)
        {
            try
            {
                var findObj = _categoryRepository.GetAll().Where(x => x.Name == obj.Name).ToList();
                if (findObj.Count >= 1)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Tên danh mục đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                } 
                obj.CreateDate = DateTime.Now;
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();
                obj.CreateBy = User.ID;

                _categoryRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới danh mục: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        { 
            //ngôn ngữ
            var languages = "vn"; 
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;
            var obj = _categoryRepository.Find(id);
            var code = obj.Code;
            var lstModuleCate = Common.CreateLevel(_categoryRepository.GetAll().Where(g => g.LangCode == languages && g.ID != id && g.Code == code).ToList());
            TempData["ModuleCate"] = lstModuleCate.ToList(); 
            return Json(RenderViewToString("~/Areas/Admin/Views/ModuleCate/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_ModuleCate obj)
        {
            try
            { 
                _categoryRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật danh mục: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _categoryRepository.Find(id);
                _categoryRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa danh mục: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục thành công",
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
                    _categoryRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục", count),
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
                var obj = _categoryRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _categoryRepository.Edit(obj);

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
      
        readonly List<int> _lstDic = new List<int>();
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _categoryRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _categoryRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        
    }
}
