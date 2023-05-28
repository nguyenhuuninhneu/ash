using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
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
    public class PositionController : BaseController
    {
        
        readonly IPositionRepository _positionRepository = new PositionRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            var list = _positionRepository.GetAll().Where(x=>x.ParentID==0&&x.LangCode==langCode).ToList();
            return View(list);
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name,string category, string LangCode, int page = 1)
        {
            var lstPosition = _positionRepository.GetAll();

            LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var video in lstPosition)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(video.LangCode) && video.LangCode == language.Code)
                        video.NgonNgu = language.Name;
                }
            }
            lstPosition = lstPosition.Where(g => g.LangCode == LangCode).ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                lstPosition = lstPosition.Where(g => g.Name.Trim().Contains(Name)).ToList();
            }
            if (!string.IsNullOrEmpty(category))
            {
                lstPosition = _positionRepository.GetListPosByCode(category);
            }
            var lstLevel = Common.CreateLevel(lstPosition);
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Position/_ListData.cshtml", lstLevel),
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

            var lst= _positionRepository.GetAll().ToList();
            var lstPost = Common.CreateLevel(lst);
            TempData["Position"] = lstPost;

            return Json(RenderViewToString("~/Areas/Admin/Views/Position/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(tbl_Position obj)
        {
            try
            {
                var lstPosition = _positionRepository.GetAll().ToList();
                var checkName = lstPosition.Where(x => x.Name == obj.Name).ToList();
                if (checkName.Count >= 1)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Tên vị trí đã tồn tại")
                    }, JsonRequestBehavior.AllowGet);
                }
                var checkCode=lstPosition.Where(x => x.Code == obj.Code).ToList();
                if (checkCode.Count >= 1)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Mã vị trí đã tồn tại")
                    }, JsonRequestBehavior.AllowGet);
                }
                //List<string> positionCode = new List<string>();
                //foreach (var item in lstPosition)
                //{
                //    positionCode.Add(item.Code);
                //}
                //if (positionCode.Contains(obj.Code))
                //{
                //    return Json(new
                //    {
                //        IsSuccess = false,
                //        Messenger = string.Format("Mã vị trí đã tồn tại")
                //    }, JsonRequestBehavior.AllowGet);
                //}
                obj.Code = HelperString.UnsignCharacter(obj.Code.Trim().ToLower());
                obj.CreatedDate = DateTime.Now;
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                _positionRepository.Add(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới vị trí thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới vị trí thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            

            var lst  = Common.CreateLevel(_positionRepository.GetAll().Where(g=>g.ID!=id).ToList());
            TempData["Position"] = lst.ToList();
            var objFooter = _positionRepository.Find(id);
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objFooter.LangCode))
                langCode = objFooter.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;
            return Json(RenderViewToString("~/Areas/Admin/Views/Position/_Edit.cshtml", objFooter), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(tbl_Position obj)
        {
            try
            {
                var objPos = _positionRepository.Find(obj.ID);
                if (obj.Name.Contains("--"))
                {
                    obj.Name = obj.Name.Substring(2);
                }
                obj.CreatedDate = objPos.CreatedDate;
                 _positionRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _positionRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _positionRepository.Edit(obj);
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
                var obj = _positionRepository.Find(id);
                _positionRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa thông tin trang: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa vị trí thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa vị trí thành công",
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
                    _positionRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} vị trí", count),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = _positionRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _positionRepository.Edit(obj);
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
