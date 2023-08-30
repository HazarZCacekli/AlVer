namespace AlVer.Models
{
    public class Basket
    {
        public int BasketId { get; set; }
        public string? UserId { get; set; }
        public int? ProductId { get; set; }
        public int Amount { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual Product? Product { get; set; }

    }
}
