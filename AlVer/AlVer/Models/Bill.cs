namespace AlVer.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public string? UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public string? Situation { get; set; }
        public virtual AppUser? User { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
