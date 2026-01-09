namespace foldingGate.Models.DB
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal Harga { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
