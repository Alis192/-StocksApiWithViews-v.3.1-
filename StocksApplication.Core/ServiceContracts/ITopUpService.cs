using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.ServiceContracts
{
    public interface ITopUpService
    {
        Task<bool> TopUpBalance(Guid userId, double amount);
    }
}
