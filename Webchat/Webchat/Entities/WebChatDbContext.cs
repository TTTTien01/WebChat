using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webchat.Entities
{
    public class WebChatDbContext : DbContext
    {

        public DbSet<AppUser> AppUsers { get; set; }//tạo bảng trong database
        //ctor tag 2 laanf
        public WebChatDbContext(DbContextOptions options): base(options)
        {
        }
    }
}
