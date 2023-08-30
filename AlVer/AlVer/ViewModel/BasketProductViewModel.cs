using Microsoft.EntityFrameworkCore;

namespace AlVer.ViewModel
{
    public class BasketProductViewModel
    {
        public int BasketId { get; set; }
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string TotalPrice { get; set; }
    }
}
