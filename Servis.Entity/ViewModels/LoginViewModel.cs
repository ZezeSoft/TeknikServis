using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Şifre")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Şifreniz en az 3 karakter olmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name="Beni Hatırla")]
        public bool RememberMe { get; set; }

    }
}
