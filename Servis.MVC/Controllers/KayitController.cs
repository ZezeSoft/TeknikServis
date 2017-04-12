using Servis.BLL.Repository;
using Servis.Entity.Entities;
using Servis.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Servis.Entity.IdentityModels;
using System.IO;
using System.Web.Helpers;
using Servis.BLL.Account;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace Servis.MVC.Controllers
{
    public class KayitController : Controller
    {
        // GET: Kayit
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Kayit()
        {
            if (HttpContext.User.Identity.IsAuthenticated == false)
                return RedirectToAction("Login", "Account");
            var model = new KayitViewModel();
            var cihazTurleri = new List<SelectListItem>();
            var markalar = new List<SelectListItem>();

            new CihazRepo().GetAll().ForEach(x => cihazTurleri.Add(new SelectListItem()
            {
                Text = x.Tur,
                Value = x.ID.ToString()
            }));

            new MarkaRepo().GetAll().ForEach(x => markalar.Add(new SelectListItem
            {
                Text = x.Tur,
                Value = x.ID.ToString()
            }));

            ViewBag.cihazturleri = cihazTurleri;
            ViewBag.markalar = markalar;

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Kayit(KayitViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            Repairment kayit = new Repairment()
            {
                Aciklama = model.Aciklama,
                Adres = model.Adres,
                Boylam = model.Boylam,
                Enlem = model.Enlem,
                CihazID = model.CihazID,
                MarkaID = model.MarkaID,
                KullaniciID = HttpContext.User.Identity.GetUserId(),
                ArizaDurumID = new ArizaDurumuRepo().GetAll().Where(x=> x.State=="To do").FirstOrDefault().ID
            };

            new ArizaRepo().Insert(kayit);

            if (model.Dosyalar.Count > 0)
            {
                foreach (var file in model.Dosyalar)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extensionName = Path.GetExtension(file.FileName);
                        fileName = fileName.Replace(" ", "");
                        fileName += Guid.NewGuid().ToString().Replace("-", "");
                        var klasorYolu = Server.MapPath("~/Upload/" + kayit.ID);
                        var dosyaYolu = Server.MapPath("~/Upload/" + kayit.ID + "/") + fileName + extensionName;
                        if (!Directory.Exists(klasorYolu))
                        {
                            Directory.CreateDirectory(klasorYolu);
                        }

                        file.SaveAs(dosyaYolu);
                        WebImage img = new WebImage(dosyaYolu);
                        img.Resize(640, 475, false);
                        img.Resize(800, 800);
                        img.Save(dosyaYolu);

                        new FotografRepo().Insert(new Photos()
                        {
                            ArizaID = kayit.ID,
                            Path = @"Upload/" + kayit.ID + "/" + fileName + extensionName
                        });
                    }
                }
            }

            #region MailIslemleri
            //string url = "http://localhost:13053/Kayit/Anket/"+kayit.ID;
            //string link = String.Format("<a href=\""+url+"\">Click here</a>");
            ////string link2 = String.Format("<a href=\"http://localhost:1900/ResetPassword/+kayit.ID+\">Click here</a>", kayit.ID);
            
            //var userStore = MembershipTools.NewUserStore();
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var findUser = userManager.FindByName(HttpContext.User.Identity.GetUserName());

            //var message = new MailMessage();
            //message.To.Add(new MailAddress(findUser.Email));  // replace with valid value 
            //message.From = new MailAddress("crazywissen503@gmail.com");  // replace with valid value
            //message.Subject = "Anket";
            //message.Body = "Arıza kaydınız tamamlanmıştır.Lütfen aşağıdaki linki tıklayıp anketimizi doldurun \n"+link;
            //message.IsBodyHtml = true;

            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential
            //    {
            //        UserName = "crazywissen503@gmail.com",  // replace with valid value
            //        Password = "wissen503"  // replace with valid value
            //    };
            //    smtp.Credentials = credential;
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 587;
            //    smtp.EnableSsl = true;

            //    await smtp.SendMailAsync(message);
            //}
            #endregion

            //return RedirectToAction("Anket", "Kayit", new { id = kayit.ID });
            return RedirectToAction("Index","Home");
        }
        public ActionResult Anket(int? id)
        {
            var anket = new AnketRepo().GetAll().Where(x=> x.ArizaID==id.Value).FirstOrDefault();
            if (anket != null) 
                return RedirectToAction("Index","Home");

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());
             var ariza = new ArizaRepo().GetById(id.Value);

            var model = new AnketViewModel();
            model.Sorular = new AnketSoruRepo().GetAll().Select(x => new SoruViewModel
            {
                ID = x.ID,
                Soru = x.Soru

            }).ToList();

            model.Cevaplar = new AnketCevapRepo().GetAll().Select(x => new CevapViewModel
            {
                Cevap = x.Cevap,
                ID = x.ID
            }).ToList();

            var cevaplar = new List<SelectListItem>();
            new AnketCevapRepo().GetAll().ForEach(x => cevaplar.Add(new SelectListItem
            {
                Text = x.Cevap,
                Value = x.ID.ToString()
            }));

            ViewBag.cevaplar = cevaplar;

            //if (user.Anketler.Count > 0)
            //{
            //    return RedirectToAction("Kayit", "Kayit");
            //}
            model.ArizaID = ariza.ID;

            return View(model);
        }
        [HttpPost]
        public ActionResult Anket(AnketViewModel model)
        {
            //foreach (var item in model.Sorular)
            //{
            //    Survey anket = new Survey()
            //    {
            //        SoruID = item.ID
            //    };
            //    new AnketRepo().Insert(anket);
            //}
            //foreach (var item in model.Cevaplar)
            //{
            //    Survey anket = new Survey()
            //    {
            //        CevapID = item.ID
            //    };
            //    new AnketRepo().Insert(anket);
            //}
            Survey anket = new Survey();
            var ariza = new ArizaRepo().GetById(model.ArizaID);

            for (int i = 0; i < model.Sorular.Count; i++)
            {
                anket.SoruID = model.Sorular[i].ID;
                anket.CevapID = model.Cevaplar[i].ID;
                //KullaniciID = HttpContext.User.Identity.GetUserId()          
                //anket.AnketinArizalari.Add(ariza);
                anket.ArizaID = ariza.ID;
                new AnketRepo().Insert(anket);
            }

            //ariza.AnketID = anket.ID;
            //new ArizaRepo().Update();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult KayitGuncelle(int? id)
        {
            if (id == null) return RedirectToAction("Index");

            var userId = HttpContext.User.Identity.GetUserId();

            var ariza = new ArizaRepo().GetById(id.Value);
            if (ariza == null) return RedirectToAction("Index");

            if (ariza.Sahibi.Id != userId)
                return RedirectToAction("Logout", "Account");

            var model = new KayitViewModel()
            {
                Aciklama = ariza.Aciklama,
                Adres = ariza.Adres,
                Boylam = ariza.Boylam,
                EklenmeTarihi = ariza.EklenmeTarihi,
                Fotograflar = ariza.Fotograflar.Count > 0 ? ariza.Fotograflar.Select(y => y.Path).ToList() : null,
                Enlem = ariza.Enlem,
                OnaylanmaTarihi = ariza.OnaylanmaTarihi,
                KullaniciID = userId,//HttpContext.User.Identity.GetUserId()
                OnaylandiMi = ariza.OnaylandiMi,
                ArizaDurumuID = ariza.ArizaDurumID,
                CihazID = ariza.CihazID,
                MarkaID = ariza.MarkaID,
                ID = ariza.ID
            };

            var cihazTurleri = new List<SelectListItem>();
            var markalar = new List<SelectListItem>();

            new CihazRepo().GetAll().ForEach(x => cihazTurleri.Add(new SelectListItem()
            {
                Text = x.Tur,
                Value = x.ID.ToString()
            }));

            new MarkaRepo().GetAll().ForEach(x => markalar.Add(new SelectListItem
            {
                Text = x.Tur,
                Value = x.ID.ToString()
            }));

            ViewBag.cihazturleri = cihazTurleri;
            ViewBag.markalar = markalar;

            return View(model);
        }
        public ActionResult KayitDuzenle(KayitViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Profile", "Account");
            var ariza = new ArizaRepo().GetById(model.ID);

            ariza.Aciklama = model.Aciklama;
            ariza.Adres = model.Adres;
            ariza.Boylam = model.Boylam;
            ariza.Enlem = model.Enlem;
            ariza.CihazID = model.CihazID;
            ariza.MarkaID = model.MarkaID;

            if (model.Dosyalar.Count > 0)
            {
                foreach (var file in model.Dosyalar)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string extensionName = Path.GetExtension(file.FileName);
                        fileName = fileName.Replace(" ", "");
                        fileName += Guid.NewGuid().ToString().Replace("-", "");

                        var klasorYolu = Server.MapPath("~/Upload/" + ariza.ID);
                        var dosyaYolu = Server.MapPath("~/Upload/" + ariza.ID + "/") + fileName + extensionName;
                        if (!Directory.Exists(klasorYolu))
                            Directory.CreateDirectory(klasorYolu);

                        file.SaveAs(dosyaYolu);

                        WebImage img = new WebImage(dosyaYolu);
                        //if (img.Width > 800)
                        img.Resize(640, 475, false);
                        img.Resize(800, 800);
                        img.Save(dosyaYolu);

                        new FotografRepo().Insert(new Photos() //parantezi açmazsak default constructor çalışıyor
                        {
                            ArizaID = ariza.ID,
                            Path = @"Upload/" + ariza.ID + "/" + fileName + extensionName
                        });
                    }
                }
            }

            new ArizaRepo().Update();

            return RedirectToAction("Profile", "Account", new { id = model.ID });
        }
        public ActionResult Performans()
        {
            //var model = new PerformansViewModel();

            //model.Anketler = new AnketRepo().GetAll().Select(x => new AnketViewModel
            //{
            //    ArizaID = x.ArizaID,
            //    Cevap = x.Cevabi,
            //    ID = x.ID,
            //    Soru = x.SoruID
            //}
            //     );
            ////model.Kayitlar = new ArizaRepo().GetAll().ToList();
            var model = new ArizaRepo().GetAll().Select(x => new KayitViewModel
            {
                Aciklama = x.Aciklama,
                Adres = x.Adres,
                Boylam = x.Boylam,
                EklenmeTarihi = x.EklenmeTarihi,
                Fotograflar = x.Fotograflar.Count > 0 ? x.Fotograflar.Select(y => y.Path).ToList() : null,
                Enlem = x.Enlem,
                OnaylanmaTarihi = x.OnaylanmaTarihi,
                KullaniciID = x.KullaniciID,//HttpContext.User.Identity.GetUserId()
                OnaylandiMi = x.OnaylandiMi,
                ArizaDurumuID = x.ArizaDurumID,
                CihazID = x.CihazID,
                MarkaID = x.MarkaID,
                 TeknisyenID = x.TeknisyenID,
                ID = x.ID
            }).ToList();

            return View(model);
        }
    }
}