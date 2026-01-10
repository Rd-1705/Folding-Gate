using System;
using System.Collections.Generic;

namespace foldingGate.Models.DB
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime TanggalPesan { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalHarga { get; set; }
        public string Status { get; set; }

        public string? SnapToken { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}