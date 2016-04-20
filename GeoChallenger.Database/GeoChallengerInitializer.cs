﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Linq;
using GeoChallenger.Database.Extensions;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Routes;
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
            var coordinates = new List<KeyValuePair<string, DbGeography>> {
                new KeyValuePair<string, DbGeography>("Test location", GeoExtensions.CreateLocationPoint(48.534159, 40.275574)),
                new KeyValuePair<string, DbGeography>("Dobrovolskogo St, 1, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.534159, 32.275574)),
                new KeyValuePair<string, DbGeography>("Shevchenka St, 1, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.515507, 32.262109)),
                new KeyValuePair<string, DbGeography>( "Karla Marksa St, 24, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.513389, 32.269261)),
                new KeyValuePair<string, DbGeography>("ulitsa Evgeniya Chikalenko, 18, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.513653, 32.269458)),
                new KeyValuePair<string, DbGeography>("Kirovogradskiy natsionalnyy tekhnicheskiy universitet, просп. Університетський, 8, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.506374, 32.210647)),
                new KeyValuePair<string, DbGeography>("Kurhanna St, 56 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.491290, 32.249571)),
                new KeyValuePair<string, DbGeography>("Hoholya St, 109 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.506386, 32.270171)),
                new KeyValuePair<string, DbGeography>("ulitsa Kavaleriyskaya, 15 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.514064, 32.258405)),
                new KeyValuePair<string, DbGeography>("Dekabrystiv St, 27/58 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.509273, 32.261560)),
                new KeyValuePair<string, DbGeography>("Korolenka St, 1, Kirovohrad", GeoExtensions.CreateLocationPoint(48.530827, 32.289544))
            };

            UsersFactory(5, ref context);
            PoisFactory(coordinates, ref context);
            context.SaveChanges();

            var users = context.Users.ToList();
            var routes = new HashSet<Route>();
            foreach (var user in users) {
                var pois = context.Pois.Take(5).ToList();
                routes.Add(CreateRoute($"{user.Name}_Route", user, pois));
            }
            context.Routes.AddRange(routes);

            base.Seed(context);
        }
        
        #region Private methods

        private static void UsersFactory(int count, ref GeoChallengerContext context)
        {
            var users = new List<User>();
            for (var i = 1; i <= count; ++i) {
                users.Add(CreateUser(i, AccountType.Google));
                users.Add(CreateUser(i, AccountType.Facebook));
            }
            context.Users.AddRange(users);
        }

        private static void PoisFactory(List<KeyValuePair<string, DbGeography>> coordinates, ref GeoChallengerContext context)
        {
            var pois = new List<Poi>();
            for (var i = 1; i <= coordinates.Count; ++i) {
                pois.Add(CreatePoi(i, coordinates[i-1].Key, coordinates[i-1].Value));
            }
            context.Pois.AddRange(pois);
        }

        private static Route CreateRoute(string name, User user, IList<Poi> pois)
        {
            return new Route {
                Name = $"{nameof(Route)}{nameof(Route.Name)}_{name}",
                StartPoint = $"{nameof(Route.StartPoint)}{name}",
                EndPoint = $"{nameof(Route.EndPoint)}{name}",
                User = user,
                Pois = pois
            };
        }

        private static Poi CreatePoi(int i, string address, DbGeography coordinates)
        {
            return new Poi {
                Title = $"Stub POI {i}",
                ContentPreview = $"Lorem {i} ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut",
                Content = $"Lorem {i} ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Address = address,
                Location = coordinates
            };
        }

        private static User CreateUser(int i, AccountType accountType)
        {
            var accounts = new Dictionary<AccountType, Account> {
                {AccountType.Google, new Account { Uid = $"{accountType}uid{i}".ToLower(), Type = AccountType.Google} },
                {AccountType.Facebook, new Account { Uid = $"{accountType}uid{i}".ToLower(), Type = AccountType.Facebook} }
            };

            return new User {
                Email = $"testuser{nameof(accountType)}{i}@example.com",
                Name = $"John Doe{i}",
                Accounts = new List<Account> { accounts[accountType] },
            };
        }

        #endregion
    }
}