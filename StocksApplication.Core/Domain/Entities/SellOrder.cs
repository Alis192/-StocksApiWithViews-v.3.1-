using StocksApplication.Core.Domain.IdentityEntities;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }

        [StringLength(10)]
        public string? StockSymbol { get; set; }

        [StringLength(30)]
        public string? StockName { get; set; }
        public DateTime? DateAndTimeOfOrder { get; set; }

        [Range(0, 100000)]
        public uint? Quantity { get; set; }

        [Range(0, 10000)]
        public double? Price { get; set; }

        public Guid UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
