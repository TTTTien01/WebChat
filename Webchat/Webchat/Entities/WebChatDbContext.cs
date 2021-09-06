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
		public DbSet<AppMessage> AppMessages { get; set; }//tạo bảng trong database

		//ctor tag 2 laanf
		public WebChatDbContext(DbContextOptions options): base(options)
        {
        }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AppMessage>()
				.HasOne(m => m.Sender)
				.WithMany(u => u.SendMessage)
				.HasForeignKey(m => m.SenderId)
				.OnDelete(DeleteBehavior.NoAction);//
			modelBuilder.Entity<AppMessage>()
				.HasOne(m => m.Reciver)
				.WithMany(u => u.RecivedMessage)
				.HasForeignKey(m => m.ReciverId)
				.OnDelete(DeleteBehavior.NoAction);//

		}
	}
}
