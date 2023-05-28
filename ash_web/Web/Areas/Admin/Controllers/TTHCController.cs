using System;
using System.Collections.Generic;
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
    public class TTHCController : BaseController
    {
        //
        // GET: /Admin/TTHC/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly ILinhVucTTHCRepository _linhVucTTHCRepository = new LinhVucTTHCRepository();
        readonly IDonViTTHCRepository _donViTTHCRepository = new DonViTTHCRepository();
        readonly ITTHCRepository _TTHCRepository = new TTHCRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Tieude, string LangCode, int page = 1)
        {

            var lstLinhVuc = _linhVucTTHCRepository.GetAll();
            TempData["LinhVuc"] = lstLinhVuc;
           
            var lstDonVi = _donViTTHCRepository.GetAll();
            TempData["DonVi"] = lstDonVi;

            var obj = new tbl_TTHC();
            TempData["AllowComment"] = obj.AllowComment;
            
            var lstTTHC = _TTHCRepository.GetAll();
            if (!string.IsNullOrEmpty(Tieude))
            {
                lstTTHC =
                    lstTTHC.Where(
                        g => HelperString.UnsignCharacter(g.Tieude.ToLower().Trim()).Contains(HelperString.UnsignCharacter(Tieude.ToLower().Trim()))).ToList();
            }
            if (!string.IsNullOrEmpty(LangCode))
            {
                lstTTHC = lstTTHC.Where(g => g.LangCode == LangCode).ToList();
            }
            lstTTHC = lstTTHC.OrderByDescending(g => g.CreatedDate);
           
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/TTHC/_ListData.cshtml", lstTTHC),
            }, JsonRequestBehavior.AllowGet);
        }
      
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLinhVuc = _linhVucTTHCRepository.GetAll();
            TempData["LinhVuc"] = lstLinhVuc;
            var lstDonVi = Common.CreateLevel(_donViTTHCRepository.GetAll());
            TempData["DonVi"] = lstDonVi;

            return Json(RenderViewToString("~/Areas/Admin/Views/TTHC/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(tbl_TTHC obj)
        {
            try
            {
                obj.CreatedDate = DateTime.Now;
                _TTHCRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới TTHC: {0}", obj.Tieude), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới TTHC thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            /*catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới TTHC thất bại")
                }, JsonRequestBehavior.AllowGet);
            }*/
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLinhVuc = _linhVucTTHCRepository.GetAll();
            TempData["LinhVuc"] = lstLinhVuc;
            var lstDonVi = Common.CreateLevel(_donViTTHCRepository.GetAll().ToList());
            TempData["DonVi"] = lstDonVi;

            var objTTHC = _TTHCRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/TTHC/_Edit.cshtml", objTTHC), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tbl_TTHC obj)
        {
            try
            {
                _TTHCRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật TTHC: {0}", obj.Tieude), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật TTHC thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            //catch (DbEntityValidationException e)
            //{
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw;
            //}
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật TTHC thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _TTHCRepository.Find(id);
                var arrCMT = _TTHCRepository.GetAllCMT().Where(g => g.TTHCID == id).Select(g => g.ID).ToArray();
                var strCMT = string.Join(",", arrCMT);
                DeleteAllCMT(strCMT);
                _TTHCRepository.Delete(id);
                
                LogController.AddLog(string.Format("Xóa TTHC: {0}", obj.Tieude), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa TTHC thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa TTHC thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAllRep(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _TTHCRepository.FindCMT(Convert.ToInt32(item));
                    // ten bai viet
                    var tenbaiviet = _TTHCRepository.Find(obj.TTHCID).Tieude;
                    //
                    _TTHCRepository.DeleteCMT(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", obj.NoiDung, tenbaiviet), User.Username);
                    count++;
                }
                catch (Exception ex)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bình luận", count),
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
                    _TTHCRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} TTHC", count),
            }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public ActionResult DetailsRepReply(tbl_TTHCComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                _TTHCRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Chỉnh sửa trả lời thành công",
                    cmtid = obj.CommentID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Chỉnh sửa trả lời thành công")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "View")]
        public ActionResult Reply(int commentId)
        {
            ViewBag.CommentId = commentId;
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataReply(string TuNgay, string DenNgay, int id, int page = 1)
        {
            TempData["cmtIDz"] = id;
            var obj = new tbl_TTHCComment();

            var lstData = _TTHCRepository.GetAllCMT();
            TempData["cmtName"] = _TTHCRepository.FindCMT(id).NoiDung;
            TempData["cmtID"] = _TTHCRepository.FindCMT(id).TTHCID;
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Value.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstData = lstData.Where(g => g.CreateDate != null && g.CreateDate.Value.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            if (id > 0)
            {
                lstData = lstData.Where(g => g.CommentID == id);
            }
            var total = lstData.Count();
            lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                cmtid = id,
                viewContent = RenderViewToString("~/Areas/Admin/Views/TTHC/ListDataReply.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailsReply(int id)
        {
            var obj = _TTHCRepository.FindCMT(id);
            TempData["objCMT"] = _TTHCRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/TTHC/DetailsReply.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DetailsReply(tbl_TTHCComment obj)
        {
            try
          {
                obj.CreatedBy = User.ID;
            _TTHCRepository.EditCMT(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Chỉnh sửa trả lời thành công",
                cmtid = obj.CommentID
            }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Chỉnh sửa trả lời thành công")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AddRepCmt(int id)
        {
            TempData["objCMT"] = _TTHCRepository.FindCMT(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/TTHC/AddRepCmt.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddRepCmt(tbl_TTHCComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                obj.CreateDate = DateTime.Now;
                _TTHCRepository.AddCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới trả lời thành công",
                    cmtid = obj.CommentID
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới trả lời thành công")
                }, JsonRequestBehavior.AllowGet);
            }


        }
        [Authorize(Roles = "View")]
        public ActionResult DetailsComment(int id)
        {
            var obj = _TTHCRepository.FindCMT(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/TTHC/DetailsComment.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        public ActionResult DetailsRep(int id)
        {
            var obj = _TTHCRepository.FindCMT(id);
            TempData["objCMT"] = _TTHCRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/TTHC/DetailsRep.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        #region Comment
        [Authorize(Roles = "View")]
        public ActionResult Comment( int TTHCID)
        {
            ViewBag.TTHCID = TTHCID;
            return View();
        }
        [Authorize(Roles = "View")]
        public ActionResult ListDataComment(string TuNgay, string DenNgay, int id, int page = 1)
        {
            var lstData = _TTHCRepository.GetAllCMT();
            TempData["tieude"] = _TTHCRepository.Find(id).Tieude;
            TempData["cmtid"] = _TTHCRepository.Find(id).ID;
            
            if (!string.IsNullOrEmpty(TuNgay))
            {
                lstData = lstData.Where(g => g.CreateDate.Value.Date >= HelperDateTime.ConvertDateTime(TuNgay).Date);
            }
            if (!string.IsNullOrEmpty(DenNgay))
            {
                lstData = lstData.Where(g => g.CreateDate.Value.Date <= HelperDateTime.ConvertDateTime(DenNgay).Date);
            }
            if (id > 0)
            {
                lstData = lstData.Where(g => g.TTHCID == id);
            }
            TempData["TTHC"] = _TTHCRepository.GetAll().ToList();
            var total = lstData.Count();
            lstData = lstData.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
         
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/TTHC/ListDataComment.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _TTHCRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _TTHCRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult EditComment(int id, string str = "Status")
        {
            var obj = _TTHCRepository.FindCMT(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _TTHCRepository.EditCMT(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult EditAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _TTHCRepository.FindCMT(Convert.ToInt32(item));
                    obj.Status = true;
                    _TTHCRepository.EditCMT(obj);
                    LogController.AddLog(string.Format("Kích hoạt bình luận: {0}", obj.ID), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Kích hoạt thành công {0} bình luận", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                var objPost = _TTHCRepository.FindCMT(id);
                var tenbaiviet = _TTHCRepository.Find(objPost.TTHCID).Tieude;
                var arrRep = _TTHCRepository.GetAllCMT().Where(g => g.CommentID == id).Select(g => g.ID).ToArray();
                var strRep = string.Join(",", arrRep);
                DeleteAllRep(strRep);
                _TTHCRepository.DeleteCMT(id);
                LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", objPost.NoiDung, tenbaiviet), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa bình luận thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa bình luận thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAllCMT(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _TTHCRepository.FindCMT(Convert.ToInt32(item));
                    var tenbaiviet = _TTHCRepository.Find(obj.TTHCID).Tieude;
                    _TTHCRepository.DeleteCMT(Convert.ToInt32(item));
                    LogController.AddLog(string.Format("Xóa bình luận: {0} trong bài viết {1}", obj.NoiDung, tenbaiviet), User.Username);
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} bình luận", count),
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
