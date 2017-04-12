using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servis.Entity.ViewModels
{
    public class PerformansViewModel
    {
        //public PerformansViewModel()
        //{
        //    Kayitlar = new List<ArizaViewModel>();
        //    Anketler = new List<AnketSonucViewModel>();
        //    Kayit = new ArizaViewModel();
        //    Anket = new AnketSonucViewModel();
        //}
        //public List<ArizaViewModel> Kayitlar { get; set; }
        //public List<AnketSonucViewModel> Anketler { get; set; }
        //public ArizaViewModel Kayit { get; set; }
        //public AnketSonucViewModel Anket { get; set; }
        public PerformansViewModel()
        {
            Kayitlar = new List<KayitViewModel>();
            Anketler = new List<AnketViewModel>();
            Kayit = new KayitViewModel();
            Anket = new AnketViewModel();
        }
        public List<KayitViewModel> Kayitlar { get; set; }
        public List<AnketViewModel> Anketler { get; set; }
        public KayitViewModel Kayit { get; set; }
        public AnketViewModel Anket { get; set; }
    }

    //public class ArizaViewModel
    //{
    //    public int ID { get; set; }
    //    public string KullaniciID { get; set; }
    //    public string TeknisyenID { get; set; }
    //}
    //public class AnketSonucViewModel
    //{
    //    public int ID { get; set; }
    //    public int ArizaID { get; set; }
    //    public CevapViewModel Cevap { get; set; }
    //    public SoruViewModel Soru { get; set; }

    //}
}
