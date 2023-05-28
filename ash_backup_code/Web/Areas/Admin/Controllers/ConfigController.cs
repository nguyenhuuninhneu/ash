using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
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
    public class ConfigController : BaseController
    {
        //
        // GET: /Admin/Log/
        readonly  IConfigRepository configRepository = new ConfigRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(string LangCode = "vn", int page = 1)
        {
            var lstConfig = configRepository.GetAll().OrderBy(g=>g.Ordering).ToList();

            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var conf in lstConfig)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(conf.LangCode) && conf.LangCode == language.Code)
                        conf.NgonNgu = language.Name;
                }
            }
            lstConfig = lstConfig.Where(g => g.LangCode == LangCode).ToList();
            var totalLog = lstConfig.Count;
            lstConfig = lstConfig.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Config/_ListData.cshtml", lstConfig),
                totalPages = Math.Ceiling(((double)totalLog / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var objConfig = Common.CreateLevel(configRepository.GetAll().ToList());

            //ngôn ngữ
            var languages = "vn";
            if (Session["langCodeDefaut"] != null)
                languages = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.LangCode == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            TempData["Config"] = objConfig.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/Config/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_Config obj, string code)
        {
            try
            {
                var findbyname = configRepository.GetAll().Where(g=>g.Name==obj.Name).ToList();
                if (findbyname.Count>1)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên loại tin bài đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //Thêm ngôn ngữ mặc định
                    var langCode = "vn";
                    if (Session["langCodeDefaut"] != null)
                        langCode = Session["langCodeDefaut"].ToString();
                    obj.LangCode = langCode; 
                    obj.Code = code;

                    configRepository.Add(obj);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Thêm mới loại tin bài thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Thêm mới loại tin bài thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var getall = configRepository.GetAll().ToList();
            var objConfig=  configRepository.Find(id);
            //ngôn ngữ
            var languages = "vn";
            if (!string.IsNullOrEmpty(objConfig.LangCode))
                languages = objConfig.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.LangCode == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            var lstConfig = Common.CreateLevel(getall);
            TempData["Config"] = lstConfig.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/Config/_Edit.cshtml",objConfig), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Config obj, string code)
        {
            try
            {
                obj.Code = code;
                configRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật loại tin bài  thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Cập nhật loại tin bài  thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                configRepository.Delete(id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa loại tin bài thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa loại tin bài thành công",
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
                    Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} loại tin bài", count),
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
                var obj = configRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    configRepository.Edit(obj);
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
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = configRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            configRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
    }
    } 

