using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecommerce.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Cần Nhập Username")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Cần Nhập Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}