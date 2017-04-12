using Microsoft.AspNet.Identity;
using Servis.BLL.Account;
using Servis.BLL.Repository;
using Servis.Entity.Entities;
using Servis.Entity.IdentityModels;
using Servis.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Servis.MVC.Controllers
{
    public class TeknisyenController : Controller
    {
        // GET: Teknisyen
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Navigation()
        {
            return PartialView("Teknisyen/_TeknisyenPartial");
        }
        public ActionResult IsDetay()
        {
            var model = new ArizaRepo().GetAll().Select(x => new KayitViewModel
            {
                Aciklama = x.Aciklama,
                Adres = x.Adres,
                Enlem = x.Enlem,
                Boylam = x.Boylam,
                CihazID = x.CihazID,
                MarkaID = x.MarkaID,
                EklenmeTarihi = x.EklenmeTarihi,
                KullaniciID = x.KullaniciID,
                OnaylandiMi = x.OnaylandiMi,
                OnaylanmaTarihi = x.OnaylanmaTarihi,
                ArizaDurumuID = x.ArizaDurumID,
                Rapor = x.Rapor,
                ID = x.ID,
                AtandiMi = x.AtandiMi,
                TeknisyenID = x.TeknisyenID,
                Fotograflar = x.Fotograflar.Select(y => y.Path).ToList()
            }).ToList();

            return View(model);
        }
        public ActionResult IsDuzenle(int? id)
        {
            if (id == null) return RedirectToAction("IsDetay", "Teknisyen");

            var kayit = new ArizaRepo().GetById(id.Value);

            if (kayit == null) return RedirectToAction("IsDetay", "Teknisyen");

            var model = new KayitViewModel
            {
                Aciklama = kayit.Aciklama,
                Adres = kayit.Adres,
                Enlem = kayit.Enlem,
                Boylam = kayit.Boylam,
                CihazID = kayit.CihazID,
                MarkaID = kayit.MarkaID,
                EklenmeTarihi = kayit.EklenmeTarihi,
                KullaniciID = kayit.KullaniciID,
                OnaylandiMi = kayit.OnaylandiMi,
                OnaylanmaTarihi = kayit.OnaylanmaTarihi,
                ArizaDurumuID = kayit.ArizaDurumID,
                Rapor = kayit.Rapor,
                ID = kayit.ID,
                AtandiMi = kayit.AtandiMi,
                Fotograflar = kayit.Fotograflar.Select(y => y.Path).ToList()
            };

            var cihazTurleri = new List<SelectListItem>();
            var markalar = new List<SelectListItem>();
            var arizaDurumlari = new List<SelectListItem>();

            new ArizaDurumuRepo().GetAll().ForEach(x => arizaDurumlari.Add(new SelectListItem
            {
                Text = x.State,
                Value = x.ID.ToString()
            }));

            ViewBag.arizadurumlari = arizaDurumlari;

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> IsDuzenle(KayitViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var guncellenecekIs = new ArizaRepo().GetById(model.ID);

            guncellenecekIs.ArizaDurumID = model.ArizaDurumuID;
            if (model.ArizaDurumuID == 3)
                guncellenecekIs.Teknisyeni.Available = false;
            new ArizaRepo().Update();

            if (model.ArizaDurumuID == 3)
            {
                #region MailIslemleri
                string url = "http://localhost:13053/Kayit/Anket/" + model.ID;
                string link = String.Format("<a href=\"" + url + "\">Click here</a>");
                //string link2 = String.Format("<a href=\"http://localhost:1900/ResetPassword/+kayit.ID+\">Click here</a>", kayit.ID);

                var userStore = MembershipTools.NewUserStore();
                var userManager = new UserManager<ApplicationUser>(userStore);
                var findUser = userManager.FindByName(HttpContext.User.Identity.GetUserName());

                var message = new MailMessage();
                message.To.Add(new MailAddress(findUser.Email));  // replace with valid value 
                message.From = new MailAddress("crazywissen503@gmail.com");  // replace with valid value
                message.Subject = "Anket";
                message.Body = "Arıza kaydınız tamamlanmıştır.Lütfen aşağıdaki linki tıklayıp anketimizi doldurun \n" + link;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "crazywissen503@gmail.com",  // replace with valid value
                        Password = "wissen503"  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);
                }
                #endregion
            }          

            return RedirectToAction("IsDuzenle", "Teknisyen");
        }
        public ActionResult BitenIs()
        {
            var model = new ArizaRepo().GetAll().Select(x => new KayitViewModel
            {
                Aciklama = x.Aciklama,
                Adres = x.Adres,
                Enlem = x.Enlem,
                Boylam = x.Boylam,
                CihazID = x.CihazID,
                MarkaID = x.MarkaID,
                EklenmeTarihi = x.EklenmeTarihi,
                KullaniciID = x.KullaniciID,
                OnaylandiMi = x.OnaylandiMi,
                OnaylanmaTarihi = x.OnaylanmaTarihi,
                ArizaDurumuID = x.ArizaDurumID,
                Rapor = x.Rapor,
                ID = x.ID,
                AtandiMi = x.AtandiMi,
                TeknisyenID = x.TeknisyenID,
                Fotograflar = x.Fotograflar.Select(y => y.Path).ToList()
            }).ToList();

            return View(model);
        }
        public ActionResult Raporlama(int? id)
        {
            if (id == null) return RedirectToAction("IsDetay", "Teknisyen");
            var kayit = new ArizaRepo().GetById(id.Value);

            if (kayit == null) return RedirectToAction("BitenIs", "Teknisyen");

            var model = new KayitViewModel()
            {
                ID = kayit.ID,
                KullaniciID = kayit.KullaniciID,
                Rapor = kayit.Rapor,
                MarkaID = kayit.MarkaID,
                CihazID = kayit.CihazID,
                TeknisyenID = kayit.TeknisyenID
            };

            return View(model);
        }
        [HttpPost]
        public ActionResult Raporlama(KayitViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var guncelKayit = new ArizaRepo().GetById(model.ID);

            guncelKayit.Rapor = model.Rapor;
            new ArizaRepo().Update();

            return RedirectToAction("BitenIs", "Teknisyen");
        }
    }
}