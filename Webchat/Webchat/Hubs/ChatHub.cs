using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webchat.Entities;

namespace Webchat.Hubs
{
	public class ChatHub : Hub
	{
		readonly WebChatDbContext db;
		public ChatHub(WebChatDbContext _db)
		{
			db = _db;
		}
		public async Task Guitinnhan(string targetUserId, string message)
		{
			var currentUserId = Context.UserIdentifier;
			var users = new string[] { currentUserId, targetUserId };
			var response = new//thông tin gui về
			{
				sender = currentUserId,
				reciver = targetUserId,
				mesg = message,
				datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")
			};
			await Clients.Users(users).SendAsync("Phanhoitinnhan", response);//gửi lên cho cả 2

			//Lưu tin nhắn vào database
			AppMessage mesg = new AppMessage
			{
				Message = message,
				SendAt = DateTime.Now,
				ReciverId = Convert.ToInt32(targetUserId),
				SenderId = Convert.ToInt32(currentUserId)
			};
			await db.AddAsync(mesg);
			await db.SaveChangesAsync();
		}
		//bắt lấy sự kiện đang online
		static List<int> onlineUsers = new List<int>();
		public override async Task<Task> OnConnectedAsync()//Task là biến tĩnh biến toàn cục; OnConnectedAsync():hàm đc hỗ trợ
		{
			var currentUserId = Context.UserIdentifier; //userId hiện tại
			onlineUsers.Add(Convert.ToInt32(currentUserId));
			var response = new
			{
				onlineUsers
			};
			await Clients.All.SendAsync("GetUsers", response);//gửi cho tất cả 
			return base.OnConnectedAsync();
		}
		// khi offline
		public override async Task<Task> OnDisconnectedAsync(Exception exception)
		{
			var currentUserId = Context.UserIdentifier; //userId hiện tại
			onlineUsers.Remove(Convert.ToInt32(currentUserId));//trước khi gửi
			var response = new
			{
				onlineUsers,
				disconnectedId = Convert.ToInt32(currentUserId)
			};
			await Clients.All.SendAsync("GetUsers", response);//gửi cho tất cả 
			return base.OnDisconnectedAsync(exception);
		}

	}
}
