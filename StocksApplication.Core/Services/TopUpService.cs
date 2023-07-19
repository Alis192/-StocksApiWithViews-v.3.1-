using Microsoft.AspNetCore.Identity;
using StocksApplication.Core.Domain.IdentityEntities;
using StocksApplication.Core.Domain.RepositoryContracts;
using StocksApplication.Core.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksApplication.Core.Services
{
    public class TopUpService : ITopUpService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITopUpRepository _topUpRepository;

        public TopUpService(UserManager<ApplicationUser> userManager, ITopUpRepository topUpRepository)
        {
            _userManager = userManager;
            _topUpRepository = topUpRepository;
        }

        public async Task<bool> TopUpBalance(Guid userId, double amount)
        {
            bool result = await _topUpRepository.TopUpBalanceRepository(userId, amount);

            return result;

        }
    }
}
