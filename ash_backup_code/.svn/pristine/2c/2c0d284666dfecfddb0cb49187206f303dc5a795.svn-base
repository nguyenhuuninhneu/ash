using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        readonly IUserAdminRepository _userRepository = new UserAdminRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        readonly IqProcedureRepository _quyTrinhXuatBanRepository = new qProcedureRepository();
        readonly IChatRepository _chatRepository = new ChatRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        //
        // GET: /Admin/Home/
        [Authorize]
        public ActionResult Index()
        {
            if (User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            //if (User.UserType == (int)EnumHelper.UserType.Binhthuong)
            //{
            //    return RedirectToAction("AccessDenined", "Error");
            //}
            // Thông tin đăng nhập User:
            var currentUser = _userRepository.Find(User.ID);
            ViewBag.rowUser = currentUser;
            // List danh sach Tin bai theo trang thai
            if (User.isAdmin == false&& currentUser.QuyTrinhXuatBanID!=null)
            {
                ViewBag.lstQuyenId = currentUser.QuyTrinhXuatBanID.Split(',').ToList();
            }
            else
            {
                ViewBag.lstQuyenId = _quyTrinhXuatBanRepository.GetAll().Select(g => g.ID.ToString()).ToList();
            }

            TempData["lstQuyTrinh"] = _newsRepository.GetDataHome().ToList(); ;
            return View();
        }
        [Authorize]
        public ActionResult Chat(int id)
        { 
            var currentUser = _userRepository.Find(User.ID);
            TempData["rowUser"] = currentUser;

            var chooseUser = _userRepository.Find(id); 
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Home/Chat.cshtml",chooseUser), 
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AjaxChat(string id, string id1,string date)
        { 
            var listchat = _chatRepository.GetAll()
                .Where(g => g.ListIdUser.Split(',').Contains(id) && g.ListIdUser.Split(',').Contains(id1) && g.Date < Convert.ToDateTime(date))
                .OrderByDescending(g => g.Date).FirstOrDefault(); 
            TempData["currentID"] = Convert.ToInt32(id);

            var user1 = (_userAdminRepository.Find(Convert.ToInt32(id))!= null &&_userAdminRepository.Find(Convert.ToInt32(id)).Photo != null ? _userAdminRepository.Find(Convert.ToInt32(id)).Photo : "");
            TempData["user1"] = user1;
            var user2 = (_userAdminRepository.Find(Convert.ToInt32(id1)) != null && _userAdminRepository.Find(Convert.ToInt32(id1)).Photo != null ? _userAdminRepository.Find(Convert.ToInt32(id1)).Photo : "");
            TempData["user2"] = user2;

            string timecheck = "";
            bool check = false;
            if (listchat != null)
            {
                timecheck = listchat.Date.ToString();
                check = true;
            }
            else
            {
                check = false;
            }
            return Json(new
            {
                check = check,
                timecheck = timecheck,
                viewContent = RenderViewToString("~/Areas/Admin/Views/Home/AjaxChat.cshtml", listchat), 
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateCountChat(string id, string id1)
        {
            var listchat = _chatRepository.GetAll().Where(g => g.ListIdUser.Split(',').Contains(id) && g.ListIdUser.Split(',').Contains(id1)).ToList();
            foreach (var item in listchat)
            {
                item.Count = 0;
                item.IdLast = null;
                _chatRepository.Edit(item);
            }

            return Json(new
            {
                success = true
            }, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult CheckUser()
        { 
            return Json(new
            {
                id = User.ID
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult SetTimeLogin()
        {
            var userobj = _userRepository.Find(User.ID);
            if (userobj != null)
            {
                userobj.LastOnline = DateTime.Now;
                userobj.isOnline = true;
                _userRepository.Edit(userobj);
            }
            return View();

        }
        [Authorize]
        public ActionResult UserOnline()
        {
            var userobj = _userRepository.Find(User.ID);
            TempData["currentid"] = userobj;
            if (userobj != null)
            {
                userobj.LastOnline = DateTime.Now;
                userobj.isOnline = true;
                _userRepository.Edit(userobj);
            }
            var timeout = Convert.ToInt32(@ConfigTextController.GetValueCFT("timeoutlogin"));
            TempData["timeout"] = timeout;
            var lstusers = _userRepository.GetAll().Where(p => p.Active && p.ID != User.ID).OrderByDescending(g=>g.isOnline).ThenByDescending(g=> g.LastOnline).ToList();

            var lstchat = _chatRepository.GetAll().Where(g => g.ListIdUser != null && g.ListIdUser.Split(',').Contains(User.ID.ToString())).ToList();
            TempData["lstchat"] = lstchat; 

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Home/UserOnline.cshtml", lstusers),
              
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        //public ActionResult GroupChat()
        //{  
        //    var lstgroup = _chatRepository.GetAll()
        //        .Where(g => g.Code == "groupchat" && g.ListIdUser.Split(',').Contains(User.ID.ToString())).ToList(); 

        //    return Json(new
        //    {
        //        viewContent = RenderViewToString("~/Areas/Admin/Views/Home/GroupChat.cshtml", lstgroup),
              
        //    }, JsonRequestBehavior.AllowGet);
        //}
         
        public ActionResult InfoAcount(int id)
        {
            var userobj = _userRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Home/InfoAcount.cshtml", userobj), JsonRequestBehavior.AllowGet); 
        } 
        //public ActionResult AddGroupChat()
        //{
        //    var lstUser = _userAdminRepository.GetAll().Where(g => g.Active && g.ID != User.ID).ToList();
        //    TempData["lstUser"] = lstUser;

        //    return Json(RenderViewToString("~/Areas/Admin/Views/Home/AddGroupChat.cshtml", null), JsonRequestBehavior.AllowGet); 
        //}
        //[HttpPost]
        //[ValidateInput(false)] 
        //public ActionResult AddGroupChat(tbl_Chat obj)
        //{ 
        //    obj.Code = "groupchat";
        //    var listUser = Request.Form["ListIdUser"];
        //    listUser += "," + User.ID;
        //    obj.ListIdUser = listUser;
        //    try
        //    {
        //        _chatRepository.Add(obj);
        //        return Json(new
        //        {
        //            IsSuccess = true,
        //            Messenger = string.Format("Thêm mới nhóm thành công")
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception e)
        //    {
        //        return Json(new
        //        {
        //            IsSuccess = false,
        //            Messenger = string.Format("Thêm mới nhóm thất bại")
        //        }, JsonRequestBehavior.AllowGet);
        //    }  
        //}

    }
}
