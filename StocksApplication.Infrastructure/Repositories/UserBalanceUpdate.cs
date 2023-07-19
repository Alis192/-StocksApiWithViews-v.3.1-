using Entities;
using Microsoft.AspNetCore.Identity;
using StocksApplication.Core.Domain.IdentityEntities;
using StocksApplication.Core.Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Infrastructure.Repositories
{
    public class UserBalanceUpdate : IUserBalanceUpdate
    {
        private readonly OrdersDbContext _ordersDb;

        public UserBalanceUpdate(OrdersDbContext ordersDb)
        {
            _ordersDb = ordersDb;
        }


        public async Task UpdateBalance(ApplicationUser user, double? newBalance)
        {
            user.Balance = newBalance.Value;
            _ordersDb.Update(user); 
            await _ordersDb.SaveChangesAsync(); 
        }
    }
}
    