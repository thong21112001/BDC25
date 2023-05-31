using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BDC25.Areas.Admin.Data
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Mời nhập username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mời nhập password")]
        public string PassWord { get; set; }

        public bool Remember { get; set; }
    }
}