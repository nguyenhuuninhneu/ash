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
    public class TopicController : BaseController
    {
        readonly ITopicRepository _topicRepository = new TopicRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        readonly IAdminMenuRepository _adminMenuRepository = new AdminMenuRepository();
        readonly IqProcedureRepository _qProcedureRepository = new qProcedureRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IFieldFileRep _fieldsFile = new FieldFileRep();

        public ActionResult Index()
        {
            var lstTopic = _topicRepository.GetAll().ToList();
            TempData["lstTopic"] = lstTopic;

            var lstTopicUser = _topicRepository.GetAll().Where(g => g.CreatedBy != null && g.CreatedBy == User.ID).ToList();
            TempData["lstTopicUser"] = lstTopicUser;
             
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
        public ActionResult ListData(string Title,string Author,string Tag, int Status = 0,int page = 1,int CreatedBy = 0, string NgayDang = null, string NgayKet = null, int Send = 0, int Return = 0)
        {
            var objqprocedure = _qProcedureRepository.GetAll().FirstOrDefault(g => g.isPublish);
            TempData["objqprocedure"] = objqprocedure;
            var objUser = _userAdminRepository.Find(User.ID);
            TempData["objUser"] = objUser;

            var lstTopic = _topicRepository.GetAll();
          var  LangCode = "vn";
            if (Session["langCodeDefaut"] != null)
                LangCode = Session["langCodeDefaut"].ToString();
            //hiển thị tên ngôn ngữ
            var lstLanguages = _languagesRepository.GetAll().ToList();
            foreach (var topic in lstTopic)
            {
                foreach (var language in lstLanguages)
                {
                    if (!string.IsNullOrEmpty(topic.LangCode) && topic.LangCode == language.Code)
                        topic.NgonNgu = language.Name;
                }
            }
            lstTopic = lstTopic.Where(g => g.LangCode == LangCode).ToList();
            if (!string.IsNullOrEmpty(Title))
            {
                lstTopic =lstTopic.Where(g =>HelperString.UnsignCharacter(g.Title.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Title.Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(Tag))
            {
                lstTopic =lstTopic.Where(g =>g.Tags!=null&&HelperString.UnsignCharacter(g.Tags.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Tag.Trim().ToLower())));
            }
            if (!string.IsNullOrEmpty(Author))
            {
                lstTopic = lstTopic.Where(g =>g.Author!=null&&HelperString.UnsignCharacter(g.Author.Trim().ToLower()).Contains(HelperString.UnsignCharacter(Author.Trim().ToLower())));
            }
            lstTopic = lstTopic.OrderByDescending(g => g.CreatedDate);
            
            if (CreatedBy > 0)
            {
                lstTopic = lstTopic.Where(g => g.CreatedBy == CreatedBy);
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
                    lstTopic = lstTopic.Where(s => s.CreatedDate.Date >= ngaydang && s.CreatedDate.Date <= ngayket);
                }
            }
            var totalTopic = lstTopic.ToList().Count();
            TempData["totalTopic"] = totalTopic;
            //var rowLimit = Webconfig.RowLimit;
            var rowLimit = 20;
            lstTopic = lstTopic.Skip((page - 1) * rowLimit).Take(rowLimit);
            //Quy trình xuất bản tin
            TempData["lstTopic"] = lstTopic.ToList();
           
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Topic/_ListData.cshtml", lstTopic),
                totalPages = Math.Ceiling(((double)totalTopic / rowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int id)
        {
          
            var lstTopic = _topicRepository.GetAll().ToList();
            TempData["lstTopic"] = lstTopic;
            var newsDetail = _topicRepository.Find(id);
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "topic").ToList();
            TempData["lstFile"] = lstFile;
            return Json(RenderViewToString("~/Areas/Admin/Views/Topic/Detail.cshtml", newsDetail), JsonRequestBehavior.AllowGet);
             
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
                TempData["languages"] = objLanguages.Name;

            var objxuatban = _qProcedureRepository.GetAll().FirstOrDefault(g => g.isPublish);
            var lstUser = _userAdminRepository.GetAll().Where(g => g.Active && !g.isAdmin && !g.QuyTrinhXuatBanID.Contains(objxuatban.ID.ToString())&&g.LangCode==langCode).ToList();
            TempData["lstUser"] = lstUser;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Add")]
        public ActionResult Add(tbl_Topic obj,qProcess objProcess, string isContinue, double x1 = 0, double y1 = 0, double h = 0, double w = 0)
        {   
            obj.CreatedBy = User.ID; 
            var date = Request["EditCreateDate"];
            obj.CreatedDate = DateTime.Now;
            if (!string.IsNullOrEmpty(date))
            {
                obj.CreatedDate = HelperDateTime.ConvertDate(date);
            }
            var startdate = Request["EditStartDate"]; 
            if (!string.IsNullOrEmpty(startdate))
            {
                obj.StartDate = HelperDateTime.ConvertDate(startdate);
            }

            var status = Request["Status"];
            obj.Status = Convert.ToBoolean(status);

            var lstUser = Request["IdNguoiChon"];
            obj.IdNguoiChon = lstUser;

            try
            {
                //Thêm ngôn ngữ mặc định
                var langCode = "vn";
                if (Session["langCodeDefaut"] != null)
                    langCode = Session["langCodeDefaut"].ToString();
                obj.LangCode = langCode.ToString();

                _topicRepository.Add(obj);
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
                        var code = "video";
                        if (!string.IsNullOrEmpty(linkfile))
                        {
                            var namefile = Common.GetNameFile(linkfile.Split('-')[1]);
                            insert_filetoDb(namefile, linkfile, replaceName, obj.ID, code, size);
                        }
                    }
                }
                LogController.AddLog(string.Format("Thêm bài viết: {0}", obj.Title), User.Username);
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
                return Redirect("/Admin/Topic?sc=1");
            }
           
            return Redirect("/Admin/Topic?sc=1");
        }
        [Authorize]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstTopic = _topicRepository.GetAll().ToList();
            TempData["lstTopic"] = lstTopic; 
            var news = _topicRepository.Find(id); 
            TempData["news"] = news;

            //ngôn ngữ
            var langCode = "vn";
            if (!string.IsNullOrEmpty(news.LangCode))
                langCode = news.LangCode;
            var objLanguages = _languagesRepository.GetAll().FirstOrDefault(g => g.Code == langCode);
            if (objLanguages != null)
                TempData["languages"] = objLanguages.Name;

            //get file
            var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "topic");
            var arrLink = lstFile.Select(g => g.LinkFile);
            var arrName = lstFile.Select(g => g.ReplaceName);
            var arrSize = lstFile.Select(g => g.Size);
            TempData["group_link"] = string.Join(",", arrLink);
            TempData["group_name"] = string.Join("|", arrName);
            TempData["group_size"] = string.Join("|", arrSize);
            var objxuatban = _qProcedureRepository.GetAll().FirstOrDefault(g => g.isPublish);
            var lstUser = _userAdminRepository.GetAll().Where(g => g.Active && !g.isAdmin && !g.QuyTrinhXuatBanID.Contains(objxuatban.ID.ToString())&&g.LangCode==langCode).ToList();
            TempData["lstUser"] = lstUser;
            return View(news);
        }

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(tbl_Topic obj,qProcess objProcess )
        {
            obj.ModifyDate = DateTime.Now;
            obj.ModifyBy = User.ID; 

            var status = Request["Status"];
            obj.Status = Convert.ToBoolean(status);

            var lstUser = Request["IdNguoiChon"];
            obj.IdNguoiChon = lstUser;

            var date = Request["EditCreateDate"]; 
            if (!string.IsNullOrEmpty(date))
            {
                obj.CreatedDate = HelperDateTime.ConvertDate(date);
            }
            var startdate = Request["EditStartDate"];
            if (!string.IsNullOrEmpty(startdate))
            {
                obj.StartDate = HelperDateTime.ConvertDate(startdate);
            }

            try
            {
                _topicRepository.Edit(obj);
                //Ghifile
                var code = "topic";
                delete_filetoDb(obj.ID, code);
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
                LogController.AddLog(string.Format("Sửa bài viết: {0}", obj.Title), User.Username);
            }
            catch (Exception ex)
            {
            }
          
            return Redirect("/Admin/Topic?sc=2");
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var objPost = _topicRepository.Find(id);
                if (objPost.CreatedBy != User.ID && User.isAdmin == false)
                {
                    return RedirectToAction("AccessDenined", "Error");
                }
                var arrCMT = _topicRepository.GetAllCMT().Where(g => g.TopicID == id).Select(g => g.ID).ToArray();
                var strCMT = string.Join(",", arrCMT);
                DeleteAllCMT(strCMT);
                //xóa file 
                var lstFile = _fieldsFile.GetData().Where(g => g.FieldID == id && g.Code == "vanban").ToList();
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
                _topicRepository.Delete(id);
                LogController.AddLog(string.Format("Xóa bài viết: {0}", objPost.Title), User.Username);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa bài viết thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa bài viết thành công",
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
                    var obj = _topicRepository.Find(Convert.ToInt32(item));
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
                        _topicRepository.Delete(Convert.ToInt32(item));
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
        
        [Authorize(Roles = "Index")]
        public ActionResult Reply(int commentId)
        {
            ViewBag.CommentId = commentId;
            return View();
        }
        public ActionResult ListDataReply(string TuNgay, string DenNgay, int id, int page = 1)
        {
            TempData["cmtIDz"] = id;
            var obj = new tbl_TopicComment();
            TempData["cmtName"] = _topicRepository.FindCMT(id).NoiDung; 
            var TopicId = _topicRepository.FindCMT(id).TopicID;
            TempData["cmtID"] = TopicId;
            TempData["topicName"] = _topicRepository.Find(TopicId).Title; 

            var lstData = _topicRepository.GetAllCMT();
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
                viewContent = RenderViewToString("~/Areas/Admin/Views/Topic/ListDataReply.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailsReply(int id)
        {
            var obj = _topicRepository.FindCMT(id);
            TempData["objCMT"] = _topicRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/Topic/DetailsReply.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DetailsReply(tbl_TopicComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                _topicRepository.EditCMT(obj);

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
            var objcmt = _topicRepository.FindCMT(id);
            TempData["objcmt"] = objcmt;
            return Json(RenderViewToString("~/Areas/Admin/Views/Topic/AddRepCmt.cshtml"),JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddRepCmt(tbl_TopicComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                obj.ThoiGianTraLoi = DateTime.Now;
                obj.IsView = true; 
                obj.IsNew = false; 
                _topicRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới trả lời thành công",
                    cmtid = obj.TopicID
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
        [Authorize(Roles = "Index")]
        public ActionResult ListDataCmt(int id, int page = 1)
        {
            TempData["cmtIDz"] = id;
            var lstData = _topicRepository.GetAllCMT().Where(g => g.CommentID == id).ToList();

            //TempData["Title"] = _newsRepository.Find(id).ID;
            var total = lstData.Count();
            lstData = lstData.OrderByDescending(g => g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Topic/ListDataRep.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id, string str = "Status")
        {
            var obj = _topicRepository.Find(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _topicRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckReply(int id)
        {
            var obj = _topicRepository.FindCMT(id);
            if (obj.IdNguoiChon == 0 || obj.IdNguoiChon == null)
            {
                obj.IdNguoiChon = User.ID; 
                _topicRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    check = "true",
                    FullName = "",
                    userother = true,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (obj.IdNguoiChon == User.ID)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        check = "true",
                        FullName = "",
                        userother = false,
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var fullname = _userAdminRepository.Find(Convert.ToInt32(obj.IdNguoiChon)); 
                    return Json(new
                    {
                        IsSuccess = true,
                        check = "false",
                        FullName = fullname.FullName,
                        userother = false,
                    }, JsonRequestBehavior.AllowGet);
                } 
            } 
        }
        public ActionResult DestroyChoose(int id)
        {
            var obj = _topicRepository.FindCMT(id);
            obj.IdNguoiChon = null;
            try
            {
                _topicRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false
                }, JsonRequestBehavior.AllowGet);
            } 
        }
        public ActionResult Choose(int id)
        { 
            string message = "";
            var obj = _topicRepository.FindCMT(id);
            if (obj.IdNguoiChon == null)
            {
                obj.IdNguoiChon = User.ID;
                message = "Lựa chọn câu hỏi thành công!";
            }
            else
            {
                if (obj.IdNguoiChon == User.ID)
                {
                    obj.IdNguoiChon = null;
                    message = "Hủy lựa chọn câu hỏi thành công!";
                }
                else
                {
                    message = _userAdminRepository.GetAll().FirstOrDefault(g => g.ID == User.ID).FullName +
                              " đang chọn.";
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = message
                    }, JsonRequestBehavior.AllowGet);
                } 
            } 
            try
            { 
                _topicRepository.EditCMT(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                message = "Chọn/Hủy chọn thất bại!";
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = message,
                }, JsonRequestBehavior.AllowGet);
            } 
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
        public void delete_filetoDb(int FieldID = 0, string code = "topic")
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
        #region Comment
        [Authorize(Roles = "View")]
        public ActionResult Comment(int TopicID)
        {
            ViewBag.TopicID = TopicID;
            return View();
        }
        [Authorize(Roles = "View")]
        public ActionResult ListDataComment(string TuNgay, string DenNgay, int id, int page = 1)
        {
            var lstData = _topicRepository.GetAllCMT();
            TempData["tieude"] = _topicRepository.Find(id).Title;
            TempData["cmtid"] = _topicRepository.Find(id).ID;
            var lstUserAdmin = _userAdminRepository.GetAll().ToList();
            TempData["lstUserAdmin"] = lstUserAdmin;
            TempData["UserCurrent"] = _userAdminRepository.Find(User.ID);

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
                lstData = lstData.Where(g => g.TopicID == id);
            }
            TempData["Topic"] = _topicRepository.GetAll().ToList();
            var total = lstData.Count();
            lstData = lstData.OrderBy(g => g.IsView).ThenBy(g=>g.IdNguoiChon).ThenByDescending(g=>g.CreateDate).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Topic/ListDataComment.cshtml", lstData),
                totalPages = Math.Ceiling(((double)total / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "View")]
        public ActionResult LoadComment(int id)
        { 
            var lstData = _topicRepository.GetAllCMT().Where(g=>g.IsNew && (g.CommentID == 0 || g.CommentID == null) && g.TopicID == id).OrderBy(g=>g.CreateDate).ToList();
            foreach (var item in lstData)
            {
                item.IsNew = false;
                _topicRepository.EditCMT(item);
            } 

            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Topic/LoadComment.cshtml", lstData),
                Checknull = (lstData.Count > 0 ? true : false),
            }, JsonRequestBehavior.AllowGet); 
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailsComment(int id)
        {
            var obj = _topicRepository.FindCMT(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Topic/DetailsComment.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "View")]
        public ActionResult DetailsRep(int id)
        {
            var obj = _topicRepository.FindCMT(id);
            TempData["objCMT"] = _topicRepository.FindCMT(Convert.ToInt32(obj.CommentID));
            return Json(RenderViewToString("~/Areas/Admin/Views/Topic/DetailsRep.cshtml", obj), JsonRequestBehavior.AllowGet);
        }

        //[Authorize(Roles = "View")]
        [HttpPost]
        public ActionResult DetailsRep(tbl_TopicComment obj)
        {
            try
            {
                obj.CreatedBy = User.ID;
                _topicRepository.EditCMT(obj);
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

        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatusCMT(int id, string str = "Status")
        {
            var obj = _topicRepository.FindCMT(id);
            var cvalue = Convert.ToBoolean(Common.GetValueByName(obj, str));
            var propertyInfo = obj.GetType().GetProperty(str);
            if (propertyInfo != null) propertyInfo.SetValue(obj, !cvalue);
            _topicRepository.EditCMT(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ChangIsView(int id)
        {
            var obj = _topicRepository.FindCMT(id);
            obj.IsView = true;
            _topicRepository.EditCMT(obj);
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
                    var obj = _topicRepository.FindCMT(Convert.ToInt32(item));
                    obj.Status = true;
                    _topicRepository.EditCMT(obj);
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
                var objPost = _topicRepository.FindCMT(id);
                // ten bai viet
                var tenbaiviet = _topicRepository.Find(objPost.TopicID).Title;
                //
                var arrRep = _topicRepository.GetAllCMT().Where(g => g.CommentID == id).Select(g => g.ID).ToArray();
                var strRep = string.Join(",", arrRep);
                DeleteAllRep(strRep);
                _topicRepository.DeleteCMT(id);

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
        public ActionResult DeleteAllRep(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var obj = _topicRepository.FindCMT(Convert.ToInt32(item));
                    // ten bai viet
                    var tenbaiviet = _topicRepository.Find(obj.TopicID).Title;
                    //
                    _topicRepository.DeleteCMT(Convert.ToInt32(item));
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
                    var obj = _topicRepository.FindCMT(Convert.ToInt32(item));
                    // ten bai viet
                    var tenbaiviet = _topicRepository.Find(obj.TopicID).Title;
                    //
                    _topicRepository.DeleteCMT(Convert.ToInt32(item));
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
