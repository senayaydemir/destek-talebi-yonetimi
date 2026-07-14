using DestekTalebiYonetimi.Filters;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DestekTalebiYonetimi.Models;

namespace DestekTalebiYonetimi.Controllers;

[SessionAuthorize]
public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("KullaniciAdi") == null)
{
    return RedirectToAction("Login", "Account");
}
        ViewBag.Kullanici = HttpContext.Session.GetString("KullaniciAdi");
        ViewBag.Rol = HttpContext.Session.GetString("Rol");

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}