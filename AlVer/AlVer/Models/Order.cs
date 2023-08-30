namespace AlVer.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int? BillId { get; set; }
        public virtual Bill? Bill { get; set; }
        public int? ProductId { get; set; }
        public virtual Product? Product { get; set; }
        public string? UserId{ get; set; }
        public virtual AppUser? User { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
