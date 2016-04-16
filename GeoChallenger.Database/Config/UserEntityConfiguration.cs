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
        }
    }
}