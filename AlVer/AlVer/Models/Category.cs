namespace AlVer.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string Image { get; set; }
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
