using DestekTalebiYonetimi.Data;
using DestekTalebiYonetimi.Models;
using Microsoft.AspNetCore.Mvc;

namespace DestekTalebiYonetimi.Controllers;

public class DestekController : Controller
{
    private readonly AppDbContext _context;

    public DestekController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string? durum, string? arama)
    {
        var taleplerQuery = _context.DestekTalepleri
            .OrderByDescending(x => x.OlusturulmaTarihi)
            .AsQueryable();

        var tumTalepler = _context.DestekTalepleri.ToList();

        ViewBag.ToplamTalep = tumTalepler.Count;
        ViewBag.BekleyenTalep = tumTalepler.Count(x => x.Durum == "Bekliyor");
        ViewBag.IslemdekiTalep = tumTalepler.Count(x => x.Durum == "İşlemde");
        ViewBag.CozulenTalep = tumTalepler.Count(x => x.Durum == "Çözüldü");

        ViewBag.SeciliDurum = durum;
        ViewBag.Arama = arama;

        if (!string.IsNullOrEmpty(durum))
        {
            taleplerQuery = taleplerQuery.Where(x => x.Durum == durum);
        }

        if (!string.IsNullOrWhiteSpace(arama))
        {
            taleplerQuery = taleplerQuery.Where(x =>
                x.Baslik.Contains(arama) ||
                x.Aciklama.Contains(arama) ||
                x.Birim.Contains(arama) ||
                x.TalepEden.Contains(arama) ||
                x.IlgiliSistem.Contains(arama) ||
                x.TalepTuru.Contains(arama));
        }

        var talepler = taleplerQuery.ToList();

        return View(talepler);
    }

    public IActionResult Ekle()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Ekle(DestekTalebi destekTalebi)
    {
        if (!ModelState.IsValid)
        {
            return View(destekTalebi);
        }

        destekTalebi.Durum = "Bekliyor";
        destekTalebi.OlusturulmaTarihi = DateTime.Now;

        _context.DestekTalepleri.Add(destekTalebi);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Detay(int id)
    {
        var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

        if (talep == null)
        {
            return NotFound();
        }

        return View(talep);
    }

    public IActionResult Duzenle(int id)
    {
        var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

        if (talep == null)
        {
            return NotFound();
        }

        return View(talep);
    }

    [HttpPost]
    public IActionResult Duzenle(DestekTalebi guncelTalep)
    {
        if (!ModelState.IsValid)
        {
            return View(guncelTalep);
        }

        var mevcutTalep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == guncelTalep.Id);

        if (mevcutTalep == null)
        {
            return NotFound();
        }

        mevcutTalep.Baslik = guncelTalep.Baslik;
        mevcutTalep.Aciklama = guncelTalep.Aciklama;
        mevcutTalep.Birim = guncelTalep.Birim;
        mevcutTalep.TalepEden = guncelTalep.TalepEden;
        mevcutTalep.DahiliNumara = guncelTalep.DahiliNumara;
        mevcutTalep.IlgiliSistem = guncelTalep.IlgiliSistem;
        mevcutTalep.TalepTuru = guncelTalep.TalepTuru;
        mevcutTalep.Oncelik = guncelTalep.Oncelik;

        _context.SaveChanges();

        return RedirectToAction("Detay", new { id = mevcutTalep.Id });
    }

    public IActionResult IslemeAl(int id)
    {
        var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

        if (talep != null)
        {
            talep.Durum = "İşlemde";
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CozulduYap(int id, string cozumAciklamasi)
    {
        var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

        if (talep != null)
        {
            talep.Durum = "Çözüldü";
            talep.CozumAciklamasi = cozumAciklamasi ?? string.Empty;
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}