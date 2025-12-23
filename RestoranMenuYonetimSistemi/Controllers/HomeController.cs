using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenuManager.Models;
using RestoranMenuYonetimSistemi.Models;
using System.Diagnostics;

namespace RestoranMenuYonetimSistemi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RestaurantContext _context;

        public HomeController(ILogger<HomeController> logger, RestaurantContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalCategories = await _context.Categories.CountAsync();
            var totalMenuItems = await _context.MenuItems.CountAsync();

            var avgPrice = totalMenuItems > 0
                ? await _context.MenuItems.AverageAsync(m => m.Price)
                : 0;

            var mostExpensive = await _context.MenuItems
                .OrderByDescending(m => m.Price)
                .FirstOrDefaultAsync();

            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalMenuItems = totalMenuItems;
            ViewBag.AvgPrice = avgPrice;
            ViewBag.MostExpensiveName = mostExpensive?.Name ?? "-";
            ViewBag.MostExpensivePrice = mostExpensive?.Price ?? 0;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Welcome()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}