using System.Collections.Generic;
using System.Data.Entity.Spatial;
using GeoChallenger.Database.Extensions;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using GeoChallenger.Domains.Challenges;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Routes;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Database.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<GeoChallengerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "GeoChallenger.Database.GeoChallengerContext";
        }

        protected override void Seed(GeoChallengerContext context)
        {
            //  This method will be called after migrating to the latest version.
            var coordinates = new List<KeyValuePair<string, DbGeography>> {
                new KeyValuePair<string, DbGeography>("Test location", GeoExtensions.CreateLocationPoint(48.534159, 32.275574)),
                new KeyValuePair<string, DbGeography>("Dobrovolskogo St, 1, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.534159, 32.275574)),
                new KeyValuePair<string, DbGeography>("Shevchenka St, 1, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.515507, 32.262109)),
                new KeyValuePair<string, DbGeography>( "Karla Marksa St, 24, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.513389, 32.269261)),
                new KeyValuePair<string, DbGeography>("ulitsa Evgeniya Chikalenko, 18, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.513653, 40.269458)),
                new KeyValuePair<string, DbGeography>("Kirovogradskiy natsionalnyy tekhnicheskiy universitet, просп. ”н≥верситетський, 8, Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.506374, 32.210647)),
                new KeyValuePair<string, DbGeography>("Kurhanna St, 56 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.491290, 32.249571)),
                new KeyValuePair<string, DbGeography>("Hoholya St, 109 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.506386, 32.270171)),
                new KeyValuePair<string, DbGeography>("ulitsa Kavaleriyskaya, 15 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.514064, 32.258405)),
                new KeyValuePair<string, DbGeography>("Dekabrystiv St, 27/58 Kirovohrad, Kirovohrads'ka oblast", GeoExtensions.CreateLocationPoint(48.509273, 32.261560)),
                new KeyValuePair<string, DbGeography>("Korolenka St, 1, Kirovohrad", GeoExtensions.CreateLocationPoint(48.530827, 32.289544))
            };

            var startEndPointsCoordinates = new Tuple<double, double, double, double>(48.507823, 32.196734, 48.545406, 32.309503);

            var users = CreateUsers(5);
            var pois = CreatePois(coordinates, users.First());

            // Create 7 challenges for first poi and first user.
            for (int i = 1; i <= 7; ++i)
            {
                context.Challenges.Add(CreateChallenge(i, users.First(), pois.First()));
            }

            context.Users.AddRange(users);
            context.Pois.AddRange(pois);

            context.SaveChanges();


            var routes = users
                .Select(user => CreateRoute($"{user.Name}_Route", user, pois.Take(5).ToList(), startEndPointsCoordinates))
                .ToList();
            context.Routes.AddRange(routes);
            context.SaveChanges();

            //base.Seed(context);
        }

        #region Private methods

        private static IList<User> CreateUsers(int count)
        {
            var users = new List<User>();
            for (var i = 1; i <= count; ++i)
            {
                users.Add(CreateUser(i, AccountType.Google));
                users.Add(CreateUser(i, AccountType.Facebook));
            }

            return users;
        }

        private static IList<Poi> CreatePois(List<KeyValuePair<string, DbGeography>> coordinates, User user)
        {
            var pois = new List<Poi>();
            for (var i = 1; i <= coordinates.Count; ++i)
            {
                pois.Add(CreatePoi(i, coordinates[i - 1].Key, coordinates[i - 1].Value, user));
            }

            return pois;
        }

        private static Route CreateRoute(string name, User user, IList<Poi> pois, Tuple<double, double, double, double> startEndPoints)
        {
            return new Route
            {
                Name = $"{nameof(Route.Name)}_{name}",
                StartPointLatitude = startEndPoints.Item1,
                StartPointLongitude = startEndPoints.Item2,
                EndPointLatitude = startEndPoints.Item3,
                EndPointLongitude = startEndPoints.Item4,
                DistanceInMeters = 10000,
                RoutePath = $"{nameof(Route.RoutePath)}{name}",
                User = user,
                Pois = pois
            };
        }

        private static Poi CreatePoi(int i, string address, DbGeography coordinates, User user)
        {
            var poi = new Poi
            {
                Title = $"Stub POI {i}",
                ContentPreview = $"Lorem {i} ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut",
                Content = $"Lorem {i} ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                Address = address,
                Location = coordinates
            };

            poi.AddOwner(user);

            return poi;
        }

        private static User CreateUser(int i, AccountType accountType)
        {
            var accounts = new Dictionary<AccountType, Account> {
                {AccountType.Google, new Account { Uid = $"{accountType}uid{i}".ToLower(), Type = AccountType.Google} },
                {AccountType.Facebook, new Account { Uid = $"{accountType}uid{i}".ToLower(), Type = AccountType.Facebook} }
            };

            return new User
            {
                Email = $"testuser{nameof(accountType)}{i}@example.com",
                Name = $"John Doe{i}",
                Accounts = new List<Account> { accounts[accountType] },
                Points = i * 10
            };
        }

        private static Challenge CreateChallenge(int index, User creator, Poi poi)
        {
            return new Challenge()
            {
                Task = $"Task {index}",
                CorrectAnswer = $"ansser {index}",
                PointsReward = 5,
                Creator = creator,
                Poi = poi
            };
        }

        #endregion
    }
}
