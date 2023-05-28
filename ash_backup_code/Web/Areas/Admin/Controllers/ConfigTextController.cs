using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class ConfigTextController : BaseController
    {
        //
        // GET: /Admin/Log/
        readonly static IConfigTextRepository ConfigTextRepository = new ConfigTextRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(string LangCode = "vn", int page = 1)
        {
            //  var objConfigText = ConfigTextRepository.GetAll().OrderByDescending(g => g.ID).FirstOrDefault(g => g.LangCode.Trim() == "vn");
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();

            var lstConfigText = ConfigTextRepository.GetAll().OrderByDescending(g => g.ID).ToList();
            var lstDetails = new List<tbl_ConfigTextField>();
            foreach (var item in lstConfigText)
            {
                if (item != null && item.ContentJson != null)
                {
                    var contenLogs = JsonConvert.DeserializeObject<List<tbl_ConfigTextField>>(item.ContentJson);//HelperXml.DeserializeXml2List<tbl_ConfigTextField>(item.Contents);
                    if (LangCode == "vn")
                    {
                        foreach (var content in contenLogs.Where(g=>g.LangCode==null||g.LangCode==LangCode).ToList())
                        {
                            lstDetails.Add(content);
                        }
                    }
                    else
                    {
                        foreach (var content in contenLogs.Where(g => g.LangCode != null&& g.LangCode == LangCode).ToList())
                        {
                            lstDetails.Add(content);
                        }
                    }

                    //lstDetails.AddRange(contenLogs);
                }
            }
            //if (objConfigText != null && objConfigText.Contents != null)
            //{
            //    var contenLogs = HelperXml.DeserializeXml2List<tbl_ConfigTextField>(objConfigText.Contents);
            //    lstDetails.AddRange(contenLogs);
            //}
            LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var detail in lstDetails)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(detail.LangCode) && detail.LangCode == language.Code)
                        detail.NgonNgu = language.Name;
                }
            }

            var totalLog = lstDetails.Count;
            lstDetails = lstDetails.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/ConfigText/_ListData.cshtml", lstDetails),
                totalPages = Math.Ceiling(((double)totalLog / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //ngôn ngữ
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;


            return Json(RenderViewToString("~/Areas/Admin/Views/ConfigText/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_ConfigTextField obj)
        {
            try
            {
                obj.ID = Guid.NewGuid();
                obj.ActionBy = User.Username;
                obj.ActionTime = DateTime.Now;
                //Thêm ngôn ngữ mặc định
                var language = "vn";
                if (Session["langCodeDefaut"] != null)
                    language = Session["langCodeDefaut"].ToString();
                obj.LangCode = language;
                var LangCode = obj.LangCode;
                var objConfigText = ConfigTextRepository.GetAll().FirstOrDefault(g => g.LangCode.Trim() == LangCode.Trim());
                var lstConfigTextField = new List<tbl_ConfigTextField>();
                if (objConfigText != null)
                {
                    //lstConfigTextField = HelperXml.DeserializeXml2List<tbl_ConfigTextField>(objConfigText.Contents);
                    lstConfigTextField = JsonConvert.DeserializeObject<List<tbl_ConfigTextField>>(objConfigText.ContentJson);
                    var isExist = lstConfigTextField.Count(g => g.Field.Trim() == obj.Field.Trim());
                    if (isExist == 0)
                    {
                        lstConfigTextField.Add(obj);
                        var xmlContentLogs = JsonConvert.SerializeObject(lstConfigTextField);//HelperXml.SerializeList2Xml(lstConfigTextField);
                        objConfigText.ContentJson = xmlContentLogs;
                        ConfigTextRepository.Edit(objConfigText);
                    }
                    else
                    {
                        return Json(new
                        {
                            IsSuccess = false,
                            Messenger = "ConfigText đã tồn tại",
                        }, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {
                    lstConfigTextField = new List<tbl_ConfigTextField> { obj };
                    objConfigText = new tbl_ConfigText
                    {
                        ContentJson = JsonConvert.SerializeObject(lstConfigTextField),//HelperXml.SerializeList2Xml(lstConfigTextField),
                        CreatedDate = DateTime.Now.Date,
                        CreatedBy = User.ID,
                        LastModifieDate = DateTime.Now,
                        LastModifieBy = User.ID,
                        LangCode = LangCode
                    };
                    ConfigTextRepository.Add(objConfigText);
                }
                LogController.AddLog("Thêm mới ConfigText", User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới ConfigText thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Thêm mới ConfigText thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(string id)
        {
            var objConfigTextField = new tbl_ConfigTextField();
            var lstConfigText = ConfigTextRepository.GetAll().ToList();
            foreach (var item in lstConfigText)
            {
                if (item != null)
                {
                    var lstConfigTextField = JsonConvert.DeserializeObject<List<tbl_ConfigTextField>>(item.ContentJson);//HelperXml.DeserializeXml2List<tbl_ConfigTextField>(item.Contents);
                    if (lstConfigTextField != null)
                    {
                        foreach (var item2 in lstConfigTextField)
                        {
                            if (item2.ID.ToString() == id)
                                objConfigTextField = item2;
                        }
                    }
                }
            }
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objConfigTextField.LangCode))
                langCode = objConfigTextField.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            return Json(RenderViewToString("~/Areas/Admin/Views/ConfigText/_Edit.cshtml", objConfigTextField), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_ConfigTextField obj)
        {
            try
            {
                var LangCode = obj.LangCode ?? "vn";
                var objConfigText = ConfigTextRepository.GetAll().FirstOrDefault(g => g.LangCode.Trim() == LangCode.Trim());
                var lstConfigTextField = JsonConvert.DeserializeObject<List<tbl_ConfigTextField>>(objConfigText.ContentJson);//HelperXml.DeserializeXml2List<tbl_ConfigTextField>(objConfigText.Contents);
                if (lstConfigTextField != null)
                {
                    foreach (var item in lstConfigTextField)
                    {
                        if (item.ID == obj.ID)
                        {
                            item.Field = obj.Field;
                            item.Value = obj.Value;
                            item.Note = obj.Note;
                            item.ActionBy = User.Username;
                            item.ActionTime = DateTime.Now;
                            item.LangCode = obj.LangCode;
                        }
                    }
                    var xmlContentLogs = JsonConvert.SerializeObject(lstConfigTextField);//HelperXml.SerializeList2Xml(lstConfigTextField);

                    objConfigText.ContentJson = xmlContentLogs;
                    ConfigTextRepository.Edit(objConfigText);
                    LogController.AddLog("Cập nhật ConfigText", User.Username);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Cập nhật ConfigText thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật ConfigText thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Cập nhật ConfigText thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(string id)
        {
            try
            {
                var LangCode = "vn";
                // var objConfigText = ConfigTextRepository.GetAll().FirstOrDefault(g => g.LangCode.Trim() == LangCode.Trim());
                var lstConfigText = ConfigTextRepository.GetAll().ToList();
                foreach (var item in lstConfigText)
                {
                    var lstConfigTextField = JsonConvert.DeserializeObject<List<tbl_ConfigTextField>>(item.ContentJson);//HelperXml.DeserializeXml2List<tbl_ConfigTextField>(item.Contents);
                    int index = lstConfigTextField.FindIndex(g => g.ID.ToString() == id);
                    if (index >= 0)
                    {
                        lstConfigTextField.RemoveAt(index);
                    }
                    var xmlContentLogs = JsonConvert.SerializeObject(lstConfigTextField);//HelperXml.SerializeList2Xml(lstConfigTextField);
                    item.ContentJson = xmlContentLogs;
                    ConfigTextRepository.Edit(item);
                }
                //var lstConfigTextField = HelperXml.DeserializeXml2List<tbl_ConfigTextField>(objConfigText.Contents);
                //int index = lstConfigTextField.FindIndex(g => g.ID.ToString() == id);
                //lstConfigTextField.RemoveAt(index);
                //var xmlContentLogs = HelperXml.SerializeList2Xml(lstConfigTextField);
                //objConfigText.Contents = xmlContentLogs;
                //ConfigTextRepository.Edit(objConfigText);
                LogController.AddLog("Xóa ConfigText", User.Username);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa ConfigText thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa ConfigText thành công",
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
                    Delete(item);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} ConfigText", count),
            }, JsonRequestBehavior.AllowGet);
        }

        public static string GetValueCFT(string Field,string Value = "")
        {
            string TextValue = "";
            var LangCode = "vn";
            if (System.Web.HttpContext.Current.Session["langCodeHome"] != null)
                LangCode = System.Web.HttpContext.Current.Session["langCodeHome"].ToString();
             
            try
            {
                var obj = new tbl_ConfigTextField();
                obj.Field = Field;
                obj.Value = Value;
                obj.ID = Guid.NewGuid(); 
                obj.ActionTime = DateTime.Now; 
                obj.LangCode = LangCode;  

                var objConfigText = ConfigTextRepository.GetAll().FirstOrDefault(g => g.LangCode.Trim() == LangCode.Trim());
                var lstConfigTextField = new List<tbl_ConfigTextField>();
                if (objConfigText != null)
                { 
                    lstConfigTextField = JsonConvert.DeserializeObject<List<tbl_ConfigTextField>>(objConfigText.ContentJson);
                    var isExist = lstConfigTextField.Count(g => g.Field.Trim() == obj.Field.Trim());
                    if (isExist == 0)
                    {
                        lstConfigTextField.Add(obj);
                        var xmlContentLogs = JsonConvert.SerializeObject(lstConfigTextField);
                        objConfigText.ContentJson = xmlContentLogs;
                        ConfigTextRepository.Edit(objConfigText);
                        TextValue = obj.Value;
                    }
                    else
                    {
                        foreach (var item in lstConfigTextField)
                        {
                            if (item.Field.Trim() == Field.Trim())
                            {
                                TextValue = item.Value;
                            }
                        }
                    }
                }
                else
                {
                    lstConfigTextField = new List<tbl_ConfigTextField> { obj };
                    objConfigText = new tbl_ConfigText
                    {
                        ContentJson = JsonConvert.SerializeObject(lstConfigTextField),
                        CreatedDate = DateTime.Now.Date, 
                        LastModifieDate = DateTime.Now, 
                        LangCode = LangCode
                    };
                    ConfigTextRepository.Add(objConfigText);
                } 
            }
            catch (Exception)
            {} 
             
            return TextValue;
        }
    }
}
