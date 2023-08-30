namespace AlVer.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string? Image2 { get; set; }
        public string? Image3 { get; set; }
        public string? Image4 { get; set; }
        public string? Image5 { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
