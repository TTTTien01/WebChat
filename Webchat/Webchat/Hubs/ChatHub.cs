using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webchat.Hubs
{
    public class ChatHub : Hub
    { 
        public async Task Guitinnhan(string targetUserId, string message)
        {
            var currentUserId = Context.UserIdentifier;
            var users = new string[] { currentUserId, targetUserId};
            var response = new//thông tin gui về
            {
                sender = currentUserId,
                reciver = targetUserId,
                mesg = message,
                datetime= DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
            };
            await Clients.Users(users).SendAsync("Phanhoitinnhan", response);//gửi lên cho cả 2
        }


    }
}
