﻿using System.Data.Entity;
using GeoChallenger.Database.Config;
using GeoChallenger.Domains.Challenges;
using GeoChallenger.Domains.Pois;
using GeoChallenger.Domains.Routes;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Database
{
    public class GeoChallengerContext: DbContext
    {
        public GeoChallengerContext() : base("GeoChallengerDb")
        {
            System.Data.Entity.Database.SetInitializer(new GeoChallengerInitializer());
        }

        public DbSet<Poi> Pois { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<PoiCheckin> PoiCheckins { get; set; }

        public DbSet<Challenge> Challenges { get; set; } 

        public DbSet<ChallengeAnswer> ChallengeAnswers { get; set; } 

        public DbSet<PoiMedia> PoiMedia { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserEntityConfiguration());
            modelBuilder.Configurations.Add(new RouteEntityConfiguration());
        }
    }
}