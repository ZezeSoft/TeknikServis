using Servis.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.Entities
{
    [Table("Ariza")]
    public class Repairment
    {
        public Repairment()
        {
            EklenmeTarihi = DateTime.Now;
            Fotograflar = new List<Photos>();
        }
        [Key]
        public int ID { get; set; }
        public string Adres { get; set; }
        [Column(TypeName="date")]
        public DateTime EklenmeTarihi { get; set; }
        public string Aciklama { get; set; }
        public double Enlem { get; set; }
        public double Boylam { get; set; }
        public bool OnaylandiMi { get; set; }
        public bool AtandiMi { get; set; }
        public DateTime? OnaylanmaTarihi { get; set; }
        public string KullaniciID { get; set; }
        [ForeignKey("KullaniciID")]
        public virtual ApplicationUser Sahibi { get; set; }
        public string TeknisyenID { get; set; }
        [ForeignKey("TeknisyenID")]
        public virtual ApplicationUser Teknisyeni { get; set; }

        public int MarkaID { get; set; }
        [ForeignKey("MarkaID")]
        public virtual Brand Marka { get; set; }

        public int CihazID { get; set; }
        [ForeignKey("CihazID")]
        public virtual Device CihazTuru { get; set; }
        public string Rapor { get; set; }

        public int? ArizaDurumID { get; set; }
        [ForeignKey("ArizaDurumID")]
        public virtual StateOfRepairment ArizaDurumu { get; set; }

        //public int? AnketID { get; set; }
        //[ForeignKey("AnketID")]
        //public virtual Survey Anket { get; set; }

        public virtual List<Survey> SoruveCevaplar { get; set; }

        public virtual List<Photos> Fotograflar { get; set; }
    }
}
