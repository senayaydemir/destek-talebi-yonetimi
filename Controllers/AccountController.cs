using DestekTalebiYonetimi.Data;
using DestekTalebiYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DestekTalebiYonetimi.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string kullaniciAdi, string sifre)
        {
            var kullanici = _context.Kullanicilar
                .FirstOrDefault(x => x.KullaniciAdi == kullaniciAdi && x.Sifre == sifre);

            if (kullanici == null)
            {
                ViewBag.Hata = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }
HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
HttpContext.Session.SetString("AdSoyad", kullanici.AdSoyad);
HttpContext.Session.SetString("Rol", kullanici.Rol);
_context.KullaniciLoglari.Add(new KullaniciLog
{
    KullaniciAdi = kullanici.KullaniciAdi,
    AdSoyad = kullanici.AdSoyad,
    Rol = kullanici.Rol,
    Islem = "Giriş Yaptı",
    Tarih = DateTime.Now,
    IpAdresi = HttpContext.Connection.RemoteIpAddress?.ToString()
});

_context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

     public IActionResult Logout()
{
    var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");
    var adSoyad = HttpContext.Session.GetString("AdSoyad");
    var rol = HttpContext.Session.GetString("Rol");

    if (!string.IsNullOrEmpty(kullaniciAdi))
    {
        _context.KullaniciLoglari.Add(new KullaniciLog
        {
            KullaniciAdi = kullaniciAdi,
            AdSoyad = adSoyad ?? "",
            Rol = rol ?? "",
            Islem = "Çıkış Yaptı",
            Tarih = DateTime.Now,
            IpAdresi = HttpContext.Connection.RemoteIpAddress?.ToString()
        });

        _context.SaveChanges();
    }

    HttpContext.Session.Clear();

    return RedirectToAction("Login");
}
    }
}