namespace GeoChallenger.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialTestData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChallengeAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Answer = c.String(),
                        CorrectAnswer = c.String(),
                        IsCorrect = c.Boolean(nullable: false),
                        EarnedPoints = c.Int(nullable: false),
                        AnsweredAtUtc = c.DateTime(nullable: false),
                        ChallengeId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Challenges", t => t.ChallengeId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.ChallengeId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Challenges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Task = c.String(),
                        CorrectAnswer = c.String(),
                        PointsReward = c.Int(nullable: false),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        UpdatedAtUtc = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        PoiId = c.Int(nullable: false),
                        CreatorId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pois", t => t.PoiId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.CreatorId)
                .Index(t => t.PoiId)
                .Index(t => t.CreatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Challenges", "CreatorId", "dbo.Users");
            DropForeignKey("dbo.ChallengeAnswers", "UserId", "dbo.Users");
            DropForeignKey("dbo.Challenges", "PoiId", "dbo.Pois");
            DropForeignKey("dbo.ChallengeAnswers", "ChallengeId", "dbo.Challenges");
            DropIndex("dbo.Challenges", new[] { "CreatorId" });
            DropIndex("dbo.Challenges", new[] { "PoiId" });
            DropIndex("dbo.ChallengeAnswers", new[] { "UserId" });
            DropIndex("dbo.ChallengeAnswers", new[] { "ChallengeId" });
            DropTable("dbo.Challenges");
            DropTable("dbo.ChallengeAnswers");
        }
    }
}
