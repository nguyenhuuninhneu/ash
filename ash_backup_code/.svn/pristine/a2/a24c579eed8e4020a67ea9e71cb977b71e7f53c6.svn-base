using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;
using System.Web.Script.Serialization;
using System.Text;

namespace Web.Areas.Admin.Controllers
{
    public class CategoryAdminController : BaseController
    {
        readonly ICategoryRepository _categoryRepository = new CategoryRepository();
        readonly IPositionRepository _positionRepository = new PositionRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        const string Keycache = "keycategory";
        //
        // GET: /Category/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
          
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData( int page = 1,string Name="")
        {
            var lstLevel = new List<tbl_Category>();
            var lstPos = _positionRepository.GetAll().ToList();
            TempData["lstPos"] = lstPos;
            var lstCategory = _categoryRepository.GetAll().OrderBy(g => g.Ordering).ToList();

            var LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var cate in lstCategory)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(cate.LangCode) && cate.LangCode == language.Code)
                        cate.NgonNgu = language.Name;
                }
            }
            lstCategory = lstCategory.Where(g => g.LangCode == LangCode).ToList();
            foreach (var item in lstCategory)
            {
                var objParent = lstCategory.FirstOrDefault(g => g.ID == item.ParentID);
                if (objParent != null)
                {
                    item.ParentName = objParent.Name;
                }
            }
            if (!string.IsNullOrEmpty(Name))
            {
                lstCategory =
                    lstCategory.Where(
                        g =>
                            HelperString.UnsignCharacter(g.Name.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Name.ToLower().Trim()))).ToList();
                //foreach (var cat in lstCategory)
                //{
                //    AddParent(cat, lstCategory, _categoryRepository.GetAll().ToList());
                //}
                //lstCategory.AddRange(_lstOtherCategory);
                var lst = lstCategory.Where(x => x.ParentID == 0).ToList();
                if(lst.Count>0)
                    lstLevel = Common.CreateLevel(lstCategory.ToList());
                else
                lstLevel = lstCategory;
            }
            else
            {
                  lstLevel = Common.CreateLevel(lstCategory.ToList());
            }
           

            var totalCategory = lstCategory.Count;
            TempData["totalCate"]= totalCategory;
            lstLevel = lstLevel.Skip((page - 1) *20).Take(20).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/CategoryAdmin/_ListData.cshtml", lstLevel),
                totalPages = Math.Ceiling(((double)totalCategory / 20)),
            }, JsonRequestBehavior.AllowGet);
        }

        readonly List<tbl_Category> _lstOtherCategory = new List<tbl_Category>();
        [Authorize(Roles = "Index")]
        [HttpPost]
     
        // tìm con của danh mục
        public void FindChild(view_Category_Languages catParent, ref List<view_Category_Languages> lstCategoryOrder, List<view_Category_Languages> lstfull)
        {
            var lstChild = lstfull.Where(g => g.ParentID == catParent.ID).ToList();
            lstCategoryOrder.Add(catParent);
            if (!lstChild.Any()) return;
            foreach (var item in lstChild)
            {
                FindChild(item, ref lstCategoryOrder, lstfull);
            }
        }
        public ActionResult GetCatbyLang(string langcode)
        {
            var lang = Request.QueryString["lang"] ?? Webconfig.LangCodeVn;
            var lstCategory = _categoryRepository.GetAll();
            if (!string.IsNullOrEmpty(lang))
            {
                lstCategory = lstCategory.Where(g => g.LangCode == lang).ToList();
            }
            var lstLevel = Common.CreateLevel(lstCategory.ToList());
            return Json(lstLevel, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            //ngôn ngữ
            var languages = "vn";
            if (Session["langCodeDefaut"] != null)
                languages = Session["langCodeDefaut"].ToString();
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            var listPosByCode = _positionRepository.GetListPosByCode("category");
            TempData["LstPos"] = listPosByCode.Where(p=>p.Status).ToList();
            var lstCategory = Common.CreateLevel(_categoryRepository.GetAll().Where(g=>g.LangCode==languages).OrderBy(g=>g.Ordering).ToList());
            TempData["Category"] = lstCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/CategoryAdmin/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_Category obj)
        {
            try
            {
                var findObj = _categoryRepository.GetAll().Where(x => x.Name == obj.Name).ToList();
                if (findObj.Count >= 1)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Tên danh mục đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                var requet = Request["Position"];
                obj.Position = requet;
                obj.CreatedDate = DateTime.Now;
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                _categoryRepository.Add(obj);
                LogController.AddLog(string.Format("Thêm mới danh mục: {0}", obj.Name), User.Username);
                genFileJson(0);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            
            var listPosByCode = _positionRepository.GetListPosByCode("category");
            TempData["LstPos"] = listPosByCode.Where(p => p.Status).ToList();
            var objCategory = _categoryRepository.Find(id);
            //ngôn ngữ
            var languages = "vn";
            if (!string.IsNullOrEmpty(objCategory.LangCode))
                languages = objCategory.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            var lstCategory = Common.CreateLevel(_categoryRepository.GetAll().Where(g => g.LangCode == languages && g.ID != id).ToList());
            TempData["Category"] = lstCategory.ToList();

           

            return Json(RenderViewToString("~/Areas/Admin/Views/CategoryAdmin/_Edit.cshtml", objCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Category obj)
        {
            try
            {
                var cate = _categoryRepository.Find(obj.ID);
                var requet = Request["Position"];
                obj.Position = requet;
                obj.CreatedDate = cate.CreatedDate;
                obj.isMenu = cate.isMenu;
                _categoryRepository.Edit(obj);
                LogController.AddLog(string.Format("Cập nhật danh mục: {0}", obj.Name), User.Username);
                genFileJson(0);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục thành công",
                }, JsonRequestBehavior.AllowGet);                
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _categoryRepository.Find(id);
                _categoryRepository.Delete(id);
                genFileJson(0);
                LogController.AddLog(string.Format("Xóa danh mục: {0}", obj.Name), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục thành công",
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
                    _categoryRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            genFileJson(0);
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục", count),
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
                var obj = _categoryRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _categoryRepository.Edit(obj);                    
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
            genFileJson(0);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MegaMenu()
        {
            var lstCatgory = HelperCache.GetCache<List<view_Category_Languages>>(Keycache);
            if (lstCatgory == null)
            {
                lstCatgory = _categoryRepository.GetAllCategoryLanguageses().Where(g => g.Active).ToList();
            }

            return View(lstCatgory);
        }
        readonly List<int> _lstDic = new List<int>();
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _categoryRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _categoryRepository.Edit(obj);
            genFileJson(0);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public void AddParent(tbl_Category cat, List<tbl_Category> lstCate, List<tbl_Category> lstAllCate)
        {
            if (_lstDic.Contains(cat.ID) || cat.ParentID == 0) return;
            _lstDic.Add(cat.ID);
            var parent = lstCate.Find(g => g.ID == cat.ParentID);
            //Nếu không có
            if (parent != null) return;
            parent = lstAllCate.Find(g => g.ID == cat.ParentID);
            _lstOtherCategory.Add(parent);
            AddParent(parent, lstCate, lstAllCate);
        }

        public void genFileJson(int parent = 0)
        {
            var file = Common.GetPath("~/Content/cate.json");
            var result = getChild();
            JavaScriptSerializer jsonseria = new JavaScriptSerializer();
            var data = jsonseria.Serialize(result);
            System.IO.File.WriteAllText(file, data, Encoding.UTF8);
        }

        public List<tbl_CategoryJson> getChild(int parent = 0)
        {
            var result = new List<tbl_CategoryJson>();
            var lstCate = _categoryRepository.GetAll().Where(g=>g.ParentID == parent && g.Active == true).OrderBy(g=>g.Ordering).ToList();
            foreach (var item in lstCate)
            {
                var obj = new tbl_CategoryJson();
                obj.cate = item;
                obj.listChildren = getChild(item.ID);
                result.Add(obj);
            }
            return result;
        }
    }
}
