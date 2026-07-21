using DestekTalebiYonetimi.Filters;
using DestekTalebiYonetimi.Data;
using DestekTalebiYonetimi.Models;
using Microsoft.AspNetCore.SignalR;
using DestekTalebiYonetimi.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;
using System.IO;

namespace DestekTalebiYonetimi.Controllers;

[SessionAuthorize]
public class DestekController : Controller
{
    private readonly AppDbContext _context;
private readonly IHubContext<NotificationHub> _hubContext;

    public DestekController(
    AppDbContext context,
    IHubContext<NotificationHub> hubContext)
{
    _context = context;
    _hubContext = hubContext;
}

    public IActionResult Index(string? durum, string? arama)
    {
        var rol = HttpContext.Session.GetString("Rol");
        var kullanici = HttpContext.Session.GetString("KullaniciAdi");

        var taleplerQuery = _context.DestekTalepleri.AsQueryable();

        if (rol == "Personel")
        {
            taleplerQuery = taleplerQuery.Where(x => x.OlusturanKullanici == kullanici);
        }

        taleplerQuery = taleplerQuery.OrderByDescending(x => x.OlusturulmaTarihi);

        var tumTalepler = _context.DestekTalepleri.ToList();

        ViewBag.ToplamTalep = tumTalepler.Count;
        ViewBag.BekleyenTalep = tumTalepler.Count(x => x.Durum == "Bekliyor");
        ViewBag.IslemdekiTalep = tumTalepler.Count(x => x.Durum == "İşlemde");
        ViewBag.CozulenTalep = tumTalepler.Count(x => x.Durum == "Çözüldü");

        ViewBag.SeciliDurum = durum;
        ViewBag.Arama = arama;
        ViewBag.Rol = rol;
        ViewBag.Kullanici = kullanici;

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
    if (HttpContext.Session.GetString("KullaniciAdi") == null)
    {
        return RedirectToAction("Login", "Account");
    }

    if (HttpContext.Session.GetString("Rol") != "Personel")
    {
        return RedirectToAction("Index");
    }

    ViewBag.Kullanici = HttpContext.Session.GetString("KullaniciAdi");
    return View();
}
[HttpPost]
public async Task<IActionResult> Ekle(DestekTalebi destekTalebi, List<IFormFile>? dosyalar)
    {
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
        {
            return RedirectToAction("Login", "Account");
        }
        if (HttpContext.Session.GetString("Rol") != "Personel")
{
    return RedirectToAction("Index");
}

        if (!ModelState.IsValid)
        {
            return View(destekTalebi);
        }

      
destekTalebi.Durum = "Bekliyor";
destekTalebi.OlusturulmaTarihi = DateTime.Now;
destekTalebi.OlusturanKullanici = HttpContext.Session.GetString("KullaniciAdi");

_context.DestekTalepleri.Add(destekTalebi);
_context.SaveChanges();
_context.Bildirimler.Add(new Bildirim
{
    Rol = "BilgiIslem",
    Mesaj = $"{destekTalebi.TalepEden} tarafından yeni destek talebi oluşturuldu.",
    Okundu = false,
    Tarih = DateTime.Now,
    DestekTalebiId = destekTalebi.Id
});

_context.SaveChanges();
await _hubContext.Clients.All.SendAsync("YeniBildirim");
if (dosyalar != null)
{
    foreach (var dosya in dosyalar)
    {
        if (dosya.Length == 0)
            continue;

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads");

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(dosya.FileName);

        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            dosya.CopyTo(stream);
        }

        _context.DestekTalepDosyalari.Add(new DestekTalepDosya
        {
            DestekTalebiId = destekTalebi.Id,
            DosyaAdi = dosya.FileName,
            DosyaYolu = "/uploads/" + fileName,
            DosyaBoyutu = dosya.Length,
            DosyaTuru = Path.GetExtension(dosya.FileName),
            YuklenmeTarihi = DateTime.Now
        });
    }

    _context.SaveChanges();
}

return RedirectToAction("Index");
    }

