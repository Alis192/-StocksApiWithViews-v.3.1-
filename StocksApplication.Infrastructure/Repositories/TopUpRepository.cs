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
    public class TopUpRepository : ITopUpRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TopUpRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> TopUpBalanceRepository(Guid userId, double amount)
        {
            string userIdString = userId.ToString();

            ApplicationUser user = await _userManager.FindByIdAsync(userIdString);


            if (user != null)
            {
                user.Balance += amount;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
