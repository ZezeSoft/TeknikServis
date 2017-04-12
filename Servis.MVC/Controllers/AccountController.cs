using Servis.BLL.Account;
using Servis.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Servis.Entity.IdentityModels;
using Microsoft.Owin.Security;
using System.Net.Mail;
using System.Net;
using Servis.BLL.Repository;

namespace Servis.MVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userManager = MembershipTools.NewUserManager();
            var findUser = userManager.FindByName(model.UserName);

            if (findUser != null)
            {
                ModelState.AddModelError("", "Bu kullanıcı kaydı daha önce oluşturulmuştur");
                return View(model);
            }

            var user = new ApplicationUser()
            {
                Name = model.Name,
                SurName = model.Surname,
                UserName = model.UserName,
                Email = model.Email,
            };

            #region MailIslemleri
            string newpass = Guid.NewGuid().ToString();
            newpass = newpass.Replace("-", "");
            newpass = newpass.Substring(0, 6);

            ViewBag.pass = newpass;
            user.ActivationCode = newpass;

            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));  // replace with valid value 
            message.From = new MailAddress("crazywissen503@gmail.com");  // replace with valid value
            message.Subject = "Aktivasyon Kodunuz";
            message.Body = "Merhaba" + user.Name + "\n Aktivasyon Kodunuz: " + newpass;
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
            #endregion

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user.Id, "NotActivated");
                    return RedirectToAction("KayitOnay", "Account", new { id = model.UserName });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu");
                    return View(model);
                }
            }
        }

        public ActionResult KayitOnay(string id)
        {
            

            RegisterViewModel model = new RegisterViewModel()
            {
                UserName = id
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> KayitOnay(string userName, string EmailOnay)
        {

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var findUser = userManager.FindByName(userName);

            if (findUser == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı");
                return RedirectToAction("KayitOnay", "Account");
            }

            if (EmailOnay == findUser.ActivationCode)
            {
                await userStore.SetEmailConfirmedAsync(findUser, true);
                await userStore.UpdateAsync(findUser);
                await userStore.Context.SaveChangesAsync();

                await userManager.RemoveFromRoleAsync(findUser.Id, "NotActivated");
                await userManager.AddToRoleAsync(findUser.Id, "Üye");

                #region MailIslemleri

                var message = new MailMessage();
                message.To.Add(new MailAddress(findUser.Email));  // replace with valid value 
                message.From = new MailAddress("crazywissen503@gmail.com");  // replace with valid value
                message.Subject = "Sitemize Hoşgeldiniz";
                message.Body = "Üyeliğinizi aktive edilmiştir.Sitemize Hoşgeldiniz";
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
            else
            {
                ModelState.AddModelError(string.Empty, "Kayıt işlemi sırasında bir hata oluştu");
                return RedirectToAction("KayitOnay", "Account");
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userManager = MembershipTools.NewUserManager();
            var findUser = await userManager.FindAsync(model.UserName, model.Password);
            if (findUser == null)
            {
                ModelState.AddModelError(string.Empty, "Böyle bir kullanıcı bulunamadı");
                return View(model);
            }

            var authManager = HttpContext.GetOwinContext().Authentication;
            var userIdentity = await userManager.CreateIdentityAsync(findUser, DefaultAuthenticationTypes.ApplicationCookie);

            authManager.SignIn(new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
            }, userIdentity);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Recovery()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Recovery(RecoveryViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Profile");

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = userManager.FindByEmail(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Bu mail adresiyle birisi yok");
                return RedirectToAction("Login");
            }
            string newpass = Guid.NewGuid().ToString();
            newpass = newpass.Replace("-", "");
            newpass = newpass.Substring(0, 6);
            await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(newpass));
            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            var message = new MailMessage();
            message.To.Add(new MailAddress(user.Email));  // replace with valid value 
            message.From = new MailAddress("crazywissen503@gmail.com");  // replace with valid value
            message.Subject = "Yeni Parolanız";
            message.Body = "Merhaba" + user.Name + "\n Yeni Parolanız: " + newpass;
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

                return RedirectToAction("Login");
            }
        }

        [Authorize]
        public ActionResult Profile()
        {
            var userManager = MembershipTools.NewUserManager();
            var id = HttpContext.User.Identity.GetUserId();
            var user = userManager.FindById(id);
            ProfileViewModel model = new ProfileViewModel()
            {
                Email = user.Email,
                Name = user.Name,
                Surname = user.SurName,
                UserName = user.UserName,
                AvatarPath = user.AvatarPath
            };
            var arizalar = new ArizaRepo().GetAll().Where(x => x.KullaniciID == id).ToList();
            if (arizalar.Count > 0)
            {
                arizalar.ForEach(x =>
                model.Arizalar.Add(new KayitViewModel
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
                    Fotograflar = x.Fotograflar.Select(y => y.Path).ToList(),
                    TeknisyenID = x.TeknisyenID
                }));
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Profile");

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = userManager.FindById(HttpContext.User.Identity.GetUserId());

            user.Email = model.Email;
            user.Name = model.Name;
            user.SurName = model.Surname;

            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return RedirectToAction("Profile");

            var userStore = MembershipTools.NewUserStore();
            var userManager = new UserManager<ApplicationUser>(userStore);

            string userName = HttpContext.User.Identity.GetUserName();

            var user = userManager.Find(userName, model.OldPassword);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Mevcut şifreniz yanlış");
                return RedirectToAction("Profile");
            }

            await userStore.SetPasswordHashAsync(user, userManager.PasswordHasher.HashPassword(model.Password));
            await userStore.UpdateAsync(user);
            await userStore.Context.SaveChangesAsync();

            return RedirectToAction("Logout");
        }
    }
}