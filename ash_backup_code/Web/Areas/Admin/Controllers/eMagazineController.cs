using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class eMagazineController : BaseController
    {
        readonly IeMagazineRepository _eMagazineRepository = new eMagazineRepository();
        readonly ISubeMagazineRepository _SubeMagazineRepository = new SubeMagazineRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IImageCategoryRepository _newsimgCategoryReporitory = new ImageCategoryRepository();
        readonly IUserAdminRepository _repUserAdmin = new UserAdminRepository();
        //
        // GET: /Admin/SlideeMagazine/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(int catid = 0, string LangCode = "vn", int PageElementId = 0, int page = 1)
        {
            var lstCat = _newsimgCategoryReporitory.GetAll().OrderBy(x => x.Ordering).ToList();
            TempData["Category"] = lstCat;
            var lsteMagazine = _eMagazineRepository.GetAll().OrderBy(x => x.Ordering).ToList();
            if (catid > 0)
            {
                //lsteMagazine = lsteMagazine.Where(g => g.ImageCategoryId == catid).ToList();
            }
            var totalSlideeMagazine = lsteMagazine.Count();
            lsteMagazine = lsteMagazine.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/eMagazine/_ListData.cshtml", lsteMagazine),
                totalPages = Math.Ceiling(((double)totalSlideeMagazine / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstCTV = _userAdminRepository.GetAll().Where(g => g.IsCTV == true).ToList();
            TempData["lstCTV"] = lstCTV;
            return Json(RenderViewToString("~/Areas/Admin/Views/eMagazine/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_eMagazine obj)
        {
            try
            {
                var date = Request["EditDate"];
                if (!string.IsNullOrEmpty(date))
                {
                    obj.CreatedDate = HelperDateTime.ConvertDate(date);
                }

                var lstImg = obj.Photo;
                for (int i = 0; i < lstImg.Split('|').Length; i++)
                {
                    obj.Photo = lstImg.Split('|')[i];
                    _eMagazineRepository.Add(obj);
                }

                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới eMagazine thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới eMagazine thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objeMagazine = _eMagazineRepository.Find(id);
            var lstCTV = _userAdminRepository.GetAll().Where(g => g.IsCTV == true).ToList();
            TempData["lstCTV"] = lstCTV;
            return Json(RenderViewToString("~/Areas/Admin/Views/eMagazine/_Edit.cshtml", objeMagazine), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_eMagazine obj)
        {
            try
            {
                var date = Request["EditDate"];
                if (!string.IsNullOrEmpty(date))
                {
                    obj.CreatedDate = HelperDateTime.ConvertDate(date);
                }
                _eMagazineRepository.Edit(obj);
                LogController.AddLog(string.Format("Sửa eMagazine: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật eMagazine thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật eMagazine thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _eMagazineRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _eMagazineRepository.Edit(obj);
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
                var listSubeMagazine = _SubeMagazineRepository.GetAll().Where(g => g.IDParent == id).ToList();
                foreach (var item in listSubeMagazine)
                {
                    _SubeMagazineRepository.Delete(item.ID);
                }
                var obj = _eMagazineRepository.Find(id);
                _eMagazineRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa eMagazine: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa eMagazine thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa eMagazine thành công",
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
                    var listSubeMagazine = _SubeMagazineRepository.GetAll().Where(g => g.IDParent == Convert.ToInt32(item)).ToList();
                    foreach (var items in listSubeMagazine)
                    {
                        _SubeMagazineRepository.Delete(items.ID);
                    }
                    _eMagazineRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} eMagazine", count),
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
                var obj = _eMagazineRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _eMagazineRepository.Edit(obj);
                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật danh sách thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật danh sách thành công",
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddeMagazine(int id)
        {
            var objeMagazine = _eMagazineRepository.Find(id); 
            TempData["objeMagazine"] = objeMagazine;
            var lstSub = _SubeMagazineRepository.GetAll().Where(g => g.IDParent == id).OrderBy(g => g.Ordering).ToList();
            return View(lstSub);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddeMagazine()
        {
            var idParent = Request.Form["IDParent"];
            var isContinue = Request.Form["isContinue"];
            try
            {
                var total = Request.Form["Total"]; 

                var lstSub = _SubeMagazineRepository.GetAll().Where(g => g.IDParent == Convert.ToInt32(idParent))
                    .ToList();
                if (lstSub.Count > 0 && lstSub != null)
                {
                    foreach (var item in lstSub)
                    {
                        _SubeMagazineRepository.Delete(item.ID);
                    }
                } 

                string timeStr = DateTime.Now.ToString("yyyyMMddHHmm");
                var objeMagazine = _eMagazineRepository.Find(Convert.ToInt32(idParent));

                if (objeMagazine != null)
                {
                    for (int i = 0; i < Convert.ToInt32(total); i++)
                    {
                        var ordering = Request.Form["ordering_" + i];
                        var content = Request.Unvalidated["content_" + i];

                        var imgFile = Request.Files.GetMultiple("fileimg_" + i);
                        string strImg = ""; 
                        foreach (var item in imgFile)
                        {
                            if (!string.IsNullOrEmpty(item.FileName))
                            {
                                string fnameDLV = Path.GetFileName(item.FileName);
                                fnameDLV = HelperString.RemoveMark(fnameDLV);
                                var fileNameDLV = "[" + timeStr + "]" + fnameDLV.Replace(" ", "-");
                                item.SaveAs(Path.Combine(Server.MapPath("/Content/eMagazine/"), fileNameDLV));
                                strImg += "/Content/eMagazine/" + fileNameDLV + "|";
                            }
                        }
                        var strPhoto = Request.Form["StrPhoto_" + i];
                        if (!String.IsNullOrEmpty(strPhoto))
                        {
                            strImg = strPhoto + "|" + strImg;
                        }
                        var countimg = Request.Form["countimg_" + i];
                        var check = Request.Form["check_" + i];
                        var position = Request.Form["position_" + i];
                        var widthimg = Request.Form["widthimg_" + i];

                        var obj = new tbl_SubeMagazine();
                        obj.Ordering = Convert.ToInt32(ordering);
                        obj.Content = content;
                        obj.NumberPhoto = Convert.ToInt32(countimg);
                        obj.isFullWidth = Convert.ToBoolean(check);
                        obj.WidthPhoto = Convert.ToInt32(widthimg);
                        obj.PositionPhoto = Convert.ToInt32(position);
                        obj.IDParent = Convert.ToInt32(idParent);
                        if (!String.IsNullOrEmpty(strImg))
                            strImg = strImg.TrimEnd('|');
                        obj.StrPhoto = strImg;
                        if(!string.IsNullOrEmpty(content) || !string.IsNullOrEmpty(strImg))
                            _SubeMagazineRepository.Add(obj);
                    }
                } 
            }
            catch (Exception e)
            {
                if (isContinue == "0") return Redirect("/Admin/eMagazine/AddeMagazine?id=" + idParent + "&sc=2");
                else return Redirect("/Admin/eMagazine?sc=2");
            }
            if (isContinue == "0") return Redirect("/Admin/eMagazine/AddeMagazine?id=" + idParent + "&sc=1");
            else return Redirect("/Admin/eMagazine?sc=1");
        }
        public ActionResult ThongKe(string tuNgay, string denNgay)
        {
            var lstEmagazine = _eMagazineRepository.GetAll().Where(g => g.Status).OrderByDescending(g=>g.CreatedDate).ToList();
           
            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {
                lstEmagazine = lstEmagazine.Where(g =>g.CreatedDate.HasValue&& g.CreatedDate.Value.Date >= HelperDateTime.ConvertDateTime(tuNgay) && g.CreatedDate.Value.Date <= HelperDateTime.ConvertDateTime(denNgay)).ToList();
            }
            var objUser = new tbl_UserAdmin();
            foreach (var item in lstEmagazine)
            {
                objUser =new tbl_UserAdmin();
                if(item.CreatedBy.HasValue)
                    objUser = _repUserAdmin.Find(item.CreatedBy.Value);
                if (objUser != null)
                    item.TenNguoiKhoiTao = objUser.FullName;

            }
            Session["tuNgay"] = tuNgay;
            Session["denNgay"] = denNgay;
            Session["lstEmagazine"] = lstEmagazine;
            return View(lstEmagazine);
        }
        public string ExportThongKe()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_eMagazine>)Session["lstEmagazine"];
                var tuNgay = Session["tuNgay"];
                var denNgay = Session["denNgay"];
                var strNgay = "";
                if (tuNgay != null && denNgay != null)
                    strNgay = "(" + tuNgay + " - " + denNgay + ")";
                TempData.Keep();
                var ws = package.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1:B1"].Merge = true;
                ws.Cells["A2:B2"].Merge = true;
                ws.Cells["C1:E1"].Merge = true;
                ws.Cells["C2:E2"].Merge = true;

                ws.Cells["A4:E4"].Merge = true;
                ws.Cells["A5:E5"].Merge = true;
                ws.Cells["D6:E6"].Merge = true;

                ws.Cells["A1"].Value = "BAN TUYÊN GIÁO TRUNG ƯƠNG";
                ws.Cells["A2"].Value = "TẠP CHÍ TUYÊN GIÁO";
                ws.Cells["C1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                ws.Cells["C2"].Value = "Độc lập - Tự do - Hạnh phúc";
                ws.Cells["A4"].Value = "BÁO CÁO TÌNH HÌNH NHUẬN BÚT";
                ws.Cells["A5"].Value = strNgay;
                ws.Cells["D6"].Value = "Ngày giờ xuất báo cáo: " + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);

                ws.Cells["A7"].Value = "STT";
                ws.Cells["B7"].Value = "Tên bài e - magazine";
                ws.Cells["C7"].Value = "Người khởi tạo";
                ws.Cells["D7"].Value = "Ngày đăng";
                ws.Cells["E7"].Value = "Nhuận bút";


                ws.Cells["A7:E7"].Style.Font.Bold = true;
                ws.Cells["A1:E6"].Style.Font.Size = 13;
                ws.Cells["A7:E7"].Style.Font.Size = 12;

                ws.Cells["A1:A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A7:E7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A7:E7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["C1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["C2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["C2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A4"].Style.Font.Bold = true;

                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A1:E6"].Style.Font.Name = "Times New Roman";

                ws.Row(7).Height = 30;
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;

                ws.Column(5).Style.Numberformat.Format = "#,#";
                int iRow = 7;
                int stt = 0;
                var objUser = new tbl_UserAdmin();
                foreach (var item in Data)
                {
                    objUser = new tbl_UserAdmin();

                    if (item.CreatedBy.HasValue)
                        objUser = _repUserAdmin.Find(item.CreatedBy.Value);
                    if (objUser != null)
                        item.TenNguoiKhoiTao = objUser.FullName;
                    stt++;
                    iRow++;
                    ws.Cells["A" + iRow].Value = stt;
                    ws.Cells["B" + iRow].Value = item.Name;
                    ws.Cells["C" + iRow].Value = item.TenNguoiKhoiTao;
                    ws.Cells["D" + iRow].Value = HelperDateTime.ConvertDateToString(item.CreatedDate);
                    ws.Cells["E" + iRow].Value = item.NewMoney;
                    ws.Cells["E" + iRow].Style.Numberformat.Format = "#,#";

                    ws.Cells["A7" + ":E" + iRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":E" + iRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":E" + iRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":E" + iRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //Căn vị trí
                    ws.Cells["A" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["D8:D" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["D8:D" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B8" + ":E" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7" + ":E" + iRow].Style.Font.Size = 11;
                    ws.Cells["A8" + ":E" + iRow].Style.WrapText = true;
                    ws.Cells["A8:E" + iRow].Style.Font.Name = "Times New Roman";

                }
                ws.Cells["D" + (Data.Count + 8) + ":E" + (Data.Count + 8)].Merge = true;
                ws.Cells[Data.Count + 8, 3].Value = "Tổng cộng: ";
                ws.Cells[Data.Count + 8, 3].Style.Font.Bold = true;
                ws.Cells[Data.Count + 8, 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells[Data.Count + 8, 4].Formula = string.Format("=SUM(E8:E{0})", iRow);
                ws.Cells[Data.Count + 8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[Data.Count + 8, 4].Style.Numberformat.Format = "#,#";
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("NhuanButEMagazine-{0}-{1}-{2}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        [HttpGet]
        public ActionResult checkDate(string tuNgay, string denNgay)
        {
            var check = false;
            if (!string.IsNullOrEmpty(tuNgay) && !string.IsNullOrEmpty(denNgay))
            {
                var dateStart = HelperDateTime.ConvertDateTime(tuNgay);
                var dateEnd = HelperDateTime.ConvertDateTime(denNgay);
                if (dateStart > dateEnd)
                {
                    check = true;
                }
            }

            return Json(new {
                check= check
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
