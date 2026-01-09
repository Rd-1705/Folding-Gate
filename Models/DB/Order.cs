namespace foldingGate.Models.DB
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime TanggalPesan { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalHarga { get; set; }
        public string Status { get; set; }

        public Customer Customer { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
