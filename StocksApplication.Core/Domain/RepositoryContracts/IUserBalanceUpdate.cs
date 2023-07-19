using StocksApplication.Core.Domain.IdentityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.Domain.RepositoryContracts
{
    public interface IUserBalanceUpdate
    {
        Task UpdateBalance(ApplicationUser user, double? newBalance);
    }
}
