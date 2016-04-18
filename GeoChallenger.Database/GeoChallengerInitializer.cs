using System.Collections.Generic;
using System.Data.Entity;
using GeoChallenger.Database.Extensions;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Database
{
    /// <summary>
    ///     Seed base data.
    /// </summary>
    public class GeoChallengerInitializer: DropCreateDatabaseAlways<GeoChallengerContext>
    {
        protected override void Seed(GeoChallengerContext context)
        {
            var pois = new List<Poi> {
                new Poi { Id = 1, Title = "Stub POI 1", ContentPreview = "Lorem 1 ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut", Content = "Lorem 1 ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", Address = "Dobrovolskogo St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.534159, 32.275574) },
                new Poi { Id = 2, Title = "Stub POI 2", ContentPreview = "Lorem 2 ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut", Content = "Lorem 2 ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", Address = "Shevchenka St, 1, Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.515507, 32.262109) },
                new Poi { Id = 3, Title = "Stub POI 3", ContentPreview = "Lorem 3 ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut", Content = "Lorem 3 ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.", Address = "Kirovohrad, Kirovohrads'ka oblast, 25000", Location = GeoExtensions.CreateLocationPoint(48.500530, 32.232154) }
            };

            foreach (var poi in pois) {
                context.Pois.Add(poi);
            }

            UsersFactory(5, ref context);

            base.Seed(context);
        }
        
        #region Private methods

        private static void UsersFactory(int index, ref GeoChallengerContext context)
        {
            var users = new List<User>();
            for (var i = 1; i <= index; ++i) {
                users.Add(CreateUser(i, AccountType.Google));
                users.Add(CreateUser(i, AccountType.Facebook));
            }
            context.Users.AddRange(users);
        }

        private static User CreateUser(int i, AccountType accountType)
        {
            var accounts = new Dictionary<AccountType, Account> {
                {AccountType.Google, new Account { Uid = $"{nameof(accountType)}uid{i}".ToLower(), Type = AccountType.Google} },
                {AccountType.Facebook, new Account { Uid = $"{nameof(accountType)}uid{i}".ToLower(), Type = AccountType.Facebook} }
            };
            return new User {
                Email = $"testuser{nameof(accountType)}{i}@example.com",
                Name = $"John Doe{i}",
                Accounts = new List<Account> { accounts[accountType] }
            };
        }

        #endregion
    }
}