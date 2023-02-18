using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Azure_0517
{
    public class SignalRHub : Hub
    {
        public void Send()
        {
            Clients.All.addNewNotice();
            Clients.All.noticeLoad();
            Clients.Others.addNewMessage();
            Clients.All.messageLoad();
        }
    }
}