using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Webchat.Common;
using Webchat.Entities;

namespace Webchat.Controllers
{
    public class HomeController : Controller
    {// khoi tao  database
        private readonly WebChatDbContext db;
        //ctor tap 2 lan
        public HomeController(WebChatDbContext _db)
        {
            db = _db;
        }
        // chat khong có chat me, lấy user ra
        public async Task <IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)// đã đăng nhập
            {
                var currenUserName = HttpContext.User.Identity.Name;
                var listUsers = await db.AppUsers
                    .AsNoTracking() // tốc độ truy vấn database
                    .Where(u => u.UserName != currenUserName)
                    .ToListAsync();
                return View("Chat",listUsers);
            }   
            else
            {
                return View();
            }
            
        }
		[Route("load-tn/{targetUserId:int}")]//thêm URL mới
		[Authorize] //Chỉ cho phép khi đã đăng nhập

		public async Task<IActionResult> LoadTN(int targetUserId)
		{
			int myId = Convert.ToInt32(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

			var DSMesg = await db.AppMessages
				.Where(m =>
				(m.SenderId == myId && m.ReciverId == targetUserId)
				|| (m.ReciverId == myId && m.SenderId == targetUserId))
				.AsNoTracking()
				.OrderByDescending(m => m.Id) //sấp xếp giảm dần theo Id
				.Take(20) //lấy 20 phần tử (20 tin nhắn)
				.ToListAsync();
			//giải mã nội dung tin nhắn 
			for (int i = 0; i < DSMesg.Count; i++)
			{
				DSMesg[i].Message = AESThenHMAC.SimpleDecryptWithPassword(DSMesg[i].Message, AppConfig.MESG_KEY);
			}
			//thống nhất cấu trúc dữ liệu cho ChatHup
			var response = DSMesg.Select(m => new
			{
				sender = m.SenderId,
				reciver = m.ReciverId,
				mesg = m.Message,
				datetime = m.SendAt.ToString("dd/MM/yyyy HH:mm:ss")
			});

			return Ok(response);
		}
    }
}

