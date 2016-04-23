using System.Data.Entity.ModelConfiguration;
using GeoChallenger.Domains.Challenges;


namespace GeoChallenger.Database.Config
{
    public class ChallengeEntityConfiguration: EntityTypeConfiguration<Challenge>
    {
        public ChallengeEntityConfiguration()
        {
            // Challenge has list of answers given by users.
            HasMany(c => c.Answers)
                .WithRequired(a => a.Challenge)
                .HasForeignKey(a => a.ChallengeId);
        }
    }
}