public IActionResult Detay(int id)
{
    var rol = HttpContext.Session.GetString("Rol");
    var kullanici = HttpContext.Session.GetString("KullaniciAdi");

    var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

    if (talep == null)
    {
        return NotFound();
    }

    if (rol == "Personel" && talep.OlusturanKullanici != kullanici)
    {
        return RedirectToAction("Index");
    }
ViewBag.KullaniciAdi = kullanici;
    var viewModel = new DestekDetayViewModel
{
    Talep = talep,

    Loglar = _context.DestekTalepLoglari
        .Where(x => x.DestekTalebiId == id)
        .OrderByDescending(x => x.Tarih)
        .ToList(),

    Dosyalar = _context.DestekTalepDosyalari
        .Where(x => x.DestekTalebiId == id)
        .OrderBy(x => x.YuklenmeTarihi)
        .ToList(),
        Mesajlar = _context.Mesajlar
    .Where(x => x.DestekTalebiId == id)
    .OrderBy(x => x.Tarih)
    .ToList()
};

return View(viewModel);
}


       

    public IActionResult Duzenle(int id)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "BilgiIslem")
        {
            return RedirectToAction("Index");
        }

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
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "BilgiIslem")
        {
            return RedirectToAction("Index");
        }

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
LogEkle(mevcutTalep.Id, "Talep düzenlendi");
        return RedirectToAction("Detay", new { id = mevcutTalep.Id });
    }

    public IActionResult IslemeAl(int id)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "BilgiIslem")
        {
            return RedirectToAction("Index");
        }

        var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

        if (talep != null)
        {
            talep.Durum = "İşlemde";
            _context.SaveChanges();
            LogEkle(talep.Id, "Talep işleme alındı");
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult CozulduYap(int id, string cozumAciklamasi)
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "BilgiIslem")
        {
            return RedirectToAction("Index");
        }

        var talep = _context.DestekTalepleri.FirstOrDefault(x => x.Id == id);

        if (talep != null)
        {
            talep.Durum = "Çözüldü";
            talep.CozumAciklamasi = cozumAciklamasi ?? string.Empty;
            _context.SaveChanges();
            LogEkle(talep.Id, "Talep çözüldü");
            _context.Bildirimler.Add(new Bildirim
{
    KullaniciAdi = talep.OlusturanKullanici,
    Mesaj = $"'{talep.Baslik}' destek talebiniz çözüldü.",
    Okundu = false,
    Tarih = DateTime.Now,
    DestekTalebiId = talep.Id
});

_context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    public IActionResult ExceleAktar()
    {
        var rol = HttpContext.Session.GetString("Rol");

        if (rol != "BilgiIslem" && rol != "Yonetici")
        {
            return RedirectToAction("Index");
        }
        _context.KullaniciLoglari.Add(new KullaniciLog
{
    KullaniciAdi = HttpContext.Session.GetString("KullaniciAdi") ?? "",
    AdSoyad = HttpContext.Session.GetString("AdSoyad") ?? "",
    Rol = HttpContext.Session.GetString("Rol") ?? "",
    Islem = "Excel raporu aldı",
    Tarih = DateTime.Now,
    IpAdresi = HttpContext.Connection.RemoteIpAddress?.ToString()
});

_context.SaveChanges();

        var talepler = _context.DestekTalepleri
            .OrderByDescending(x => x.OlusturulmaTarihi)
            .ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Destek Talepleri");

            worksheet.Cell(1, 1).Value = "Talep No";
            worksheet.Cell(1, 2).Value = "Başlık";
            worksheet.Cell(1, 3).Value = "Birim";
            worksheet.Cell(1, 4).Value = "Talep Eden";
            worksheet.Cell(1, 5).Value = "İlgili Sistem";
            worksheet.Cell(1, 6).Value = "Talep Türü";
            worksheet.Cell(1, 7).Value = "Öncelik";
            worksheet.Cell(1, 8).Value = "Durum";
            worksheet.Cell(1, 9).Value = "Oluşturan Kullanıcı";
            worksheet.Cell(1, 10).Value = "Oluşturulma Tarihi";

            int satir = 2;

            foreach (var talep in talepler)
            {
                worksheet.Cell(satir, 1).Value = talep.Id;
                worksheet.Cell(satir, 2).Value = talep.Baslik;
                worksheet.Cell(satir, 3).Value = talep.Birim;
                worksheet.Cell(satir, 4).Value = talep.TalepEden;
                worksheet.Cell(satir, 5).Value = talep.IlgiliSistem;
                worksheet.Cell(satir, 6).Value = talep.TalepTuru;
                worksheet.Cell(satir, 7).Value = talep.Oncelik;
                worksheet.Cell(satir, 8).Value = talep.Durum;
                worksheet.Cell(satir, 9).Value = talep.OlusturanKullanici;
                worksheet.Cell(satir, 10).Value = talep.OlusturulmaTarihi.ToString("dd.MM.yyyy HH:mm");

                satir++;
            }

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);

                return File(
                    stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"DestekTalepleri_{DateTime.Now:yyyyMMddHHmmss}.xlsx");
            }
        }
    }
    [HttpPost]
[HttpPost]
public async Task<IActionResult> MesajGonder(int destekTalebiId, string mesaj)
{
    var kullanici = HttpContext.Session.GetString("KullaniciAdi");

    if (string.IsNullOrWhiteSpace(kullanici))
    {
        return RedirectToAction("Login", "Account");
    }

    if (string.IsNullOrWhiteSpace(mesaj))
    {
        return RedirectToAction("Detay", new { id = destekTalebiId });
    }

    _context.Mesajlar.Add(new Mesaj
    {
        DestekTalebiId = destekTalebiId,
        Gonderen = kullanici,
        MesajMetni = mesaj,
        Tarih = DateTime.Now
    });

    _context.SaveChanges();

    return RedirectToAction("Detay", new { id = destekTalebiId });
}
    private void LogEkle(int talepId, string islem)
{
    _context.DestekTalepLoglari.Add(new DestekTalepLog
    {
        DestekTalebiId = talepId,
        KullaniciAdi = HttpContext.Session.GetString("KullaniciAdi") ?? "",
        Islem = islem,
        Tarih = DateTime.Now
    });

    _context.SaveChanges();
}
}
