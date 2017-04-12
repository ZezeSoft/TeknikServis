using Servis.BLL.Repository;
using Servis.Entity.Entities;
using Servis.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Servis.BLL.Account;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Servis.Entity.IdentityModels;

namespace Servis.MVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult Navigation()
        {
            return PartialView("Admin/_NavigationPartial");
        }
        public ActionResult ArizaDetay()
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
                Fotograflar = x.Fotograflar.Select(y => y.Path).ToList()
            }).ToList();

            var userManager = MembershipTools.NewUserManager();
            var roleManager = MembershipTools.NewRoleManager();

            var teknisyenler = new List<SelectListItem>();
            var hepsi = userManager.Users.Where(x => x.Roles.FirstOrDefault().RoleId == "28b266b5-c87e-4b60-a563-b178fd9f0b6c" && x.Available == false).ToList();
            hepsi.ForEach(x => teknisyenler.Add(new SelectListItem
            {
                Text = x.Name + x.SurName,
                Value = x.Id
            }));
            ViewBag.teknisyenler = teknisyenler;

            return View(model);
        }
        public ActionResult ArizaDuzenle(int? id)
        {
            if (id == null) return RedirectToAction("ArizaDetay", "Admin");

            var ariza = new ArizaRepo().GetById(id.Value);

            if (ariza == null) return RedirectToAction("ArizaDetay", "Admin");

            var model = new KayitViewModel
            {
                Aciklama = ariza.Aciklama,
                Adres = ariza.Adres,
                Enlem = ariza.Enlem,
                Boylam = ariza.Boylam,
                CihazID = ariza.CihazID,
                MarkaID = ariza.MarkaID,
                EklenmeTarihi = ariza.EklenmeTarihi,
                KullaniciID = ariza.KullaniciID,
                OnaylandiMi = ariza.OnaylandiMi,
                OnaylanmaTarihi = ariza.OnaylanmaTarihi,
                ArizaDurumuID = ariza.ArizaDurumID,
                Rapor = ariza.Rapor,
                Fotograflar = ariza.Fotograflar.Select(y => y.Path).ToList(),
                ID = ariza.ID
            };

            var cihazTurleri = new List<SelectListItem>();
            var markalar = new List<SelectListItem>();
            var arizaDurumlari = new List<SelectListItem>();

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

            new ArizaDurumuRepo().GetAll().ForEach(x => arizaDurumlari.Add(new SelectListItem
            {
                Text = x.State,
                Value = x.ID.ToString()
            }));

            ViewBag.cihazturleri = cihazTurleri;
            ViewBag.markalar = markalar;
            ViewBag.arizadurumlari = arizaDurumlari;

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> ArizaDuzenle(KayitViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var guncellenecekAriza = new ArizaRepo().GetById(model.ID);

            #region MailGondermeİslemi

            if (guncellenecekAriza.OnaylandiMi == false && model.OnaylandiMi == true)
            {
                guncellenecekAriza.OnaylanmaTarihi = DateTime.Now;

                var message = new MailMessage();
                message.To.Add(new MailAddress(guncellenecekAriza.Sahibi.Email));
                message.From = new MailAddress("crazywissen503@gmail.com");
                message.Subject = "Arıza Kaydınız Onaylanmıştır!";
                message.Body = "Merhaba" + guncellenecekAriza.Sahibi.Name + "/n:" + guncellenecekAriza.ID + " nolu arıza kaydınız onaylanmıştır./nEn yakın zamanda adresinize kontrol için gelinecektit";
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "crazywissen503@gmail.com",
                        Password = "wissen503"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);
                }
            }
            else if (guncellenecekAriza.OnaylandiMi == true && model.OnaylandiMi == false)
            {
                guncellenecekAriza.OnaylanmaTarihi = null;

                var message = new MailMessage();
                message.To.Add(new MailAddress(guncellenecekAriza.Sahibi.Email));
                message.From = new MailAddress("crazywissen503@gmail.com");
                message.Subject = "Arıza Kaydınızın Onayı Kaldırıldı!";
                message.Body = "Merhaba" + guncellenecekAriza.Sahibi.Name + "/n:" + guncellenecekAriza.ID + " nolu arıza kaydınızın onayı kaldırılmıştır./nLütfen Girdiğiniz bilgilerin doğruluğundan emin olunuz";
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "crazywissen503@gmail.com",
                        Password = "wissen503"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    await smtp.SendMailAsync(message);
                }
            }

            #endregion

            guncellenecekAriza.Aciklama = model.Aciklama;
            guncellenecekAriza.Adres = model.Adres;
            guncellenecekAriza.ArizaDurumID = model.ArizaDurumuID;
            guncellenecekAriza.Enlem = model.Enlem;
            guncellenecekAriza.Boylam = model.Boylam;
            guncellenecekAriza.CihazID = model.CihazID;
            guncellenecekAriza.EklenmeTarihi = model.EklenmeTarihi;
            guncellenecekAriza.MarkaID = model.MarkaID;
            guncellenecekAriza.OnaylandiMi = model.OnaylandiMi;
            new ArizaRepo().Update();

            return RedirectToAction("ArizaDetay", "Admin");
        }
        public ActionResult TeknisyenAtama()
        {
            var userManager = MembershipTools.NewUserManager();
            var roleManager = MembershipTools.NewRoleManager();

            var model = userManager.Users.ToList().Select(x => new ProfileViewModel
            {
                Available = x.Available,
                Name = x.Name,
                Surname = x.SurName,
                UserName = x.UserName,
                RolID = x.Roles.FirstOrDefault().RoleId,
                UserID = x.Id,
            }).ToList();

            model.ForEach(x => x.RolName = roleManager.FindById(x.RolID).Name);

            return View(model.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> TeknisyenAtama(KayitViewModel model)
        {
            var guncellenecekAriza = new ArizaRepo().GetById(model.ID);
            guncellenecekAriza.TeknisyenID = model.TeknisyenID;
            guncellenecekAriza.AtandiMi = true;

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindById(model.TeknisyenID);
            user.Available = true;

            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            new ArizaRepo().Update();

            return RedirectToAction("ArizaDetay", "Admin");
        }

        public ActionResult KayitAyar()
        {
            var model = new ManagementViewModel();
            model.CihazTurleri = new CihazRepo().GetAll().Select(x => new CihazViewModel
            {
                ID = x.ID,
                Tur = x.Tur
            }).ToList();

            model.Markalar = new MarkaRepo().GetAll().Select(x => new MarkaViewModel
            {
                ID = x.ID,
                Tur = x.Tur
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult YeniMarkaEkle(string markaadi)
        {
            if (string.IsNullOrEmpty(markaadi.Trim()))
            {
                return Json(new
                {
                    success = false,
                    message = "Eklenecek kayit giriniz"
                }, JsonRequestBehavior.AllowGet);
            }

            var marka = new MarkaRepo().GetAll().Where(x => x.Tur == markaadi).FirstOrDefault();
            if (marka != null)
            {
                return Json(new
                {
                    success = false,
                    message = "Bu kayıt daha önce eklenmiştir"
                }, JsonRequestBehavior.AllowGet);
            }

            try
            {
                Brand yeniMarka = new Brand();
                yeniMarka.Tur = markaadi;
                new MarkaRepo().Insert(yeniMarka);
                return Json(new
                {
                    success = true,
                    message = "Kaydınız başarıyla eklenmiştir"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Kayıt ekleme sırasında bir hata oluştu"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult MarkaBul(int? id)
        {
            if (id == null)
                return Json(new
                {
                    success = false,
                    message = "Guncellenecek kayıt bulunamadı"
                }, JsonRequestBehavior.AllowGet);

            var guncellenecek = new MarkaRepo().GetById(id.Value);
            return Json(new
            {
                success = true,
                message = guncellenecek.Tur
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult MarkaGuncelle(int? markaid, string guncellenecekadi)
        {
            if (markaid == null || string.IsNullOrEmpty(guncellenecekadi.Trim()))
                return Json(new
                {
                    success = false,
                    message = "Guncellenecek kayıt yok"
                }, JsonRequestBehavior.AllowGet);

            try
            {
                var marka = new MarkaRepo().GetById(markaid.Value);
                marka.Tur = guncellenecekadi;
                new MarkaRepo().Update();
                return Json(new
                {
                    success = true,
                    message = "Guncelleme işlemi başarıyla tamamlandı"
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Guncelleme sırasında bir hata oluştu"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult YeniCihazEkle(string cihazadi)
        {
            if (cihazadi == null)
                return Json(new
                {
                    success = false,
                    message = "Eklenecek bir cihaz bulunamadı"
                }, JsonRequestBehavior.AllowGet);

            var kayit = new CihazRepo().GetAll().Where(x => x.Tur == cihazadi).FirstOrDefault();
            if (kayit != null)
                return Json(new
                {
                    success = false,
                    message = "Bu cihaz zaten kayıtlı"
                }, JsonRequestBehavior.AllowGet);
            try
            {
                Device cihaz = new Device();
                cihaz.Tur = cihazadi;
                new CihazRepo().Insert(cihaz);
                return Json(new
                {
                    success = true,
                    message = "Kayıt işlemi başarıyla tamamlandı"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Kayıt işlemi sırasında bir hata oluştu => " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult CihazBul(int? id)
        {
            if (id == null)
                return Json(new
                {
                    success = false,
                    message = "Cihaz bulunamadı"
                }, JsonRequestBehavior.AllowGet);

            var cihaz = new CihazRepo().GetById(id.Value);
            return Json(new
            {
                success = true,
                message = cihaz.Tur
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CihazGuncelle(int? cihazid, string ad)
        {
            if (cihazid == null || string.IsNullOrEmpty(ad))
                return Json(new
                {
                    success = false,
                    message = "Kayıt bulunamadı"
                }, JsonRequestBehavior.AllowGet);

            try
            {
                var kayit = new CihazRepo().GetById(cihazid.Value);
                kayit.Tur = ad;
                new CihazRepo().Update();
                return Json(new
                {
                    success = true,
                    message = "Guncelleme işlemi başarıyla gerçekleşti"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Guncelleme işlemi sırasında bir hata oluştu"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AnketAyar()
        {
            var model = new AnketViewModel();
            model.Sorular = new AnketSoruRepo().GetAll().Select(x => new SoruViewModel
            {
                Soru = x.Soru,
                ID = x.ID
            }).ToList();

            model.Cevaplar = new AnketCevapRepo().GetAll().Select(x => new CevapViewModel
            {
                Cevap = x.Cevap,
                ID = x.ID
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public JsonResult YeniSoruEkle(string soruadi)
        {
            if (soruadi == null)
                return Json(new
                {
                    success = false,
                    message = "Eklemek istediğiniz soruyu yazınız"
                }, JsonRequestBehavior.AllowGet);

            var kayit = new AnketSoruRepo().GetAll().Where(x => x.Soru == soruadi).FirstOrDefault();
            if (kayit != null)
                return Json(new
                {
                    success = false,
                    message = "Bu soru zaten kayıtlı"
                }, JsonRequestBehavior.AllowGet);
            try
            {
                QuestionsOfSurvey soru = new QuestionsOfSurvey();
                soru.Soru = soruadi;
                new AnketSoruRepo().Insert(soru);
                return Json(new
                {
                    success = true,
                    message = "Kayıt işlemi başarıyla tamamlandı"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Kayıt işlemi sırasında bir hata oluştu => " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult SoruBul(int? id)
        {
            if (id == null)
                return Json(new
                {
                    success = false,
                    message = "Soru bulunamadı"
                }, JsonRequestBehavior.AllowGet);

            var soru = new AnketSoruRepo().GetById(id.Value);
            return Json(new
            {
                success = true,
                message = soru.Soru
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SoruGuncelle(int? soruid, string ad)
        {
            if (soruid == null || string.IsNullOrEmpty(ad))
                return Json(new
                {
                    success = false,
                    message = "Kayıt bulunamadı"
                }, JsonRequestBehavior.AllowGet);

            try
            {
                var kayit = new AnketSoruRepo().GetById(soruid.Value);
                kayit.Soru = ad;
                new AnketSoruRepo().Update();
                return Json(new
                {
                    success = true,
                    message = "Guncelleme işlemi başarıyla gerçekleşti"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "Guncelleme işlemi sırasında bir hata oluştu"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Raporlama()
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
                Fotograflar = x.Fotograflar.Select(y => y.Path).ToList(),
                ID = x.ID,
                TeknisyenID = x.TeknisyenID
            }).ToList();

            return View(model);
        }
    }
}