﻿using System.Data.Entity.ModelConfiguration;
using GeoChallenger.Domains.Users;

namespace GeoChallenger.Database.Config
{
    public class UserEntityConfiguration : EntityTypeConfiguration<User>
    {
        public UserEntityConfiguration()
        {
            // Many-to-one relationship (Accounts-User)
            HasMany(u => u.Accounts)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId);

            // One-to-many relationship (User-Route)
            HasMany(u => u.Routes)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);

            // User has created by him pois.
            HasMany(u => u.Pois)
                .WithRequired(a => a.Owner)
                .HasForeignKey(a => a.OwnerId)
                .WillCascadeOnDelete(false);

            // User has list of the checked in pois.
            HasMany(u => u.Checkins)
                .WithRequired(c => c.User)
                .HasForeignKey(c => c.UserId);

            // User has created by him challenges.
            HasMany(u => u.CreatedChallenges)
                .WithRequired(c => c.Creator)
                .HasForeignKey(c => c.CreatorId)
                .WillCascadeOnDelete(false);

            // User has list of answers to challenges.
            HasMany(u => u.Answers)
                .WithRequired(a => a.User)
                .HasForeignKey(a => a.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}