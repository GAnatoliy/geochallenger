using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        #region Queries

        public async Task<UserDto> GetOrGetWithCreatingUserAsync(string oauthToken, AccountTypeDto accountTypeDto)
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

        public async Task<UserDto> GetUserAsync(string accountUid, AccountTypeDto accountTypeDto)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var account = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Accounts
                    .GetAccount(accountUid, _mapper.Map<AccountType>(accountTypeDto))
                    .SingleOrDefaultAsync();

                if (account == null) {
                    return null;
                }

                return _mapper.Map<UserDto>(await dbContextScope.DbContexts.Get<GeoChallengerContext>().Users.SingleOrDefaultAsync(u => u.Id == account.UserId));
            }
        }

        public async Task<UserDto> GetUserAsync(int userId)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var user = await dbContextScope.DbContexts.Get<GeoChallengerContext>().Users
                    .GetUser(userId)
                    .SingleOrDefaultAsync();

                if (user == null) {
                    return null;
                }

                return _mapper.Map<UserDto>(user);
            }
        }

        public async Task<IList<UserDto>> GetLeaderboardAsync(int take)
        {
            using (var dbContextScope = _dbContextScopeFactory.CreateReadOnly()) {
                var context = dbContextScope.DbContexts.Get<GeoChallengerContext>();

                var users = await context.Users
                    .OrderByDescending(u => u.Points)
                    .Take(take)
                    .ToListAsync();

                return _mapper.Map<List<UserDto>>(users);
            }
        }
        #endregion

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
