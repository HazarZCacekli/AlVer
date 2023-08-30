using Microsoft.AspNetCore.Identity;

namespace AlVer.Models
{
    public class AppUser : IdentityUser
    {
        public string AdSoyad { get; set; }
        public DateTime Tarih { get; set; }
        public string? Address { get; set; }
        public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();
        public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
