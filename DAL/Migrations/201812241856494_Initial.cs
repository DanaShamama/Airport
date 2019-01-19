namespace DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlightDTOes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsDeparture = c.Boolean(nullable: false),
                        FlightGuidAndType = c.String(),
                        StartTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FlightsHistoryDTOes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FlightDTOId = c.Int(),
                        StationDTOId = c.Int(),
                        EnterStationTime = c.DateTime(nullable: false),
                        ExitStationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlightDTOes", t => t.FlightDTOId)
                .ForeignKey("dbo.StationDTOes", t => t.StationDTOId)
                .Index(t => t.FlightDTOId)
                .Index(t => t.StationDTOId);
            
            CreateTable(
                "dbo.StationDTOes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.Int(nullable: false),
                        FlightDTOId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlightDTOes", t => t.FlightDTOId)
                .Index(t => t.FlightDTOId);
            
            CreateTable(
                "dbo.FutureArrivalsDTOes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FlightDTOId = c.Int(),
                        IsEntered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlightDTOes", t => t.FlightDTOId)
                .Index(t => t.FlightDTOId);
            
            CreateTable(
                "dbo.FutureDeparturesDTOes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FlightDTOId = c.Int(),
                        IsEntered = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FlightDTOes", t => t.FlightDTOId)
                .Index(t => t.FlightDTOId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FutureDeparturesDTOes", "FlightDTOId", "dbo.FlightDTOes");
            DropForeignKey("dbo.FutureArrivalsDTOes", "FlightDTOId", "dbo.FlightDTOes");
            DropForeignKey("dbo.FlightsHistoryDTOes", "StationDTOId", "dbo.StationDTOes");
            DropForeignKey("dbo.StationDTOes", "FlightDTOId", "dbo.FlightDTOes");
            DropForeignKey("dbo.FlightsHistoryDTOes", "FlightDTOId", "dbo.FlightDTOes");
            DropIndex("dbo.FutureDeparturesDTOes", new[] { "FlightDTOId" });
            DropIndex("dbo.FutureArrivalsDTOes", new[] { "FlightDTOId" });
            DropIndex("dbo.StationDTOes", new[] { "FlightDTOId" });
            DropIndex("dbo.FlightsHistoryDTOes", new[] { "StationDTOId" });
            DropIndex("dbo.FlightsHistoryDTOes", new[] { "FlightDTOId" });
            DropTable("dbo.FutureDeparturesDTOes");
            DropTable("dbo.FutureArrivalsDTOes");
            DropTable("dbo.StationDTOes");
            DropTable("dbo.FlightsHistoryDTOes");
            DropTable("dbo.FlightDTOes");
        }
    }
}
