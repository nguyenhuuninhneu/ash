using System; 
using System.Data.Entity.Validation; 
using System.Linq; 
using System.Web.Mvc; 
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class MagazineController : BaseController
    {
        readonly IMagazineRepository _magazineRepository = new MagazineRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IAdminMenuRepository _adminMenuRepository = new AdminMenuRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IFieldFileRep _fieldsFile = new FieldFileRep();
        public ActionResult Index()
        {
            var lstMagazine = _magazineRepository.GetAll().ToList();
            TempData["lstMagazine"] = lstMagazine;

            var lstMagazineUser = _magazineRepository.GetAll().Where(g => g.CreatedBy == User.ID).ToList();
            TempData["lstMagazineUser"] = lstMagazineUser;

            var FromYear = lstMagazine.OrderBy(g => g.Year).FirstOrDefault();
            if (FromYear != null) TempData["FromYear"] = FromYear.Year;
            else TempData["FromYear"] = 0;

            var ToYear = lstMagazine.OrderByDescending(g => g.Year).FirstOrDefault();
            if (ToYear != null) TempData["ToYear"] = ToYear.Year;
            else TempData["ToYear"] = 0; 

            var curl = HttpContext.Request.Url.AbsolutePath;
            var adminMenuID = 0;
            var adminMenu= _adminMenuRepository.GetAll().FirstOrDefault(g => g.Url.Trim() == curl);
            if (adminMenu != null)
            {
                adminMenuID = adminMenu.ID;
                var act = new GetAction();
                var action = act.Get(User.GroupUser, adminMenuID,User.isAdmin);
                TempData["action"] = action.ToList();
            }

            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
         
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Title,string Author,string Tag, int Status = 0,int page = 1,int CreatedBy = 0, string NgayDang = null, string NgayKet = null, int Send = 0, int Return = 0, int Nam = 0)
        {
            
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin; 
            var lstMagazine = _magazineRepository.GetAll();

           var LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var magazine in lstMagazine)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(magazine.LangCode) && magazine.LangCode == language.Code)
                        magazine.NgonNgu = language.Name;
                }
            }
            lstMagazine = lstMagazine.Where(g => g.LangCode == LangCode).ToList();
            if (!string.IsNullOrEmpty(Title))
            {
                lstMagazine =lstMagazine.Where(g =>HelperString.UnsignCharacter(g.Title.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Title.Trim().ToLower())));
            } 
            lstMagazine = lstMagazine.OrderBy(g=>g.Ordering).ThenByDescending(g => g.CreatedDate); 
            if (CreatedBy > 0)
            {
                lstMagazine = lstMagazine.Where(g => g.CreatedBy == CreatedBy);
            }
            if (!string.IsNullOrEmpty(NgayDang) && !string.IsNullOrEmpty(NgayKet))
            {

                var ngaydang = HelperDateTime.ConvertDate(NgayDang);
                var ngayket = HelperDateTime.ConvertDate(NgayKet);
                if (ngaydang > ngayket)
                {
                    Session["Messenger"] = new Notified { Value = EnumNotifield.Error, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" };
                }
                else
                {
                    lstMagazine = lstMagazine.Where(s => Convert.ToDateTime(s.CreatedDate).Date >= ngaydang && Convert.ToDateTime(s.CreatedDate).Date <= ngayket);
                }
            }
            if (Nam > 0)
            {
                lstMagazine = lstMagazine.Where(g => g.Year == Nam);
            }

            var totalMagazine = lstMagazine.ToList().Count();
            TempData["totalMagazine"] = totalMagazine; 
            var rowLimit = 20;
            lstMagazine = lstMagazine.Skip((page - 1) * rowLimit).Take(rowLimit); 
            TempData["lstMagazine"] = lstMagazine.ToList(); 
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Magazine/_ListData.cshtml", lstMagazine),
                totalPages = Math.Ceiling(((double)totalMagazine / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        { 
            var lstMagazine = _magazineRepository.GetAll().ToList();
            TempData["lstMagazine"] = lstMagazine;
            var newsDetail = _magazineRepository.Find(id);
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "tapchi").ToList();
            TempData["lstFile"] = lstFile;
            return Json(RenderViewToString("~/Areas/Admin/Views/Magazine/Detail.cshtml", newsDetail), JsonRequestBehavior.AllowGet);
             
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

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_Magazine obj,qProcess objProcess, string isContinue, double x1 = 0, double y1 = 0, double h = 0, double w = 0)
        {
            var objMagazine = _magazineRepository.GetAll().Where(x => x.Title == obj.Title.Trim()).ToList();
            if (objMagazine.Count >= 1)
            {

                return Redirect("/Admin/Magazine?sc=3");

            }
            obj.CreatedBy = User.ID; 
            var date = Request["EditCreateDate"];
            obj.CreatedDate = DateTime.Now;
            if (!string.IsNullOrEmpty(date))
            {
                obj.CreatedDate = HelperDateTime.ConvertDate(date);
            } 

            var status = Request["Status"];
            obj.Status = Convert.ToBoolean(status);

            //Thêm ngôn ngữ mặc định
            var langCode = "vn";
            if (Session["langCodeDefaut"] != null)
                langCode = Session["langCodeDefaut"].ToString();
            obj.LangCode = langCode.ToString();

            try
            {
                _magazineRepository.Add(obj);

                //GhiFile
                var arrLinkFile = Request.Form.GetValues("linkFile");
                var arrNameFile = Request.Form.GetValues("abcd"); // set ten file
                var arrSize = Request.Form.GetValues("sizeFile");
                if (arrLinkFile != null)
                {
                    int numfile = arrLinkFile.Count();
                    for (int i = 0; i < numfile; i++)
                    {
                        var linkfile = arrLinkFile[i];
                        var replaceName = arrNameFile[i];
                        var size = arrSize[i];
                        var code = "tapchi";
                        if (!string.IsNullOrEmpty(linkfile))
                        {
                            var namefile = Common.GetNameFile(linkfile.Split('-')[1]);
                            insert_filetoDb(namefile, linkfile, replaceName, obj.ID, code, size);
                        }
                    }
                }

                LogController.AddLog(string.Format("Thêm tạp chí: {0}", obj.Title), User.Username);
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
            if (isContinue == "1")
            {
                return Redirect("/Admin/Magazine/Add?sc=1");
            }
           
            return Redirect("/Admin/Magazine?sc=1");
        }
        [Authorize]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstMagazine = _magazineRepository.GetAll().ToList();
            TempData["lstMagazine"] = lstMagazine; 
            var news = _magazineRepository.Find(id); 
            TempData["news"] = news;

            //ngôn ngữ
            var languages = "vn";
            if (!string.IsNullOrEmpty(news.LangCode))
                languages = news.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == languages);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;


            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "tapchi");
            var arrLink = lstFile.Select(g => g.LinkFile);
            var arrName = lstFile.Select(g => g.ReplaceName);
            var arrSize = lstFile.Select(g => g.Size);
            TempData["group_link"] = string.Join(",", arrLink);
            TempData["group_name"] = string.Join("|", arrName);
            TempData["group_size"] = string.Join("|", arrSize);

            return View(news);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(tbl_Magazine obj,qProcess objProcess )
        { 
            var status = Request["Status"];
            obj.Status = Convert.ToBoolean(status);

            var date = Request["EditCreateDate"]; 
            if (!string.IsNullOrEmpty(date))
            {
                obj.CreatedDate = HelperDateTime.ConvertDate(date);
            } 
            try
            {
                _magazineRepository.Edit(obj);
                //Ghifile
                var code = "tapchi";
                delete_filetoDb(obj.ID,code);
                var arrLinkFile = Request.Form.GetValues("linkFile");
                var arrNameFile = Request.Form.GetValues("abcd"); // set ten file
                var arrSize = Request.Form.GetValues("sizeFile");
                if (arrLinkFile != null)
                {
                    int numfile = arrLinkFile.Count();
                    for (int i = 0; i < numfile; i++)
                    {
                        var linkfile = arrLinkFile[i];
                        var replaceName = arrNameFile[i];
                        var size = arrSize[i];
                        if (!string.IsNullOrEmpty(linkfile))
                        {
                            var namefile = Common.GetNameFile(linkfile.Split('-')[1]);
                            insert_filetoDb(namefile, linkfile, replaceName, obj.ID, code, size);
                        }
                    }
                }
                //Log người thao tác hệ thống
                LogController.AddLog(string.Format("Sửa tạp chí: {0}", obj.Title), User.Username);
            }
            catch (Exception ex)
            {
            }
          
            return Redirect("/Admin/Magazine?sc=2");
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var objPost = _magazineRepository.Find(id);
                if (objPost.CreatedBy != User.ID && User.isAdmin == false)
                {
                    return RedirectToAction("AccessDenined", "Error");
                }
                //xóa file 
                var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "vanban").ToList();
                foreach (var file in lstFile)
                {
                    //xóa db
                    _fieldsFile.DeletePermanently(file.Id);
                    //xóa sever
                    var nameFile = file.LinkFile.Substring(file.LinkFile.LastIndexOf('/') + 1, file.LinkFile.Length - file.LinkFile.LastIndexOf('/') - 1);
                    if (nameFile != "")
                    {
                        var link = Common.GetPath("~/Images/", nameFile.Split('/').Last());
                        System.IO.File.Delete(link);
                    }
                }
                _magazineRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa tạp chí: {0}", objPost.Title), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa tạp chí thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa tạp chí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var countId = arrid.Count();
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _magazineRepository.Find(Convert.ToInt32(item));
                    if(User.isAdmin)
                    {
                        //xóa file
                        var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == Convert.ToInt16(item) && g.Code == "vanban").ToList();
                        foreach (var file in lstFile)
                        {
                            //xóa db
                            _fieldsFile.DeletePermanently(file.Id);
                            //xóa server
                            var nameFile = file.LinkFile.Substring(file.LinkFile.LastIndexOf('/') + 1, file.LinkFile.Length - file.LinkFile.LastIndexOf('/') - 1);
                            if (nameFile != "")
                            {
                                var link = Common.GetPath("~/Images/", nameFile.Split('/').Last());
                                System.IO.File.Delete(link);
                            }
                        }
                        _magazineRepository.Delete(Convert.ToInt32(item));
                        count++;
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
            if (count == 0)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Bạn không có quyền xóa tin xuất bản"),
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (count < countId)
                {
                    var sub = countId - count;
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = string.Format("Xóa thành công {0} tin bài, còn {1} tin xuất bản", count, sub),
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = string.Format("Xóa thành công {0} tin bài", count),
                }, JsonRequestBehavior.AllowGet);
            }
        } 

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _magazineRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _magazineRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = _magazineRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _magazineRepository.Edit(obj);
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
        public void insert_filetoDb(string namefile = "", string linkfile = "", string replaceName = "", int FieldID = 0, string code = "vanban", string size = "")
        {
            if (string.IsNullOrEmpty(replaceName))
                replaceName = namefile;
            var fobj = new tbl_fieldfiles();
            fobj.NameFile = namefile;
            fobj.LinkFile = linkfile;
            fobj.ReplaceName = replaceName;
            fobj.Size = size;
            fobj.FieldID = FieldID;
            fobj.Code = code;
            fobj.Status = 1;
            fobj.CreateDate = DateTime.Now;
            _fieldsFile.Create(fobj);
        }
        public void delete_filetoDb(int FieldID = 0, string code = "tapchi")
        {
            if (FieldID > 0)
            {
                var allFileByField = _fieldsFile.GetData().Where(g => g.FieldID == FieldID && g.Code == code).Select(g => g.Id).ToList();
                if (allFileByField != null)
                {
                    var itemId = 0;
                    foreach (var item in allFileByField)
                    {
                        itemId = Convert.ToInt32(item);
                        _fieldsFile.DeletePermanently(itemId);
                    }
                }
            }
        }
    }
}
