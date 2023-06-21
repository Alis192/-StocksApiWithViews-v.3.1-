using Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }

        public virtual ICollection<BuyOrder> BuyOrders { get; set; }
        
        public virtual ICollection<SellOrder> SellOrders { get; set; }

    }
}
