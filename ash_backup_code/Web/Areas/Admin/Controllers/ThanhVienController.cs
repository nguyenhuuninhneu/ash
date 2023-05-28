using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using PagedList;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Web.Areas.Admin.Controllers
{
    public class ThanhVienController : BaseController
    {
        //
        // GET: /ThanhVien/
        readonly IUserRepository _thanhvienRepository = new UserRepository();
        private IDMChungRepository _repositorydonvi = new DMChungRepository();
        private PortalNewsEntities dbcontext = new PortalNewsEntities();
        readonly IChucVuRepository _chucvuservice = new ChucVuRepository();
        
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        #region QUẢN LÝ NGƯỜI DÙNG

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int page = 1)
        {
            var lstdonvi = _repositorydonvi.GetAll().Where(s => s.Code == "tructhuoc" && s.Status == true).ToList();
            var lstdonvi1 = Common.CreateLevel(lstdonvi.ToList());
            TempData["donvi"] = lstdonvi1;
            var lstchucvu = dbcontext.tbl_ChucVu.OrderByDescending(s => s.Ordering).ToList();
            TempData["chucvu"] = lstchucvu;
            var lstUser = _thanhvienRepository.GetAll().ToList();
            
            if (!string.IsNullOrEmpty(Name))
            {
                lstUser = lstUser.Where(g => HelperString.UnsignCharacter(g.FullName.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Name.ToLower().Trim())) || HelperString.UnsignCharacter(g.UserName.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }
            var lstLevel = lstUser.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            TempData["ThanhVienls"] = lstUser;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/ThanhVien/_ListData.cshtml", lstLevel),
                totalPages = Math.Ceiling(((double)lstUser.Count / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_User>)TempData["ThanhVienls"];
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                ws.Cells["A1:G1"].Merge = true;
                ws.Cells[1, 1].Value = "Danh Sách Thành Viên";
                ws.Cells[2, 1].Value = "STT";
                ws.Cells[2, 2].Value = "Họ Tên";
                ws.Cells[2, 3].Value = "Tên Đăng Nhập";
                ws.Cells[2, 4].Value = "Chức Vụ";
                ws.Cells[2, 5].Value = "Đơn Vị";
                ws.Cells[2, 6].Value = "Số Di Động";
                ws.Cells[2, 7].Value = "Email";
                ws.Cells["A1:G1"].Style.Font.Bold = true;
                ws.Cells["A1:G1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:G2"].Style.Font.Bold = true;
                ws.Cells["A2:G2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:G2"].Style.WrapText = true;
                ws.Cells["A:XFD"].Style.Font.Name = "Arial";
                ws.Row(1).Height = 30;
                ws.Row(2).Height = 30;
                ws.Column(2).Width = 30;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 50;
                ws.Column(6).Width = 20;
                ws.Column(7).Width = 35;
                int iRow = 2;
                int i = 0;
                foreach (var item in Data)
                {
                    var chucvumodel = _chucvuservice.Find(item.ChucVuId);
                    var donvimodel = _repositorydonvi.Find(item.DonviId);
                    i++;
                    iRow++;
                    ws.Cells[iRow, 1].Value = i.ToString();
                    ws.Cells[iRow, 2].Value = item.FullName;
                    ws.Cells[iRow, 3].Value =item.UserName;
                    ws.Cells[iRow, 4].Value = (chucvumodel!=null)?chucvumodel.Name:"Chưa cập nhật";
                    ws.Cells[iRow, 5].Value = (donvimodel!=null)?donvimodel.Ten:"Chưa cập nhật";
                    ws.Cells[iRow, 6].Value = item.Phone;
                    ws.Cells[iRow, 7].Value = item.Email;
                    ws.Row(iRow).Height = 20;
                }
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("DS_ThanhVien-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstdonvi = _repositorydonvi.GetAll().Where(s => s.Code == "tructhuoc" && s.Status == true).ToList();
            var lstdonvi1 = Common.CreateLevel(lstdonvi.ToList());
            TempData["donvi"] = lstdonvi1;
            var lstchucvu = dbcontext.tbl_ChucVu.OrderByDescending(s => s.Ordering).ToList();
            TempData["chucvu"] = lstchucvu;
            return Json(RenderViewToString("~/Areas/Admin/Views/ThanhVien/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_User obj, int[] PageNoiBo)
        {
            try
            {

                obj.CreatedDate = DateTime.Now;
                obj.Password = HelperEncryptor.Md5Hash(obj.Password);
                obj.PageNoiBo = PageNoiBo != null ? string.Join(",", PageNoiBo) : null;
                _thanhvienRepository.Add(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới người dùng thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới người dùng thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstdonvi = _repositorydonvi.GetAll().Where(s => s.Code == "tructhuoc" && s.Status == true).ToList();
            var lstdonvi1 = Common.CreateLevel(lstdonvi.ToList());
            TempData["donvi"] = lstdonvi1;
            var lstchucvu = dbcontext.tbl_ChucVu.OrderByDescending(s => s.Ordering).ToList();
            TempData["chucvu"] = lstchucvu;
            var objUser = _thanhvienRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/ThanhVien/_Edit.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_User obj, int[] PageNoiBo)
        {
            try
            {
                var objUser = _thanhvienRepository.Find(obj.ID);
                objUser.FullName = obj.FullName;
                objUser.IsBanChapHanh = obj.IsBanChapHanh;
                objUser.Active = obj.Active;
                objUser.NoiBo = obj.NoiBo;
                objUser.DonviId = obj.DonviId;
                objUser.ChucVuId = obj.ChucVuId;
                objUser.Phone = obj.Phone;
                objUser.Telephone = obj.Telephone;
                objUser.PageNoiBo = PageNoiBo != null ? string.Join(",", PageNoiBo) : null;
                objUser.UserName = obj.UserName;
                objUser.Email = obj.Email;
                _thanhvienRepository.Edit(objUser);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật người dùng thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật người dùng thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _thanhvienRepository.Delete(id);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh người dùng thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa người dùng thành công",
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
                    _thanhvienRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} người dùng", count),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ResetPassword(int id)
        {
            var objUser = _thanhvienRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/ThanhVien/ResetPassword.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult ResetPassword(int id, string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                try
                {
                    var objUser = _thanhvienRepository.Find(id);
                    objUser.Password = HelperEncryptor.Md5Hash(password);
                    _thanhvienRepository.Edit(objUser);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Reset mật khẩu thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Reset mật khẩu thất bại",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Reset mật khẩu thất bại",
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangePass(int id)
        {
            var objUser = _thanhvienRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/ThanhVien/ChangePass.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult ChangePass(int id, string oldpassword, string password, string confirmpassword)
        {
            var objUser = _thanhvienRepository.Find(id);
            if (objUser.Password != HelperEncryptor.Md5Hash(oldpassword))
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Mật khẩu cũ nhập vào không đúng",
                }, JsonRequestBehavior.AllowGet);
            }
            if (password == confirmpassword)
            {
                try
                {
                    objUser.Password = HelperEncryptor.Md5Hash(password);
                    _thanhvienRepository.Edit(objUser);
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Reset mật khẩu thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Reset mật khẩu thất bại",
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = false,
                Messenger = "Reset mật khẩu thất bại",
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
