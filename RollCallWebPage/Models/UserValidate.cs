using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace RollCallWebPage.Models
{
    public class UserValidate
    {
        [Required(ErrorMessage="用户名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage="密码不能为空")]
        public string Password { get; set; }

        public string Identity { get; set; }

        [DisplayName("下次自动登录")]
        public bool RememberMe { get; set; }
    }
}