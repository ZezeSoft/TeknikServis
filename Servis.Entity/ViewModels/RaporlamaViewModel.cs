using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class RaporlamaViewModel
    {
        public int ID { get; set; }
        [Required]
        public string Durum { get; set; }
    }
}
