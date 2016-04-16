using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Domains.Users;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Services.Providers.Interfaces;
using Mehdime.Entity;

namespace GeoChallenger.Services
{
    public class UsersService : IUsersService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly Dictionary<AccountTypeDto, ISocialNetworksProvider> _socialNetworksProviders;
        private readonly IMapper _mapper;

        public UsersService(IDbContextScopeFactory dbContextScopeFactory, Dictionary<AccountTypeDto, ISocialNetworksProvider> socialNetworksProviders, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _socialNetworksProviders = socialNetworksProviders;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserAsync(string oauthToken, AccountTypeDto accountTypeDto)
        {
            var accountType = _mapper.Map<AccountType>(accountTypeDto);
            var socialNetworkValidationData = await _socialNetworksProviders[accountTypeDto].ValidateCredentialsAsync(oauthToken);

            var account = await GetAccountAsync(socialNetworkValidationData.Uid, accountType);
            if (account == null) {
                return null;
            }

            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                return _mapper.Map<UserDto>(await dbContextScope.DbContexts.Get<GeoChallengerContext>().Users.FindAsync(account.UserId));
            }
        }

        #region Private Methods

        private async Task<Account> GetAccountAsync(string uid, AccountType type)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                return await dbContextScope.DbContexts.Get<GeoChallengerContext>().Accounts
                    .SingleOrDefaultAsync(a => a.Uid.Equals(uid, StringComparison.OrdinalIgnoreCase) && a.Type == type);
            }
        }

        private async Task VerifyAccountUniquess(string uniqueAccountSignature, AccountType accountType)
        {
            var account = await GetAccountAsync(uniqueAccountSignature, accountType);
            if (account != null) {
                throw new Exception($"User's {accountType} account already exist.");
            }
        }

        #endregion
    }
}
