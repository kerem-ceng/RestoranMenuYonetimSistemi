using System.ComponentModel.DataAnnotations;

namespace RestaurantMenuManager.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kategori adı boş bırakılamaz")]
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
