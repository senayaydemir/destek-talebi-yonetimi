using DestekTalebiYonetimi.Data;
using Microsoft.AspNetCore.Mvc;

namespace DestekTalebiYonetimi.Controllers;

public class BildirimController : Controller
{
    private readonly AppDbContext _context;

    public BildirimController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
{
    var rol = HttpContext.Session.GetString("Rol");
    var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

    var bildirimler = _context.Bildirimler
        .Where(x =>
            x.Rol == rol ||
            x.KullaniciAdi == kullaniciAdi)
        .OrderByDescending(x => x.Tarih)
        .ToList();

    return View(bildirimler);
}

    [HttpGet]
    public IActionResult SonBildirimler()
    {
        var rol = HttpContext.Session.GetString("Rol");
        var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

        var bildirimler = _context.Bildirimler
           .Where(x =>
    x.Rol == rol ||
    x.KullaniciAdi == kullaniciAdi)
            .OrderByDescending(x => x.Tarih)
            .Take(5)
            .Select(x => new
            {
                x.Id,
                x.Mesaj,
                x.Tarih,
                x.Okundu,
                x.DestekTalebiId
            })
            .ToList();

        return Json(bildirimler);
    }

    [HttpPost]
    public IActionResult OkunduYap(int id)
    {
        var bildirim = _context.Bildirimler.FirstOrDefault(x => x.Id == id);

        if (bildirim == null)
        {
            return NotFound();
        }

        bildirim.Okundu = true;

        _context.SaveChanges();

        return Ok();
    }
[HttpPost]
public IActionResult TumunuOkunduYap()
{
    var rol = HttpContext.Session.GetString("Rol");
    var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

    var bildirimler = _context.Bildirimler
        .Where(x =>
            (x.Rol == rol || x.KullaniciAdi == kullaniciAdi)
            && !x.Okundu)
        .ToList();

    foreach (var bildirim in bildirimler)
    {
        bildirim.Okundu = true;
    }

    _context.SaveChanges();

    return RedirectToAction("Index");
}
[HttpPost]
public IActionResult Sil(int id)
{
    var rol = HttpContext.Session.GetString("Rol");
    var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

    var bildirim = _context.Bildirimler.FirstOrDefault(x =>
        x.Id == id &&
        (x.Rol == rol || x.KullaniciAdi == kullaniciAdi));

    if (bildirim == null)
    {
        return NotFound();
    }

    _context.Bildirimler.Remove(bildirim);

    _context.SaveChanges();

    return RedirectToAction("Index");
}
[HttpPost]
public IActionResult TumunuSil()
{
    var rol = HttpContext.Session.GetString("Rol");
    var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

    var bildirimler = _context.Bildirimler
        .Where(x => x.Rol == rol || x.KullaniciAdi == kullaniciAdi)
        .ToList();

    _context.Bildirimler.RemoveRange(bildirimler);

    _context.SaveChanges();

    return RedirectToAction("Index");
}
}