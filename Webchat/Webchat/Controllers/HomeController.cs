using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
    }
}

