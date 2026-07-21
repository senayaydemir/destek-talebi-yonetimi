using Microsoft.AspNetCore.Mvc;
using DestekTalebiYonetimi.Data;
using DestekTalebiYonetimi.Models;

namespace DestekTalebiYonetimi.Controllers
{
    public class YonetimController : Controller
    {
        private readonly AppDbContext _context;

        public YonetimController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ToplamKullanici = _context.Kullanicilar.Count();
            ViewBag.ToplamTalep = _context.DestekTalepleri.Count();
            ViewBag.BekleyenTalep = _context.DestekTalepleri.Count(x => x.Durum == "Bekliyor");
            ViewBag.CozulenTalep = _context.DestekTalepleri.Count(x => x.Durum == "Çözüldü");

            return View();
        }

        public IActionResult Kullanicilar()
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
            {
                return RedirectToAction("Index", "Home");
            }

            var kullanicilar = _context.Kullanicilar
                .OrderBy(x => x.AdSoyad)
                .ToList();

            return View(kullanicilar);
        }

        // ==========================
        // KULLANICI EKLE
        // ==========================

        public IActionResult KullaniciEkle()
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public IActionResult KullaniciEkle(Kullanici kullanici)
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.Kullanicilar.Add(kullanici);
                _context.SaveChanges();

                return RedirectToAction("Kullanicilar");
            }

            return View(kullanici);
        }

        // ==========================
        // KULLANICI DÜZENLE
        // ==========================

        public IActionResult KullaniciDuzenle(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
                return RedirectToAction("Index", "Home");

            var kullanici = _context.Kullanicilar.Find(id);

            if (kullanici == null)
                return NotFound();

            return View(kullanici);
        }

        [HttpPost]
        public IActionResult KullaniciDuzenle(Kullanici kullanici)
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.Kullanicilar.Update(kullanici);
                _context.SaveChanges();

                return RedirectToAction("Kullanicilar");
            }

            return View(kullanici);
        }

        // ==========================
        // KULLANICI SİL
        // ==========================

        public IActionResult KullaniciSil(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Yonetici")
                return RedirectToAction("Index", "Home");

            var kullanici = _context.Kullanicilar.Find(id);

            if (kullanici == null)
                return NotFound();

            _context.Kullanicilar.Remove(kullanici);
            _context.SaveChanges();

            return RedirectToAction("Kullanicilar");
        }
    }
}