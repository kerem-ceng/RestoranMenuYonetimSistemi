using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantMenuManager.Models;
using System.Text.Json;

namespace RestaurantMenuManager.Controllers
{
    public class OrdersController : Controller
    {
        private readonly RestaurantContext _context;

        public OrdersController(RestaurantContext context)
        {
            _context = context;
        }

        // Tüm siparişleri listele
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }


        // Sepeti göster
        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        // Sepete ekle
        public IActionResult AddToCart(int menuItemId)
        {
            var menuItem = _context.MenuItems.FirstOrDefault(m => m.Id == menuItemId);
            if (menuItem == null)
                return NotFound();

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(c => c.MenuItemId == menuItemId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new OrderItem
                {
                    MenuItemId = menuItem.Id,
                    MenuItemName = menuItem.Name,
                    Quantity = 1,
                    UnitPrice = menuItem.Price
                });


            }

            SaveCart(cart);
            return RedirectToAction("Index", "MenuItems");
        }

        // Siparişi tamamla
        [HttpPost]
        public IActionResult Checkout(string customerName)
        {
            var cart = GetCart();
            if (!cart.Any())
                return RedirectToAction("Index", "MenuItems");

            var order = new Order
            {
                OrderDate = DateTime.Now,
                CustomerName = customerName,
                TotalPrice = cart.Sum(c => c.UnitPrice * c.Quantity),
                OrderItems = cart
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return View(order);
        }




        // ---- SESSION İŞLEMLERİ ----

        private List<OrderItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            if (string.IsNullOrEmpty(cartJson))
                return new List<OrderItem>();

            return JsonSerializer.Deserialize<List<OrderItem>>(cartJson);
        }

        private void SaveCart(List<OrderItem> cart)
        {
            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
        }

        // Sepetten ürün sil
        public IActionResult RemoveFromCart(int menuItemId)
        {
            var cart = GetCart();

            var itemToRemove = cart.FirstOrDefault(x => x.MenuItemId == menuItemId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SaveCart(cart);
            }

            return RedirectToAction("Cart");
        }

        // Siparişi iptal et
        public IActionResult Cancel(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        // Sepette ürün adedini 1 azalt
        public IActionResult DecreaseQuantity(int menuItemId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MenuItemId == menuItemId);
            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                {
                    cart.Remove(item);
                }

                SaveCart(cart);
            }

            return RedirectToAction("Cart");
        }
        // Sepette ürün adedini 1 artır
        public IActionResult IncreaseQuantity(int menuItemId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.MenuItemId == menuItemId);
            if (item != null)
            {
                item.Quantity++;
                SaveCart(cart);
            }

            return RedirectToAction("Cart");
        }
        // Sipariş düzenleme ekranı
        public IActionResult Edit(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }
        [HttpPost]
        public IActionResult Edit(Order model)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == model.Id);

            if (order == null)
                return NotFound();

            // müşteri adı güncelle
            order.CustomerName = model.CustomerName;

            // sipariş kalemlerini güncelle
            foreach (var item in order.OrderItems.ToList())
            {
                var updatedItem = model.OrderItems
                    .FirstOrDefault(x => x.Id == item.Id);

                if (updatedItem == null)
                {
                    // ürün silinmiş
                    _context.OrderItems.Remove(item);
                }
                else
                {
                    // adet güncelle
                    item.Quantity = updatedItem.Quantity;
                }
            }

            order.TotalPrice = order.OrderItems
                .Sum(x => x.UnitPrice * x.Quantity);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
