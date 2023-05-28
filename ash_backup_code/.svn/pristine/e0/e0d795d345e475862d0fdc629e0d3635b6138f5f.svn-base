using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
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
    public class ImageCategoryController : BaseController
    {
        //

        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IImageCategoryRepository _imgCategoryReporitory = new ImageCategoryRepository();
        readonly IImagesRepository _imagesRep = new ImagesRepository();
        readonly IUserAdminRepository _repUserAdmin = new UserAdminRepository();

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
            var lstgallerycategory = _imgCategoryReporitory.GetAll().OrderBy(x => x.Ordering).ToList();

            LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var imagesCate in lstgallerycategory)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(imagesCate.LangCode) && imagesCate.LangCode == language.Code)
                        imagesCate.NgonNgu = language.Name;
                }
            }
            lstgallerycategory = lstgallerycategory.Where(g => g.LangCode == LangCode).ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                lstgallerycategory =
                    lstgallerycategory.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/ImageCategory/_ListData.cshtml", lstgallerycategory),
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
                TempData["langCode"] = objLanguages.Name;

            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/ImageCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_ImagesCategory obj)
        {
            try
            {
                obj.CreateBy = User.ID;
                var nhuanbut = Request["NewMoney"];
                var strNhuanBut = Common.RemoveChar(nhuanbut);
                obj.NewMoney = Convert.ToInt32(strNhuanBut);
                var check = _imgCategoryReporitory.GetAll().FirstOrDefault(p => p.Name == obj.Name);
                if (check != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Tiêu đề đã tồn tại.")
                    }, JsonRequestBehavior.AllowGet);
                }
                obj.CreatedDate = DateTime.Now;
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                _imgCategoryReporitory.Add(obj);
                GenImgHome();
                LogController.AddLog(string.Format("Thêm mới danh mục thư viện ảnh: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục thư viện ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục thư viện ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var objGalleryCategory = _imgCategoryReporitory.Find(id);
            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(objGalleryCategory.LangCode))
                langCode = objGalleryCategory.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["langCode"] = objLanguages.Name;
            return Json(RenderViewToString("~/Areas/Admin/Views/ImageCategory/_Edit.cshtml", objGalleryCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_ImagesCategory obj)
        {
            try
            {
                var nhuanbut = Request["NewMoney"];
                var strNhuanBut = Common.RemoveChar(nhuanbut);
                obj.NewMoney = Convert.ToInt32(strNhuanBut);
                _imgCategoryReporitory.Edit(obj);
                GenImgHome();
                LogController.AddLog(string.Format("Cập nhật danh mục thư viện ảnh: {0}", obj.Name), User.Username);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục thư viện ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục thư viện ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _imgCategoryReporitory.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _imgCategoryReporitory.Edit(obj);
            GenImgHome();
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
                var obj = _imgCategoryReporitory.Find(id);
                //xóa hình ảnh của danh mục
                var lstImage = _imagesRep.GetAll().Where(g => g.ImageCategoryId == id).ToList();
                foreach (var item in lstImage)
                {
                    _imagesRep.Delete(item.ID);
                }
                _imgCategoryReporitory.Delete(id);
                GenImgHome();
                LogController.AddLog(string.Format("Xóa danh mục thư viện ảnh: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục thư viện ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục thư viện ảnh thành công",
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
                    var lstImages = _imagesRep.GetAll().
                        Where(g => g.ImageCategoryId == Convert.ToInt32(item)).ToList();
                    foreach (var image in lstImages)
                    {
                        _imagesRep.Delete(image.ID);
                    }
                    _imgCategoryReporitory.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            GenImgHome();
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục thư viện ảnh", count),
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
                var obj = _imgCategoryReporitory.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _imgCategoryReporitory.Edit(obj);
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
            GenImgHome();
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ThongKe(string tuNgay, string denNgay)
        {
            var lstImageCate = _imgCategoryReporitory.GetAll().Where(g => g.Status).OrderByDescending(g=>g.CreatedDate).ToList();
            if(!string.IsNullOrEmpty(tuNgay)&&!string.IsNullOrEmpty(denNgay))
            {
                lstImageCate = lstImageCate.Where(g => g.CreatedDate.Date >= HelperDateTime.ConvertDateTime(tuNgay) && g.CreatedDate.Date <= HelperDateTime.ConvertDateTime(denNgay)).ToList();
            }
            var lstImages = new List<tbl_Images>();
            var objUser = new tbl_UserAdmin();
            foreach (var item in lstImageCate)
            {
                lstImages = new List<tbl_Images>();
                objUser = new tbl_UserAdmin();
                lstImages = _imagesRep.GetAll().Where(g => g.ImageCategoryId == item.ID).ToList();
                item.TongAnh = lstImages.Count();

                if (item.CreateBy.HasValue)
                    objUser = _repUserAdmin.Find(item.CreateBy.Value);
                if (objUser != null)
                    item.TenNguoiTao = objUser.FullName;
            }
            Session["lstImageCate"] = lstImageCate;
            Session["tuNgay"] = tuNgay;
            Session["denNgay"] = denNgay;
            return View(lstImageCate);
        }
        public string ExportExcel()
        {
            using (var package = new ExcelPackage())
            {
                var Data = (List<tbl_ImagesCategory>)Session["lstImageCate"];
                var tuNgay = Session["tuNgay"];
                var denNgay = Session["denNgay"];
                var strNgay = "";
                if (tuNgay != null && denNgay != null)
                    strNgay = "(" + tuNgay + " - " + denNgay + ")";
                TempData.Keep();
                var ws = package.Workbook.Worksheets.Add("Sheet1");

                ws.Cells["A1:C1"].Merge = true;
                ws.Cells["A2:C2"].Merge = true;
                ws.Cells["D1:F1"].Merge = true;
                ws.Cells["D2:F2"].Merge = true;
                ws.Cells["A5:F5"].Merge = true;

                ws.Cells["A4:F4"].Merge = true;
                ws.Cells["D6:F6"].Merge = true;

                ws.Cells["A1"].Value = "BAN TUYÊN GIÁO TRUNG ƯƠNG";
                ws.Cells["A2"].Value = "TẠP CHÍ TUYÊN GIÁO";
                ws.Cells["D1"].Value = "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM";
                ws.Cells["D2"].Value = "Độc lập - Tự do - Hạnh phúc";
                ws.Cells["A4"].Value = "BÁO CÁO TÌNH HÌNH NHUẬN BÚT";
                ws.Cells["A5"].Value = strNgay;
                ws.Cells["D6"].Value = "Ngày giờ xuất báo cáo: " + string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now);

                ws.Cells["A7"].Value = "STT";
                ws.Cells["B7"].Value = "Tên nhóm hình ảnh";
                ws.Cells["C7"].Value = "Số ảnh";
                ws.Cells["D7"].Value = "Người khởi tạo";
                ws.Cells["E7"].Value = "Ngày đăng";
                ws.Cells["F7"].Value = "Nhuận bút";


                ws.Cells["A7:F7"].Style.Font.Bold = true;
                ws.Cells["A1:F6"].Style.Font.Size = 13;
                ws.Cells["A7:F7"].Style.Font.Size = 12;

                ws.Cells["A1:A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                ws.Cells["A5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["D1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells["A4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["A4"].Style.Font.Bold = true;

                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["A1:F6"].Style.Font.Name = "Times New Roman";

                ws.Row(7).Height = 30;
                ws.Column(1).Width = 5;
                ws.Column(2).Width = 40;
                ws.Column(3).Width = 20;
                ws.Column(4).Width = 20;
                ws.Column(5).Width = 20;
                ws.Column(6).Width = 20;

                ws.Column(6).Style.Numberformat.Format = "#,#";
                int iRow = 7;
                int stt = 0;
                var lstImages = new List<tbl_Images>();
                var objUser = new tbl_UserAdmin();
                foreach (var item in Data)
                {
                    lstImages = new List<tbl_Images>();
                    objUser = new tbl_UserAdmin();
                    lstImages = _imagesRep.GetAll().Where(g => g.ImageCategoryId == item.ID).ToList();

                    if (item.CreateBy.HasValue)
                        objUser = _repUserAdmin.Find(item.CreateBy.Value);
                    if (objUser != null)
                        item.TenNguoiTao = objUser.FullName;
                    stt++;
                    iRow++;
                    ws.Cells["A" + iRow].Value = stt;
                    ws.Cells["B" + iRow].Value = item.Name;
                    ws.Cells["C" + iRow].Value = lstImages.Count();
                    ws.Cells["D" + iRow].Value = item.TenNguoiTao;
                    ws.Cells["E" + iRow].Value = HelperDateTime.ConvertDateToString(item.CreatedDate);
                    ws.Cells["F" + iRow].Value = item.NewMoney;
                    ws.Cells["F" + iRow].Style.Numberformat.Format = "#,#";

                    ws.Cells["A7" + ":F" + iRow].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":F" + iRow].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":F" + iRow].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells["A7" + ":F" + iRow].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    //Căn vị trí
                    ws.Cells["A" + iRow].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    ws.Cells["A" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["B8" + ":F" + iRow].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells["A7" + ":F" + iRow].Style.Font.Size = 11;
                    ws.Cells["A8" + ":F" + iRow].Style.WrapText = true;
                    ws.Cells["A8:F"+iRow].Style.Font.Name = "Times New Roman";

                }
                ws.Cells["E" + (Data.Count + 8) + ":F" + (Data.Count + 8)].Merge = true;
                ws.Cells[Data.Count + 8, 4].Value = "Tổng cộng: ";
                ws.Cells[Data.Count + 8, 4].Style.Font.Bold = true;
                ws.Cells[Data.Count + 8, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                ws.Cells[Data.Count + 8, 5].Formula = string.Format("=SUM(F8:F{0})", iRow);
                ws.Cells[Data.Count + 8, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells[Data.Count + 8, 5].Style.Numberformat.Format = "#,#";
                DateTime now = DateTime.Now;
                string timeStr = now.ToString("yyyyMMddHHmm");
                System.Web.HttpContext.Current.Response.Clear();
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=" + string.Format("NhuanButHinhAnh-{0}-{1}-{2}.xlsx", now.Year, now.Month, now.Day, timeStr));
                System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                System.Web.HttpContext.Current.Response.BinaryWrite(package.GetAsByteArray());
                System.Web.HttpContext.Current.Response.End();
            }
            return "";
        }
        public void GenImgHome()
        {
            ILimitRepository _limitRepository = new LimitRepository();
            var lstHomeImg = new List<tbl_ImagesCategory>();
            var objlimit = _limitRepository.GetAll().FirstOrDefault(g => g.Code.Trim() == "hinh-anh-cot-phai" && g.Status);
            lstHomeImg = _imgCategoryReporitory.GetAll().Where(g => g.IsHome && g.Status).OrderBy(g => g.Ordering).ThenByDescending(g => g.CreatedDate).Take(objlimit != null ? objlimit.Value : 3).ToList();

            string strFileName = "img_home.json";
            Common.genCommonFileJson(lstHomeImg, strFileName);
        }
    }
}
