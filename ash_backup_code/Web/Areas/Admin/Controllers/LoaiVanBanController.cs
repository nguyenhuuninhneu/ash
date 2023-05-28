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
    public class LoaiVanBanController : BaseController
    {
        //
        // GET: /Admin/LoaiVanBan/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly ILoaiVanBanRepository _loaiVanBanRepository = new LoaiVanBanRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstCqbh = _loaiVanBanRepository.GetAll().OrderBy(g=>g.Ordering).ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                lstCqbh =
                    lstCqbh.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstCqbh = lstCqbh.Where(g => g.LangCode == LangCode).ToList();
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/LoaiVanBan/_ListData.cshtml", lstCqbh),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/LoaiVanBan/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_LoaiVanBan obj)
        {
            try
            {
                _loaiVanBanRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới loại văn bản: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới loại văn bản thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới loại văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var objLoaiVanBan = _loaiVanBanRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/LoaiVanBan/_Edit.cshtml", objLoaiVanBan), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_LoaiVanBan obj)
        {
            try
            {
                _loaiVanBanRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật loại văn bản: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật loại văn bản thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật loại văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _loaiVanBanRepository.Find(id);
                _loaiVanBanRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa loại văn bản: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa loại văn bản thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa loại văn bản thành công",
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
                    _loaiVanBanRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} loại văn bản", count),
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
                var obj = _loaiVanBanRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _loaiVanBanRepository.Edit(obj);

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
