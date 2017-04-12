using Servis.Entity.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class AnketViewModel
    {
        public AnketViewModel()
        {
            Cevaplar = new List<CevapViewModel>();
            Sorular = new List<SoruViewModel>();
            Soru = new SoruViewModel();
            Cevap = new CevapViewModel();
        }
        //public int ID { get; set; }
        //public string Soru { get; set; }
        //public int? CevapID { get; set; }
        //public string KullaniciID { get; set; }
        public int ID { get; set; }
        public int ArizaID { get; set; }

        public List<CevapViewModel> Cevaplar { get; set; }
        public List<SoruViewModel> Sorular { get; set; }

        public CevapViewModel Cevap { get; set; }
        public SoruViewModel Soru { get; set; }
        //public int KullaniciID { get; set; }

    }

    public class CevapViewModel
    {
        public int ID { get; set; }
        public string Cevap { get; set; }
        public int Puani { get; set; }
    }
    public class SoruViewModel
    {
        public int ID { get; set; }
        public string Soru { get; set; }
    }
}
