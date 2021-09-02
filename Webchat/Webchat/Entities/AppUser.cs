using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Webchat.Entities
{
    [Table("AppUser")]
    [Index("UserName",IsUnique=true)]
    public class AppUser    //chuyen Appuser thanh bang
    {
        //ten proboties..se la ten cot
        //Id
        //Username
        //PasswordHash
        //PasswordSalt
        //FullName
        //CreateDate

        public int Id { get; set; } //Id khoa chinnh int tu tang
        [Required] //=> not null
        [MaxLength(200)] //=>nvarchar(200)
        public string UserName { get; set; } //nvarchar
        [Required] //=> not null
        [MaxLength(200)]
        public byte[] PasswordHash { get; set; }
        [Required]
        [MaxLength(200)]
        public byte[] PasswordSalt { get; set; }
        [Required]
        [MaxLength(200)]
        public string FullName { get; set; }

        public DateTime? CreateDate { get; set; }// Allow? co the null trong database


    }
}
