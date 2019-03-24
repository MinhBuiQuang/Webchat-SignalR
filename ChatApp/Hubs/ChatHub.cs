using ChatApp.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        WebChatEntities db = new WebChatEntities();
        public override Task OnConnected()
        {
            int userID = Int32.Parse(HttpContext.Current.Request.Cookies["userid"].Value);
            User user = db.Users.First(p => p.UserID == userID);
            user.IsOnline = true;
            user.LastOnline = DateTime.Now;
            user.ClientChatID = Context.ConnectionId;
            db.SaveChanges();
            return base.OnConnected();
        }


        public Task SendingMessageToGroup(string groupName, string message)
        {
            MessageLog mes = new MessageLog();
            User user = db.Users.First(p => p.ClientChatID == Context.ConnectionId);
            mes.Message = message;
            mes.UserID = user.UserID;
            mes.YeuCauID = Int32.Parse(groupName);
            mes.Timestamp = DateTime.Now;           
            db.MessageLogs.Add(mes);
            db.SaveChangesAsync();
            return Clients.Group(groupName).receivingMessage(Context.ConnectionId, message, user.Username, DateTime.Now);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public async Task AddToGroupAdmin(string groupName)
        {
            await Groups.Add(Context.ConnectionId, "QLTN-AdminGroup");
            await Groups.Add(Context.ConnectionId, groupName);
        }

        public override Task OnReconnected()
        {
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            User user = db.Users.First(p => p.ClientChatID == Context.ConnectionId);
            user.IsOnline = false;
            user.LastOnline = DateTime.Now;
            db.SaveChanges();
            return base.OnDisconnected(stopCalled);
        }
    }
}