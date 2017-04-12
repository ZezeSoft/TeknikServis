using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Servis.Entity.ViewModels
{
    public class KayitViewModel
    {
        public KayitViewModel()
        {
            Fotograflar = new List<string>();
            Dosyalar = new List<HttpPostedFileBase>();
        }
        public int ID { get; set; }
        public string Adres { get; set; }
        [Column(TypeName = "date")]
        public DateTime EklenmeTarihi { get; set; }
        [Display(Name="Açiklama")]
        public string Aciklama { get; set; }
        public double Enlem { get; set; }
        public double Boylam { get; set; }
        public bool OnaylandiMi { get; set; }
        public bool AtandiMi { get; set; }
        public DateTime? OnaylanmaTarihi { get; set; }
        public int? ArizaDurumuID { get; set; }
        public string KullaniciID { get; set; }
        public string TeknisyenID { get; set; }
     
        [Display(Name = "Marka")]
        public int MarkaID { get; set; }
        [Display(Name = "Cihaz Türü")]
        public int CihazID { get; set; }
        [Display(Name = "Rapor")]
        public string Rapor { get; set; }
    
        public List<string> Fotograflar { get; set; }
        public List<HttpPostedFileBase> Dosyalar { get; set; }
    }

}
