using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenuManager.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantMenuManager.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly RestaurantContext _context;

        public MenuItemsController(RestaurantContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        // GET: MenuItems
        // GET: MenuItems
        public async Task<IActionResult> Index(
            string search,
            int? categoryId,
            decimal? minPrice,
            decimal? maxPrice,
            int page = 1)
        {
            int pageSize = 5; // sayfa başına kaç kayıt

            var menuItems = _context.MenuItems
                .Include(m => m.Category)
                .AsQueryable();

            // 🔍 Arama
            if (!string.IsNullOrWhiteSpace(search))
                menuItems = menuItems.Where(m => m.Name.Contains(search));

            // 🗂️ Kategori filtresi
            if (categoryId.HasValue)
                menuItems = menuItems.Where(m => m.CategoryId == categoryId);

            // 💰 Fiyat aralığı
            if (minPrice.HasValue)
                menuItems = menuItems.Where(m => m.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                menuItems = menuItems.Where(m => m.Price <= maxPrice.Value);

            int totalCount = await menuItems.CountAsync();

            var items = await menuItems
                .OrderBy(m => m.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            ViewBag.Categories = _context.Categories.ToList();

            return View(items);
        }



        // GET: MenuItems/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: MenuItems/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            if (ModelState.IsValid)
            {
                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", menuItem.CategoryId);
            return View(menuItem);
        }

        // GET: MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", menuItem.CategoryId);
            return View(menuItem);
        }

        // POST: MenuItems/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem)
        {
            if (id != menuItem.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(menuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", menuItem.CategoryId);
            return View(menuItem);
        }

        // GET: MenuItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null)
                return NotFound();

            return View(menuItem);
        }

        // POST: MenuItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       
        

    }
}
