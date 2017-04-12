using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class RecoveryViewModel
    {
        [Required, Display(Name = "E Posta Adresin"), EmailAddress]
        public string Email { get; set; }

        public string Message { get; set; }
    }
}
