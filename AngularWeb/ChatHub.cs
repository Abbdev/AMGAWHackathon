using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace AngularWeb
{
    
    public class ChatHub : Hub
    {
        public void SendToAll(MessageData message)
        {
            message.Date = DateTime.Now;
            Clients.All.SendAsync("sendToAll", message);
        }
    }

    public class MessageData
    {
        public MessageUser ToUser { get; set; }
        public MessageUser FromUser { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
    public class MessageUser
    {
        public string Email {get; set;}
        public string Name { get; set; }
        public string Photo { get; set; }
    }
}
