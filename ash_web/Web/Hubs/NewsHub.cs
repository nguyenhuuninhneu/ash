using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Web.Hubs
{
    [HubName("loadNews")]
    public class NewsHub : Hub
    {
        public void Send(int id)
        { 
            Clients.All.addNewMessageToPage(id);
        }
    }
}