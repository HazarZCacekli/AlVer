namespace AlVer.ViewModel
{
    public class OrdersViewModel
    {
        public int BillId { get; set; }
        public int? ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int OrdersAmount { get; set; }
        public string AmountPrice { get; set; }
    }
}
