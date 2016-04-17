using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using GeoChallenger.Database;
using GeoChallenger.Domains.Users;
using GeoChallenger.Services.Interfaces;
using GeoChallenger.Services.Interfaces.DTO.Users;
using GeoChallenger.Services.Providers.DTO;
using GeoChallenger.Services.Providers.Interfaces;
using GeoChallenger.Services.Queries;
using Mehdime.Entity;

namespace GeoChallenger.Services
{
    public class UsersService : IUsersService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly Dictionary<AccountTypeDto, ISocialNetworksProvider> _socialNetworksProviders;
        private readonly IMapper _mapper;

        const string DefaultUserNameStub = "GeoChallenger User";

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

            using (var dbContextScope = _dbContextScopeFactory.Create()) {
                var user = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Users
                    .GetUser(socialNetworkValidationData.Email)
                    .SingleOrDefaultAsync();

                // User exist
                if (user != null) {
                    var account = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Accounts
                        .GetAccount(socialNetworkValidationData.Uid, accountType)
                        .SingleOrDefaultAsync();

                    // User exist, account exist
                    if (account != null) {
                        return _mapper.Map<UserDto>(user);
                    }

                    // User exist, create new account
                    dbContextScope.DbContexts.Get<GeoChallengerContext>().Accounts
                        .Add(CreateUserAccount(socialNetworkValidationData, user));

                    await dbContextScope.SaveChangesAsync();
                    return _mapper.Map<UserDto>(user);
                }

                user = new User {
                    Email = socialNetworkValidationData.Email,
                    Name = !string.IsNullOrEmpty(socialNetworkValidationData.Name) ? socialNetworkValidationData.Name : DefaultUserNameStub
                };

                dbContextScope.DbContexts.Get<GeoChallengerContext>().Accounts
                    .Add(CreateUserAccount(socialNetworkValidationData, user));
                dbContextScope.DbContexts.Get<GeoChallengerContext>().Users
                    .Add(user);

                await dbContextScope.SaveChangesAsync();
                return _mapper.Map<UserDto>(user);
            }
        }

        #region Private Methods

        private Account CreateUserAccount(SocialNetworkValidationData socialNetworkValidationData, User user)
        {
            var account = _mapper.Map<Account>(socialNetworkValidationData);
            account.UserId = user.Id;
            account.User = user;
            return account;
        }

        #endregion
    }
}
