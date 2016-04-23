namespace GeoChallenger.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.String(),
                        Type = c.Byte(nullable: false),
                        AttachedAtUtc = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        Name = c.String(),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        Points = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PoiCheckins",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Points = c.Int(nullable: false),
                        CheckedInAtUtc = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        PoiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pois", t => t.PoiId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.PoiId);
            
            CreateTable(
                "dbo.Pois",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ContentPreview = c.String(),
                        Content = c.String(),
                        Address = c.String(),
                        Location = c.Geography(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
            CreateTable(
                "dbo.PoiMedias",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediaName = c.String(),
                        MediaType = c.Byte(nullable: false),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                        PoiId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pois", t => t.PoiId, cascadeDelete: true)
                .Index(t => t.PoiId);
            
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        StartPointLatitude = c.Double(nullable: false),
                        StartPointLongitude = c.Double(nullable: false),
                        EndPointLatitude = c.Double(nullable: false),
                        EndPointLongitude = c.Double(nullable: false),
                        DistanceInMeters = c.Double(nullable: false),
                        RoutePath = c.String(),
                        CreatedAtUtc = c.DateTime(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.RoutePois",
                c => new
                    {
                        Route_Id = c.Int(nullable: false),
                        Poi_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Route_Id, t.Poi_Id })
                .ForeignKey("dbo.Routes", t => t.Route_Id, cascadeDelete: true)
                .ForeignKey("dbo.Pois", t => t.Poi_Id, cascadeDelete: true)
                .Index(t => t.Route_Id)
                .Index(t => t.Poi_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routes", "UserId", "dbo.Users");
            DropForeignKey("dbo.Pois", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.PoiCheckins", "UserId", "dbo.Users");
            DropForeignKey("dbo.RoutePois", "Poi_Id", "dbo.Pois");
            DropForeignKey("dbo.RoutePois", "Route_Id", "dbo.Routes");
            DropForeignKey("dbo.PoiMedias", "PoiId", "dbo.Pois");
            DropForeignKey("dbo.PoiCheckins", "PoiId", "dbo.Pois");
            DropForeignKey("dbo.Accounts", "UserId", "dbo.Users");
            DropIndex("dbo.RoutePois", new[] { "Poi_Id" });
            DropIndex("dbo.RoutePois", new[] { "Route_Id" });
            DropIndex("dbo.Routes", new[] { "UserId" });
            DropIndex("dbo.PoiMedias", new[] { "PoiId" });
            DropIndex("dbo.Pois", new[] { "OwnerId" });
            DropIndex("dbo.PoiCheckins", new[] { "PoiId" });
            DropIndex("dbo.PoiCheckins", new[] { "UserId" });
            DropIndex("dbo.Accounts", new[] { "UserId" });
            DropTable("dbo.RoutePois");
            DropTable("dbo.Routes");
            DropTable("dbo.PoiMedias");
            DropTable("dbo.Pois");
            DropTable("dbo.PoiCheckins");
            DropTable("dbo.Users");
            DropTable("dbo.Accounts");
        }
    }
}
