using OfficeOpenXml;
using OfficeOpenXml.Style;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Areas.Admin.Controllers;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class DMChungController : BaseController
    {
        private readonly IDMChungRepository _repository = new DMChungRepository();
        readonly IDMNhomRepository _dmnhomRepository = new DMNhomRepository();
        readonly IUserRepository _userRepository = new UserRepository();
        // GET: /DMChung/
        [Authorize(Roles = "Index")]
        public ActionResult Index(string Code = "DMChung")
        {
            var lstnhomtructhuoc = _dmnhomRepository.GetAll().Where(s => s.Code == "tructhuoc").ToList();
            var lstnhomtinhthanh = _dmnhomRepository.GetAll().Where(s => s.Code == "tinhthanh").ToList();
            TempData["Tructhuoc"] = lstnhomtructhuoc;
            TempData["TinhThanh"] = lstnhomtinhthanh;
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Code = "tructhuoc", int Nhom = 0)
        {
            var lstDMNhom = _dmnhomRepository.GetAll().Where(g => g.Code == Code && g.Status).ToList();
            TempData["lstDMNhom"] = lstDMNhom;
            var lstDMChung = _repository.GetAll().Where(g => g.Code == Code).ToList();
            if (Nhom != 0)
            {
                lstDMChung = lstDMChung.Where(s => s.CatID == Nhom).ToList();
            }
            lstDMChung = Common.CreateLevel(lstDMChung);
            TempData["LstDMChung"] = lstDMChung;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/DMChung/_ListData.cshtml", lstDMChung),
            }, JsonRequestBehavior.AllowGet);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_DMChung>)TempData["LstDMChung"];
                var ws = package.Workbook.Worksheets.Add("Sheet1");
                // Tạo header
                ws.Cells["A1:F1"].Merge = true;
                ws.Cells[1, 1].Value = "Danh Sách Đơn Vị";
                ws.Cells[2, 1].Value = "STT";
                ws.Cells[2, 2].Value = "Tên Đơn Vị";
                ws.Cells[2, 3].Value = "Nhóm";
                ws.Cells[2, 4].Value = "Điện Thoại";
                ws.Cells[2, 5].Value = "Di Động";
                ws.Cells[2, 6].Value = "Trạng Thái";
                ws.Cells["A1:F1"].Style.Font.Bold = true;
                ws.Cells["A1:F1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A1:F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:F2"].Style.Font.Bold = true;
                ws.Cells["A2:F2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2:F2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A2:F2"].Style.WrapText = true;
                ws.Cells["A:XFD"].Style.Font.Name = "Arial";
                ws.Row(1).Height = 30;
                ws.Row(2).Height = 30;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 30;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;
                int iRow = 2;
                int i = 0;
                foreach (var item in Data)
                {
                    var nhomdonvi = _dmnhomRepository.Find(item.CatID);
                    i++;
                    iRow++;
                    ws.Cells[iRow, 1].Value = i.ToString();
                    ws.Cells[iRow, 2].Value = item.Ten;
                    ws.Cells[iRow, 3].Value = nhomdonvi.Name;
                    ws.Cells[iRow, 4].Value = item.Phone;
                    ws.Cells[iRow, 5].Value = item.DiDong;
                    ws.Cells[iRow, 6].Value = (item.Status)? "Kích hoạt":"Vô hiệu";
                    ws.Row(iRow).Height = 20;
                    //ws.Cells["B" + iRow].Style.Font.Bold = true;
                }
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("DS_DonVi-{0}{1}{2}_{3}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add(string Code = "tructhuoc")
        {
            var lstDMNhom = _dmnhomRepository.GetAll().Where(g => g.Code == Code && g.Status).ToList();
            TempData["lstDMNhom"] = lstDMNhom;
            var lstUser = _userRepository.GetAll().Where(g => g.Active).ToList();
            TempData["lstUser"] = lstUser;
            var lstDB = _repository.GetAll().Where(g => g.Code == Code);
            lstDB = Common.CreateLevel(lstDB.ToList());
            TempData["lstParent"] = lstDB;
            //ViewBag.lstDB = lstDB;
            var tblDMChung = _repository.GetAll().OrderByDescending(g => g.Ordering).FirstOrDefault(g => g.Code == Code && g.ParentID == 0);
            if (tblDMChung != null)
            {
                var maxtt = tblDMChung.Ordering;
                ViewBag.nextOrder = Convert.ToInt32(maxtt) + 1;
            }
            ViewBag.Code = Code;
            return Json(RenderViewToString("~/Areas/Admin/Views/DMChung/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_DMChung obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["BCH"]))
                {
                    obj.BCH = Request["BCH"];
                }
                _repository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới danh mục đơn vị: {0}", obj.Ten), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục đơn vị thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục đơn vị thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id, string Code = "DMChung")
        {
            var lstDMNhom = _dmnhomRepository.GetAll().Where(g => g.Code == Code && g.Status).ToList();
            TempData["lstDMNhom"] = lstDMNhom;
            var lstUser = _userRepository.GetAll().Where(g => g.Active).ToList();
            TempData["lstUser"] = lstUser;
            var obj = _repository.Find(id);
            var lstDB = _repository.GetAll().Where(g => g.Code == obj.Code && g.ID != id);
            if (obj.CatID > 0)
                lstDB = lstDB.Where(g => g.CatID == obj.CatID);
            lstDB = Common.CreateLevel(lstDB.ToList());
            TempData["lstParent"] = lstDB;
            ViewBag.lstDB = lstDB;
            var objDMChung = _repository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/DMChung/_Edit.cshtml", objDMChung), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tbl_DMChung obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(Request["BCH"]))
                {
                    obj.BCH = Request["BCH"];
                }
                _repository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật danh mục đơn vị: {0}", obj.Ten), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục đơn vị thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục đơn vị thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
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
                var obj = _repository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _repository.Edit(obj);

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

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _repository.Find(id);
                _repository.Delete(id);
                LogController.AddLog(string.Format("Xóa danh mục đơn vị: {0}", obj.Ten), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục đơn vị thất bại")
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục đơn vị thành công",
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
                    _repository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục đơn vị", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Update")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _repository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _repository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetAllParentByCat(string Code = "tructhuoc", int CatID = 0)
        {
            var lstParent = _repository.GetAll().Where(g => g.Code == Code && g.Status && g.CatID == CatID).ToList();
            lstParent = Common.CreateLevel(lstParent);
            lstParent.ForEach(g => g.Ten = string.Concat(Enumerable.Repeat("--", g.Level)) + " " + g.Ten);
            return Json(lstParent, JsonRequestBehavior.AllowGet);
        }
    }
}
