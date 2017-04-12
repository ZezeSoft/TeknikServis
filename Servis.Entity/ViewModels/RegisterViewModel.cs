using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class RegisterViewModel
    {
        public string ID { get; set; }
        [Required]
        [Display(Name="Ad")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name="Şifre")]
        [StringLength(100,MinimumLength=3,ErrorMessage="Şifreniz en az 3 karakter olmalıdır")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name="Şifrenizi Tekrar Girin")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage="Şifreler Uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        [Display(Name="Aktivasyon Kodu")]
        public string EmailOnay { get; set; }

    }
}
