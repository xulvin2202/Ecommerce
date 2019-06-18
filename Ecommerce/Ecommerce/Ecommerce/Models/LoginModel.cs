using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class LoginModel
    {
        [Key]
        [Display(Name  ="Tên đăng nhập")]
        [Required(ErrorMessage = "Cần nhập tài khoản")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Cần nhập mật khẩu")]
        [Display(Name ="Mật khẩu")]
        public string Password { get; set; }
    }
}