using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class ManagementViewModel
    {
        public ManagementViewModel()
        {
            this.Markalar = new List<MarkaViewModel>();
            this.CihazTurleri = new List<CihazViewModel>();
            this.Arizalar = new List<KayitViewModel>();

            Marka = new MarkaViewModel();
            Ariza = new KayitViewModel();
            CihazTuru = new CihazViewModel();
        }
        public List<MarkaViewModel> Markalar { get; set; }
        public List<CihazViewModel> CihazTurleri { get; set; }
        public List<KayitViewModel> Arizalar { get; set; }
        public MarkaViewModel Marka { get; set; }
        public CihazViewModel CihazTuru { get; set; }
        public KayitViewModel Ariza { get; set; }

    }
    public class MarkaViewModel
    {
        public int ID { get; set; }
        [StringLength(50)]
        [Required]
        [Display(Name="Marka")]
        public string Tur { get; set; }
    }
    public class CihazViewModel
    {
        public int ID { get; set; }
        [StringLength(50)]
        [Required]
        [Display(Name = "Cihaz Türü")]
        public string Tur { get; set; }
    }
}
