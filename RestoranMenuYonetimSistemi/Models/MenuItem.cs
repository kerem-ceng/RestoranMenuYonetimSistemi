using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantMenuManager.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Menü adı zorunludur!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fiyat zorunludur!")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Kategori")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }

}
