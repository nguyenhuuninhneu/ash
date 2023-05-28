using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    public class GroupCustomerController : BaseController
    {
        private IGroupCusRepository customerService = new CusGroupRepository();
        private IUserRepository userservice = new UserRepository();
        private IDMChungRepository donviservice = new DMChungRepository();
        private IChucVuRepository chucvuservice = new ChucVuRepository();
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int page = 1)
        {
            var lstGroupUser = customerService.Getall();
            var totalRecord = lstGroupUser.Count();
            lstGroupUser =
                customerService.Getall().Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/GroupCustomer/_ListData.cshtml", lstGroupUser),
                totalPages = Math.Ceiling(((double)totalRecord / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            return Json(RenderViewToString("~/Areas/Admin/Views/GroupCustomer/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(GroupCusUser obj)
        {
            try
            {
                customerService.Add(obj);
                //LogController.AddLog(string.Format("Thêm mới nhóm thành viên: {0}", obj.NameGroup), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới nhóm thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới nhóm thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objGroupUser = customerService.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/GroupCustomer/_Edit.cshtml", objGroupUser), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(GroupCusUser obj)
        {
            try
            {
                var objOld = customerService.Find(obj.ID);
                objOld.NameGroup = obj.NameGroup;
                objOld.Status = obj.Status;
                objOld.Ordering = obj.Ordering;
                customerService.Edit(objOld);
                // LogController.AddLog(string.Format("Sửa nhóm người dùng: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật nhóm thành viên thành công",
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
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = customerService.Find(id);
                customerService.Delete(id);
                //LogController.AddLog(string.Format("Xóa nhóm người dùng: {0}", obj.NameGroup), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa nhóm thành viên thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa nhóm thành viên thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Update(List<GroupCusUser> entity)
        {
            foreach (var item in entity)
            {
                customerService.Edit(item);
            }
            return RedirectToAction("Index");
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
                    customerService.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} nhóm thành viên", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult Search(GroupCusUser obj, int page = 1)
        {
            var lstGroupUser = customerService.Getall();
            if (!string.IsNullOrEmpty(obj.NameGroup))
            {
                lstGroupUser =
                    lstGroupUser.Where(
                        g =>
                            HelperString.UnsignCharacter(g.NameGroup.ToLower().Trim())
                                .Contains(HelperString.UnsignCharacter(obj.NameGroup).ToLower().Trim()));
            }
            var totalRecord = lstGroupUser.Count();
            lstGroupUser =
                lstGroupUser.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/GroupCustomer/_ListData.cshtml", lstGroupUser),
                totalPages = Math.Ceiling(((double)totalRecord / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult AddUserToGroup(int id)
        {
            var lstdonvi = donviservice.GetAll().Where(s=>s.Code=="tructhuoc"&& s.Status==true).ToList();
            var lstchucvu = chucvuservice.GetAll().Where(s =>s.isPublish == true).ToList();
            TempData["donvi"]=lstdonvi;
            TempData["chucvu"] = lstchucvu;
            var objGroupUser = customerService.Find(id);
            var lstUser = userservice.GetAll().Where(g => g.Active).ToList();
            //lấy những user đã được thêm vào nhóm
            var lstUserAdded =
            lstUser.Where(
                g =>
                    !string.IsNullOrEmpty(g.GroupUserID) &&
                    g.GroupUserID.Split(',').Select(Int32.Parse).Contains(id)).ToList();
            //danh sách user chưa được thêm vào nhóm
            var lstUserUnAdd = lstUser;
            //lstUser.Where(
            //    g =>
            //        lstUserAdded.Count(t => t.ID == g.ID) == 0 && g.UserType != (int)EnumHelper.UserType.Binhthuong)
            //    .ToList();
            TempData["lstUserAdded"] = lstUserAdded;
            TempData["lstUserUnAdd"] = lstUserUnAdd;
            return Json(RenderViewToString("~/Areas/Admin/Views/GroupCustomer/_AddUserToGroup.cshtml", objGroupUser),
                JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult AddUserToGroup(int[] to, int id)
        {
            var lstUser = userservice.GetAll().ToList();
            var lstUserAdded = lstUser.Where(g => g.Active && !string.IsNullOrEmpty(g.GroupUserID) && g.GroupUserID.Split(',').Select(Int32.Parse).Contains(id)).ToList();
            foreach (var userAdded in lstUserAdded)
            {
                var arrUser = userAdded.GroupUserID.Split(',').Select(Int32.Parse).Where(val => val != id).ToArray();
                userAdded.GroupUserID = string.Join(",", arrUser);
            }
            try
            {
                userservice.Edit(lstUserAdded);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Có lỗi xảy ra")
                }, JsonRequestBehavior.AllowGet);
            }
            if (to != null)
            {
                var lstUserNewAdd = lstUser.Where(g => to.Contains(g.ID)).ToList();
                foreach (var user in lstUserNewAdd)
                {
                    var arrUser = new List<int>();
                    if (!string.IsNullOrEmpty(user.GroupUserID))
                    {
                        arrUser = user.GroupUserID.Split(',').Select(Int32.Parse).ToList();
                    }
                    arrUser.Add(id);
                    user.GroupUserID = string.Join(",", arrUser);
                }
                try
                {
                    userservice.Edit(lstUserNewAdd);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Có lỗi xảy ra")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = string.Format("Thêm thành viên vào nhóm thành công")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
