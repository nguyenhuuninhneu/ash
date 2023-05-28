using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository.Entity;
using Web.Repository;

namespace SignalRChat
{ 
    public class ChatHub : Hub
    { 
        readonly IChatRepository _chatRepository = new ChatRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        public void Send(string name, string message)
        {
            if (message.Contains("www.youtube.com/watch?v="))
            {
                string code = message.Split('&')[0];
                code = code.Split('=')[1];
                code = "https://www.youtube.com/embed/" + code;
                message = "<iframe style='width:100%' src='"+ code + "' allow='autoplay; encrypted-media' allowfullscreen></iframe>";
            }
            else if (message.StartsWith("http") || message.StartsWith("https") || message.StartsWith("www"))
            {
                message = "<p style='cursor: pointer;text-decoration: underline' onclick=window.open('" + message + "','_blank')>"+ message + "<p>";
            } 

            string[] stringSplit = new string[] {"|||"};
            var fullname = name.Split(stringSplit, StringSplitOptions.None)[0]; 
            var id = name.Split(stringSplit, StringSplitOptions.None)[1];
            var id1 = name.Split(stringSplit,StringSplitOptions.None)[3];
            var time = name.Split(stringSplit, StringSplitOptions.None)[5];

            var count = 1;
            var now = DateTime.Now.Date;
            var objChat = _chatRepository.GetAll().FirstOrDefault(g => g.Date == now && g.ListIdUser != null && g.ListIdUser.Split(',').Contains(id) && g.ListIdUser.Split(',').Contains(id1));
            var newMsg = new tbl_Message()
            {
                ID = Guid.NewGuid(),
                Message = message, 
                IdUser = Convert.ToInt32(id),
                Time = time,
                Name = fullname,
            };
            List<tbl_Message> lstContent;
            if (objChat != null)
            {
                lstContent = JsonConvert.DeserializeObject<List<tbl_Message>>(objChat.Content);
                lstContent.Add(newMsg);
                var Content = JsonConvert.SerializeObject(lstContent, Formatting.Indented);
                objChat.Content = Content;
                objChat.IdLast = Convert.ToInt32(id);
                count = objChat.Count + 1;
                objChat.Count = count;
                _chatRepository.Edit(objChat);
            }
            else
            {
                lstContent= new List<tbl_Message> { newMsg };
                var newChat = new tbl_Chat()
                {
                    Date = DateTime.Now.Date,
                    ListIdUser =  id + ',' + id1,
                    Count = count,
                    Content = JsonConvert.SerializeObject(lstContent, Formatting.Indented),
                    IdLast = Convert.ToInt32(id),
            };
                _chatRepository.Add(newChat);
            }

            var user1 = (_userAdminRepository.Find(Convert.ToInt32(id)) != null && _userAdminRepository.Find(Convert.ToInt32(id)).Photo != null ? _userAdminRepository.Find(Convert.ToInt32(id)).Photo : "");
            var user2 = (_userAdminRepository.Find(Convert.ToInt32(id1)) != null && _userAdminRepository.Find(Convert.ToInt32(id1)).Photo != null ? _userAdminRepository.Find(Convert.ToInt32(id1)).Photo : "");

            Clients.All.addNewMessageToPage(name, message, count, user1, user2);
        } 
    }
}