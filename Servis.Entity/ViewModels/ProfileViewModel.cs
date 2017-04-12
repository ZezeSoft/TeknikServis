using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class ProfileViewModel
    {
        public ProfileViewModel()
        {
            Arizalar = new List<KayitViewModel>();
        }
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Display(Name = "Durumu")]
        public bool Available { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 5 karakter olmalıdır")]
        [Display(Name = "Sifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Sifre Tekrar")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler Uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 5 karakter olmalıdır")]
        [Display(Name = "Eski Sifre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Display(Name = "Profil Fotografı")]
        public string AvatarPath { get; set; }
        [Display(Name = "Rol")]
        public string RolName { get; set; }
        public string RolID { get; set; }
        public string UserID { get; set; }
        public List<KayitViewModel> Arizalar { get; set; }
    }
}
