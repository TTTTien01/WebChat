using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webchat.Entities;
using Webchat.ViewModels.AddUser;


namespace Webchat.Controllers
{
    public class AppUserController : Controller
    {
        readonly WebChatDbContext db; //thay cho var db= new.ƯebChatDbContext();
        //gọi là DI 
        public AppUserController(WebChatDbContext _db)//WebChatcontext ở Starup dòng 28

        {
           this.db = _db;//contracter đc gọi khi controller đc truy cập 
        }
        //Đăng ký
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(AddAppUser model)
        {
            AppUser user = new AppUser();
            if(ModelState.IsValid)//Nếu dữ liệu hợp lệ
            {
                try {
                    //mã hóa mật khẩu mot chieu
                    HMACSHA512 hmac = new();
                    var pwByte = Encoding.UTF8.GetBytes(model.Password);
                    user.PasswordHash = hmac.ComputeHash(pwByte);
                    user.PasswordSalt = hmac.Key;
                    //sao chep du lieu
                    user.UserName = model.UserName.Replace(" ","").ToLower();//Replace(" ",""): xóa khoảng trắng , ToLower(): chuyển chữ hoa về chữ thường 
                    user.FullName = model.FullName;
                    user.CreateDate =DateTime.Now;
                     //luu du lieeu
                    await db.AppUsers.AddAsync(user);
                    await db.SaveChangesAsync();
                    //tạo thành cong chuyển về trang đăng nhập
                    //tạo thất bại
                } 
                catch
                {
                    TempData["Mesg"] = "Đã xảy ra lỗi trong quá trình đăng kí tài khoản";
                }
            }
            else
            {
                TempData["Mesg"] = "Dữ liệu không hợp lệ";
            }
            return RedirectToAction(nameof(SignUp));
        }

        //Đăng nhập
        public IActionResult Login()
        {
            return View();//Tạo view Login.cshtml
        }
        [HttpPost]
        public async Task<IActionResult> Login(Login loginData)
        {
            loginData.UserName = loginData.UserName.Replace(" ", "").ToLower();
            var user = db.AppUsers
                .AsNoTracking()
                .SingleOrDefault(u => u.UserName == loginData.UserName);
            if(user!=null)
            {
                HMACSHA512 hmac = new HMACSHA512();
                var pwByte = Encoding.UTF8.GetBytes(loginData.Password);
                hmac.Key = user.PasswordSalt;
                var pwHash = hmac.ComputeHash(pwByte);
                if(pwHash.SequenceEqual(user.PasswordHash))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.GivenName, user.FullName)
                    };
                    var claimIdentity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(claimIdentity);
                    var authProperties = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddHours(6),
                        IsPersistent = loginData.RememberMe
                    };
                    await HttpContext.SignInAsync("Cookies", principal, authProperties);
                    //về trang đăng nhập
                    return RedirectToAction("Index", "Home");
                }   
            }
            else
            {
                //dang nhap that bai
            }
            return RedirectToAction(nameof(Login));
        }
        //dang xuat
        public async Task<IActionResult>LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
