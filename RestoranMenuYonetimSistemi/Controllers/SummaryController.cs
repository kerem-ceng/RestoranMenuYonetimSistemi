using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenuManager.Models;

public class SummaryController : Controller
{
    private readonly RestaurantContext _context;

    public SummaryController(RestaurantContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        DateTime last24Hours = DateTime.Now.AddDays(-1);

        var newCategories = _context.Categories
            .Where(c => c.CreatedAt >= last24Hours)
            .ToList();

        var newMenuItems = _context.MenuItems
            .Where(m => m.CreatedAt >= last24Hours)
            .ToList();

        var todaysOrders = _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.OrderDate >= last24Hours)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        ViewBag.NewCategories = newCategories;
        ViewBag.NewMenuItems = newMenuItems;
        ViewBag.TodaysOrders = todaysOrders;

        return View();
    }
}
