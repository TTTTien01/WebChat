using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webchat.ViewModels.AddUser
{
    public class AddAppUser
    {
        [Required(ErrorMessage ="Dữ liệu này là bắt buộc")]//ràng buộc ứng dụng
        [MinLength(3,ErrorMessage ="dữ liệu quá ngắn")]
        [MaxLength(200,ErrorMessage ="Dữ liệu quá dài")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Dữ liệu này là bắt buộc")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Mật khẩu quá ngắn")]

        public string Password { get; set; }

        [DisplayName("xác Nhận Mật Khẩu")]
        [DataType( DataType.Password)]
        [Compare("Password",ErrorMessage ="Mật khẩu không khớp")]//so sánh mật khẩu
        public string ConfirmPassword { get; set; }
        
        [Display(Name ="Tên Đầy Đủ")]
        [Required(ErrorMessage = "Dữ liệu này là bắt buộc")]
        public string FullName { get; set; }

    }
}
