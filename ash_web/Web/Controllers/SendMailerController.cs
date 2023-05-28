using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Net.Mail;
using Web.Core;
using Web.Repository;
using Web.Repository.Entity;

namespace PublicService.Controllers
{
    public class SendMailerController : Controller
    {
        private readonly IUserRepository _userRepository = new UserRepository();
        private readonly IQAReporitory _qaReporitory = new QAReporitory();
        readonly IContactReporitory _contactReporitory = new ContactReporitory();
        private readonly IToGiacRepository _toGiacRepository = new ToGiacRepository();
        private readonly IVanBanRepository _vanbanResponsitory = new VanBanRepository();
        private string SITE_URL = ConfigurationManager.ConnectionStrings["SITE_URL"].ToString();
        private string DOMAIN_NAME = ConfigurationManager.ConnectionStrings["DOMAIN_NAME"].ToString();
        public ActionResult SendEmailVz(string UserEmail, string Subject, string Body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(UserEmail);
            mail.From = new MailAddress("lienminhhtx.info@gmail.com");
            mail.Subject = Subject;
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("lienminhhtx.info@gmail.com", "lienminhhtx");// tài khoản Gmail của bạn      
            smtp.EnableSsl = true;
            smtp.Send(mail);
            return null;
        }
        // GET: /SendMailer/  
        public ActionResult Index(string Email)
        {
            if (ModelState.IsValid && Email != null)
            {
                var newpass = RandomString(8);
                var objUser = _userRepository.GetAll().FirstOrDefault(g => Email != null && g.Email.ToLower() == Email.ToLower());
                objUser.Password = HelperEncryptor.Md5Hash(newpass);
                _userRepository.Edit(objUser);
                var Subject = "Thông báo khởi tạo mật khẩu mới tại Liên minh HTX Quảng Trị";
                string Body = "";
                Body += "<p>Xin chào " + "<b>" + objUser.FullName + "</b>" + "," + "</p>";
                Body += "<p>Bạn đã yêu cầu tạo mới mật khẩu của mình.</p>";
                Body += "<p>Tài khoản của bạn: " + "<b>" + objUser.UserName + "</b>" + "</p>";
                Body += "<p>Mật khẩu mới: " + "<b>" + newpass + "</b>" + "</p>";
                Body += "<p>Mời bạn click <a href=\"" + SITE_URL + "/User/Login" + "\" target='_blank'>vào đây</a> để đăng nhập và đổi mật khẩu.</p>";
                Body += "<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>";
                SendEmailVz(Email, Subject, Body);
            }
            return null;
        }
        public ActionResult SendMailVanBan(string Email, string lstid,string tenchude,string noidung)
        {
            if (ModelState.IsValid && Email != null)
            {
                var topfirst = "border-top: 1px solid black; border-left:1px solid black;";
                var toplast = "border-top: 1px solid black; border-left:1px solid black; border-right:1px solid black;";

                var bottomfirst = "border-top: 1px solid black; border-left:1px solid black; border-bottom:1px solid black;";
                var bottomlast = "border:1px solid black;";

                var Subject=tenchude;
                if (string.IsNullOrEmpty(tenchude))
                {
                    Subject = "Thông báo văn bản trên trang Công đoàn Bộ Y Tế Việt Nam";
                }
                string Body = "";
                Body += noidung;
                if(string.IsNullOrEmpty(noidung))
                {
                    Body += "<p>Xin chào," + "</p>";
                    Body += "<p>Bạn nhận được văn bản trên trang Công đoàn Bộ Y Tế Việt Nam.</p>";
                    Body += "<p><b>Thông tin văn bản:</b></p>";
                }
                
                Body += "<table cellspacing='0' cellpadding='10' style='width:100%;'><tr><th style='width:20px;"+topfirst+ "'>STT</th><th style='width:100px;" + topfirst + "'>Số hiệu</th><th style='" + topfirst + "'>Trích yếu</th><th style='width:80px;" + topfirst + "'>File</th><th style='width:80px;" + toplast + "'>Xem chi tiết</th></tr>";
                if (!string.IsNullOrEmpty(lstid))
                {
                    var lstVanban = _vanbanResponsitory.GetAll().Where(g => g.Status).ToList();
                    var arrItem = lstid.Split(',');
                    if (arrItem.Length > 0)
                    {
                        for (int i = 0; i < arrItem.Length; i++)
                        {
                            var objVanBan = lstVanban.FirstOrDefault(g => g.ID == Convert.ToInt32(arrItem[i]));
                            objVanBan.IsSendMail = true;
                            _vanbanResponsitory.Edit(objVanBan);
                            if (objVanBan != null)
                            {
                                if(i == arrItem.Length - 1)
                                {
                                    topfirst = bottomfirst;
                                    toplast = bottomlast;
                                }
                                Body += "<tr><td style='" + topfirst + "'>" + (i + 1) + "</td><td style='" + topfirst + "'>" + objVanBan.SoHieu + "</td><td style='" + topfirst + "'>" + objVanBan.TrichYeu + "</td><td style='" + topfirst + "'><a href='" + SITE_URL + objVanBan.Attachment + "'>Tải về</a></td><td style='" + toplast + "'><a href='" + SITE_URL + "VanBanFE/details/" + objVanBan.ID + "' target='_blank'>vào đây</a></td></tr>";
                            }
                        }
                    }
                }
                Body += "</table>";
                Body += "<p>Thời gian: " + string.Format("{0: dd/MM/yyyy HH:mm}", DateTime.Now) + "</p>";
                //Body += "<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.</p>";
                SendEmailVz(Email, Subject, Body);
            }
            return null;
        }
        public ActionResult Reply(string Email)
        {
            if (ModelState.IsValid && Email != null)
            {
                var obj = _qaReporitory.GetAll().OrderByDescending(g => g.ID).FirstOrDefault(g => Email != null && g.Email.ToLower() == Email.ToLower());
                var Subject = "Thông báo phản hồi tại " + DOMAIN_NAME + "";
                string Body = "";
                Body += "<p>Xin chào " + "<b>" + obj.FullNameOfQuestion + "</b>" + "," + "</p>";
                Body += "<p>Nội dung câu hỏi: </p>";
                Body += "<p>" + obj.Question + "</p>";
                Body += "<p>Trả lời: </p>";
                Body += "<p>" + obj.Answer + "</p>";
                Body += "<p>Cảm ơn bạn đã gửi câu hỏi đến chúng tôi chúng tôi.</p>";
                SendEmailVz(Email, Subject, Body);
            }
            return null;
        }
        public ActionResult DongGopYKien(string Email)
        {
            if (ModelState.IsValid && Email != null)
            {
                var obj = _qaReporitory.GetAll().OrderByDescending(g => g.ID).FirstOrDefault();
                var Subject = "Thông báo đóng góp ý kiến tại " + DOMAIN_NAME + "";
                string Body = "";
                Body += "<p>Xin chào," + "</p>";
                Body += "<p>Vừa có thư đóng góp ý kiến tại <a href=\"" + SITE_URL + "\">" + DOMAIN_NAME + "</a>";
                Body += "<p>Họ tên: " + obj.FullNameOfQuestion + "</p>";
                Body += "<p>Điện thoại: " + obj.Phone + "</p>";
                Body += "<p>Địa chỉ: " + obj.DiaChi + "</p>";
                Body += "<p>Email: " + obj.Email + "</p>";
                Body += "<p>Thời gian: " + string.Format("{0: dd/MM/yyyy HH:mm}", obj.CreatedDate) + "</p>";
                Body += "<p>Nội dung: </p>";
                Body += "<p>" + obj.Question + "</p>";
                SendEmailVz(Email, Subject, Body);
            }
            return null;
        }
        public ActionResult DongGopYKienLienhe(string Email)
        {
            if (ModelState.IsValid && Email != null)
            {
                var obj = _contactReporitory.GetAll().OrderByDescending(g => g.ID).FirstOrDefault();
                var Subject = "Thông báo đóng góp ý kiến tại " + DOMAIN_NAME + "";
                string Body = "";
                Body += "<p>Xin chào," + "</p>";
                Body += "<p>Vừa có thư đóng góp ý kiến tại <a href=\"" + SITE_URL + "\">" + DOMAIN_NAME + "</a>";
                Body += "<p>Họ tên: " + obj.FullNameOfQuestion + "</p>";
                Body += "<p>Điện thoại: " + obj.Phone + "</p>";
                Body += "<p>Địa chỉ: " + obj.DiaChi + "</p>";
                Body += "<p>Email: " + obj.Email + "</p>";
                Body += "<p>Thời gian: " + string.Format("{0: dd/MM/yyyy HH:mm}", obj.CreatedDate) + "</p>";
                Body += "<p>Nội dung: </p>";
                Body += "<p>" + obj.Question + "</p>";
                SendEmailVz(Email, Subject, Body);
            }
            return null;
        }
        public ActionResult ToGiacToiPham(string Email)
        {
            if (ModelState.IsValid && Email != null)
            {
                var obj = _toGiacRepository.GetAll().OrderByDescending(g => g.ID).FirstOrDefault();
                var Subject = "Thông báo gửi tố giác tội phạm tại " + DOMAIN_NAME + "";
                string Body = "";
                Body += "<p>Xin chào," + "</p>";
                Body += "<p>Vừa có thư tố giác tội phạm tại <a href=\"" + SITE_URL + "\">" + DOMAIN_NAME + "</a>";
                Body += "<p>Họ tên: " + obj.FullName + "</p>";
                Body += "<p>Điện thoại: " + obj.Phone + "</p>";
                Body += "<p>Địa chỉ: " + obj.DiaChi + "</p>";
                Body += "<p>Email: " + obj.Email + "</p>";
                Body += "<p>Thời gian: " + string.Format("{0: dd/MM/yyyy HH:mm}", obj.CreateDate) + "</p>";
                Body += "<p>Nội dung: </p>";
                Body += "<p>" + obj.NoiDung + "</p>";
                SendEmailVz(Email, Subject, Body);
            }
            return null;
        }
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}