using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Recaptcha.Models;


namespace Web.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        //
        // GET: /Account/
        readonly IGroupUserRepository _groupuserRepository = new GroupUserRepository();
        readonly IUserAdminRepository _userRepository = new UserAdminRepository();
        readonly IPageElementRepository _pageElementRepository = new PageElementRepository();
        readonly IqProcedureRepository _quyTrinhXuatBanRepository = new qProcedureRepository();
        readonly IConfigTextRepository _configTextRepository = new ConfigTextRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index(int IsCTV = 0)
        {
            TempData["IsCTV"] = IsCTV;
            return View();
        }

        public ActionResult Login()
        {
            var host = Request.Url.Host;
            if (host.Trim().ToLower() == "localhost")
            {
                ViewData["sitekey"] = ConfigTextController.GetValueCFT("sitekeylocal");
            }
            else
            {
                ViewData["sitekey"] = ConfigTextController.GetValueCFT("sitekey");
            }

            ViewData["objrecapcha"] = ConfigTextController.GetValueCFT("recapcha");

            if (User != null) return RedirectToAction("Index", "Home");
            return View("/Areas/Admin/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public ActionResult Login(tbl_User obj)
        {
            var host = Request.Url.Host;
            if (host.Trim().ToLower() == "localhost")
            {
                ViewData["sitekey"] = ConfigTextController.GetValueCFT("sitekeylocal");
            }
            else
            {
                ViewData["sitekey"] = ConfigTextController.GetValueCFT("sitekey");
            }

            string objrecapcha = ConfigTextController.GetValueCFT("recapcha");
            ViewData["objrecapcha"] = objrecapcha;

            if (string.IsNullOrEmpty(obj.UserName) || string.IsNullOrWhiteSpace(obj.Password))
            {
                Session["Messenger"] = "Bạn chưa nhập tên đăng nhập hoặc mật khẩu nhập vào không đúng!";
                return View("/Areas/Admin/Views/Account/Login.cshtml");
            }

            if (objrecapcha == "" || objrecapcha == "off" || IsValidRecaptcha(Request["g-recaptcha-response"]))
            {
                var user = _userRepository.GetAll().FirstOrDefault(u => u.Password == HelperEncryptor.Md5Hash(obj.Password) && u.UserName == obj.UserName);
                if (user != null)
                {
                    user.TimeLogin = DateTime.Now;
                    user.IPLogin = obj.IPLogin;
                    user.LastOnline = DateTime.Now;
                    user.isOnline = true;
                    _userRepository.Edit(user);
                    if (user.Active == false)
                    {
                        Session["Messenger"] = "Tài khoản này chưa được kích hoạt!";
                        return View("/Areas/Admin/Views/Account/Login.cshtml");
                    }
                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        ID = user.ID,
                        Username = user.UserName,
                        FullName = user.FullName,
                        Email = user.Email,
                        GroupUser = user.GroupUserID,
                        isAdmin = user.isAdmin,
                    };
                    var serializer = new JavaScriptSerializer();

                    var userData = serializer.Serialize(serializeModel);
                    var authTicket = new FormsAuthenticationTicket(
                        1,
                        obj.UserName,
                        DateTime.Now,
                        DateTime.Now.AddHours(1),
                        false,
                        userData);

                    var encTicket = FormsAuthentication.Encrypt(authTicket);
                    var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    faCookie.Name = "ADMIN_COOKIES";
                    Response.Cookies.Add(faCookie);
                    var browsertype = "";
                    if (!String.IsNullOrEmpty(Request["BrowserType"]))
                    {
                        browsertype = Request["BrowserType"];
                    }
                    var devicetype = "";
                    if (!String.IsNullOrEmpty(Request["DeviceType"]))
                    {
                        devicetype = Request["DeviceType"];
                    }
                    LogController.AddLog(string.Format("Đăng Nhập"), obj.UserName, browsertype, devicetype);
                }
                else
                {
                    Session["Messenger"] = "Bạn chưa nhập tên đăng nhập hoặc mật khẩu nhập vào không đúng!";

                    return View("/Areas/Admin/Views/Account/Login.cshtml");
                }
                var preUrl = (Uri)Session["returnUrl"];
                if (preUrl != null)
                {
                    Session["returnUrl"] = null;
                    return Redirect(preUrl.ToString());
                }
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                ViewBag.CheckCapcha = "uncheck";
                return View("/Areas/Admin/Views/Account/Login.cshtml");
            }
        }

        [Authorize(Roles = "Add")]
        public ActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _userRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _userRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UserInfo(int id)
        {
            var objUser = _userRepository.GetAll().Where(g => g.ID == id).FirstOrDefault();
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/_UserInfo.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Register(tbl_UserAdmin user)
        {
            _userRepository.Add(user);
            var serializeModel = new CustomPrincipalSerializeModel
            {
                ID = user.ID,
                Username = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                GroupUser = user.GroupUserID,
                isAdmin = user.isAdmin,
            };

            var serializer = new JavaScriptSerializer();

            var userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(
                1,
                user.UserName,
                DateTime.Now,
                DateTime.Now.AddYears(1),
                false,
                userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
            return View();
        }
        //GET: /Account/Logout
        public ActionResult Logout()
        {
            //FormsAuthentication.SignOut();
            try
            {
                var userobj = _userRepository.Find(User.ID);
                if (userobj != null)
                {
                    userobj.isOnline = false;
                    _userRepository.Edit(userobj);
                }

            }
            catch (Exception)
            {
            }
            Session["Messenger"] = "";
            var httpCookie = Response.Cookies["ADMIN_COOKIES"];
            if (httpCookie != null) httpCookie.Expires = DateTime.Now.AddDays(-1);
            LogController.AddLog(string.Format("Đăng Xuất"), User.Username);
            return RedirectToAction("Login", "Account");
        }

        private bool IsValidRecaptcha(string recaptcha)
        {
            var host = Request.Url.Host;
            var secretkey = "";
            if (host.Trim().ToLower() == "localhost")
            {
                secretkey = ConfigTextController.GetValueCFT("secretkeylocal");
            }
            else
            {
                secretkey = ConfigTextController.GetValueCFT("secretkey");
            }


            if (secretkey != null)
            {
                if (string.IsNullOrEmpty(recaptcha))
                {
                    return false;
                }
                var secretKey = secretkey; //Mã bí mật
                string remoteIp = Request.ServerVariables["REMOTE_ADDR"];
                string myParameters = String.Format("secret={0}&response={1}&remoteip={2}", secretKey, recaptcha,
                    remoteIp);
                RecaptchaResult captchaResult;
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    var json = wc.UploadString("https://www.google.com/recaptcha/api/siteverify", myParameters);
                    var js = new DataContractJsonSerializer(typeof(RecaptchaResult));
                    var ms = new MemoryStream(Encoding.ASCII.GetBytes(json));
                    captchaResult = js.ReadObject(ms) as RecaptchaResult;
                    if (captchaResult != null && captchaResult.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
        #region QUẢN LÝ NGƯỜI DÙNG


        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, int IsCTV = 0, int page = 1)
        {

            var lstUser = _userRepository.GetAll().Where(g => g.IsCTV == Convert.ToBoolean(IsCTV)).ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                lstUser = lstUser.Where(g => HelperString.UnsignCharacter(g.FullName.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Name.ToLower().Trim())) || HelperString.UnsignCharacter(g.UserName.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
            }
            var lstLevel = lstUser.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            TempData["lstUser"] = lstUser;
            TempData["IsCTV"] = IsCTV;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Account/_ListData.cshtml", lstLevel),
                totalPages = Math.Ceiling(((double)lstUser.Count / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportExcel(int IsCTV)
        {

            using (var package = new ExcelPackage())
            {
                var Data = _userRepository.GetAll().Where(g => g.IsCTV == Convert.ToBoolean(IsCTV)).ToList();
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                if (IsCTV == 0)
                {
                    ws.Cells["A1:F1"].Merge = true;
                    ws.Cells[1, 1].Value = "DANH SÁCH NGƯỜI DÙNG";
                    ws.Cells[2, 1].Value = "STT";
                    ws.Cells[2, 2].Value = "Họ Tên";
                    ws.Cells[2, 3].Value = "Tên Đăng Nhập";
                    ws.Cells[2, 4].Value = "Email";
                    ws.Cells[2, 5].Value = "Địa Chỉ";
                    ws.Cells[2, 6].Value = "Số Điện Thoại";
                    ws.Cells["A1:F1"].Style.Font.Bold = true;
                    ws.Cells["A1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:F2"].Style.Font.Bold = true;
                    ws.Cells["A2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:F2"].Style.WrapText = true;
                    ws.Cells["A:XFF"].Style.Font.Name = "Times New Roman";
                    ws.Row(1).Height = 30;
                    ws.Row(2).Height = 30;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 40;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 40;
                    int iRow = 2;
                    int i = 0;
                    foreach (var item in Data)
                    {
                        var groupmodel = _groupuserRepository.Find(Convert.ToInt32(item.GroupUserID));
                        i++;
                        iRow++;
                        ws.Cells["A" + iRow + ":F" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A" + iRow + ":F" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[iRow, 1].Value = i.ToString();
                        ws.Cells[iRow, 2].Value = item.FullName;
                        ws.Cells[iRow, 3].Value = item.UserName;
                        ws.Cells[iRow, 4].Value = (item.Email != null) ? item.Email : "";
                        ws.Cells[iRow, 5].Value = (item.Address != null) ? item.Address : "";
                        ws.Cells[iRow, 6].Value = (item.Phone != null) ? item.Phone : "";
                        ws.Row(iRow).Height = 20;
                    }
                    
                }
                else
                {
                    ws.Cells["A1:G1"].Merge = true;
                    ws.Cells[1, 1].Value = "DANH SÁCH CỘNG TÁC VIÊN";
                    ws.Cells[2, 1].Value = "STT";
                    ws.Cells[2, 2].Value = "Họ Tên";
                    ws.Cells[2, 3].Value = "Email";
                    ws.Cells[2, 4].Value = "Địa Chỉ";
                    ws.Cells[2, 5].Value = "Số Điện Thoại";
                    ws.Cells[2, 6].Value = "Số Tài Khoản";
                    ws.Cells[2, 7].Value = "Chi Nhánh";
                    ws.Cells["A1:G1"].Style.Font.Bold = true;
                    ws.Cells["A1:G1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A1:G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:G2"].Style.Font.Bold = true;
                    ws.Cells["A2:G2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A2:G2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A2:G2"].Style.WrapText = true;
                    ws.Cells["A:XFG"].Style.Font.Name = "Times New Roman";
                    ws.Row(1).Height = 30;
                    ws.Row(2).Height = 30;
                    ws.Column(2).Width = 40;
                    ws.Column(3).Width = 40;
                    ws.Column(4).Width = 40;
                    ws.Column(5).Width = 40;
                    ws.Column(6).Width = 40;
                    ws.Column(7).Width = 40;
                    int iRow = 2;
                    int i = 0;
                    foreach (var item in Data)
                    {
                        var groupmodel = _groupuserRepository.Find(Convert.ToInt32(item.GroupUserID));
                        i++;
                        iRow++;
                        ws.Cells["A" + iRow + ":G" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A" + iRow + ":G" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells[iRow, 1].Value = i.ToString();
                        ws.Cells[iRow, 2].Value = item.FullName;
                        ws.Cells[iRow, 3].Value = (item.Email != null) ? item.Email : "";
                        ws.Cells[iRow, 4].Value = (item.Address != null) ? item.Address : "";
                        ws.Cells[iRow, 5].Value = (item.Phone != null) ? item.Phone : "";
                        ws.Cells[iRow, 6].Value = (item.SoTaiKhoan != null) ? item.SoTaiKhoan : "";
                        ws.Cells[iRow, 7].Value = (item.NganHang != null) ? item.NganHang : "";
                        ws.Row(iRow).Height = 20;
                    }
                }

                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("DS_NguoiDung-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return null;
        }
        [Authorize(Roles = "Add")]
        [HttpGet]
        public ActionResult Add(int IsCTV)
        {
            TempData["IsCTV"] = IsCTV;
            var lstQuyTrinhXuatBan = _quyTrinhXuatBanRepository.GetAll();
            ViewBag.QuyTrinhXuatBan = lstQuyTrinhXuatBan;
            var lstPageElement = _pageElementRepository.GetAll();
            TempData["PageElement"] = lstPageElement;
            var lstGroupUser = _groupuserRepository.GetAll().ToList();
            TempData["lstGroupUser"] = lstGroupUser;
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_UserAdmin obj, int[] arrquyTrinhXuatBanId)
        {
            try
            {
                //obj.IsCTV = Convert.ToBoolean(Request["IsCTV"]);
                obj.CreatedDate = DateTime.Now;
                if (obj.IsCTV == false)
                {
                    obj.Password = HelperEncryptor.Md5Hash(obj.Password);
                    obj.QuyTrinhXuatBanID =
                        arrquyTrinhXuatBanId != null ? string.Join(",", arrquyTrinhXuatBanId) : null;
                    obj.PageElementID = "1";
                }
                else
                {
                    obj.UserName = HelperString.RemoveMark(obj.FullName) + DateTime.Now.ToString("dd/MM/yyyy/hh/mm/ss").Replace("/",string.Empty); 
                    obj.Password = HelperEncryptor.Md5Hash(obj.UserName);
                }
                _userRepository.Add(obj);
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
            var lstQuyTrinhXuatBan = _quyTrinhXuatBanRepository.GetAll().OrderBy(g => g.Ordering).ToList();
            TempData["QuyTrinhXuatBan"] = lstQuyTrinhXuatBan;
            var objUser = _userRepository.Find(id);
            TempData["User"] = objUser;
            var lstPageElement = _pageElementRepository.GetAll();
            TempData["PageElement"] = lstPageElement;
            var lstGroupUser = _groupuserRepository.GetAll().ToList();
            TempData["lstGroupUser"] = lstGroupUser;
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/_Edit.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_UserAdmin obj, int[] arrquyTrinhXuatBanId, int[] arrPageElementId)
        {
            try
            {
                var objUser = _userRepository.Find(obj.ID);
                objUser.FullName = obj.FullName;
                objUser.Active = obj.Active;
                objUser.isAdmin = obj.isAdmin;
                objUser.Address = obj.Address;
                objUser.Phone = obj.Phone;
                objUser.SoTaiKhoan = obj.SoTaiKhoan;
                objUser.NganHang = obj.NganHang;
                objUser.IsDuyetComment = obj.IsDuyetComment;
                objUser.PageElementID = arrPageElementId != null ? string.Join(",", arrPageElementId) : null;
                objUser.QuyTrinhXuatBanID = arrquyTrinhXuatBanId != null ? string.Join(",", arrquyTrinhXuatBanId) : null;
                objUser.GroupUserID = obj.GroupUserID;
                objUser.IsCTV = obj.IsCTV;
                _userRepository.Edit(objUser);
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

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult EditStatusCTV(int id)
        {
            var objUser = _userRepository.Find(id);
            objUser.IsCTV = !objUser.IsCTV;
            _userRepository.Edit(objUser);
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
                _userRepository.Delete(id);
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
                    _userRepository.Delete(Convert.ToInt32(item));
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
            var objUser = _userRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/ResetPassword.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult ResetPassword(int id, string password, string confirmpassword)
        {
            if (password == confirmpassword)
            {
                try
                {
                    var objUser = _userRepository.Find(id);
                    objUser.Password = HelperEncryptor.Md5Hash(password);
                    _userRepository.Edit(objUser);
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
            var objUser = _userRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/ChangePass.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult ChangePass(int id, string oldpassword, string password, string confirmpassword)
        {
            var objUser = _userRepository.Find(id);
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
                    _userRepository.Edit(objUser);
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

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeInfo(int id)
        {
            var objUser = _userRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/ChangeInfo.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ViewInfo(int id)
        {
            var objUser = _userRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Account/ViewInfo.cshtml", objUser), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult ChangeInfo(tbl_UserAdmin obj)
        {
            try
            {
                var objUser = _userRepository.Find(obj.ID);
                objUser.FullName = obj.FullName;
                objUser.Address = obj.Address;
                objUser.Phone = obj.Phone;
                objUser.Photo = obj.Photo;
                objUser.SoTaiKhoan = obj.SoTaiKhoan;
                objUser.NganHang = obj.NganHang;
                _userRepository.Edit(objUser);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thông tin thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

            }
            return Json(new
            {
                IsSuccess = false,
                Messenger = "Cập nhật thông tin thất bại",
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}