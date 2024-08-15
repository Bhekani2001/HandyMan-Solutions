namespace HandyMan_Solutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarOnboardings",
                c => new
                    {
                        caronboardingkey = c.Int(nullable: false, identity: true),
                        CarName = c.String(),
                        CarType = c.String(),
                        CarModel = c.String(),
                        CarMake = c.String(),
                        CarRegistrationNumber = c.String(),
                        AcquisitionDate = c.DateTime(nullable: false),
                        DiscExpiryDate = c.DateTime(nullable: false),
                        CarStatus = c.String(),
                    })
                .PrimaryKey(t => t.caronboardingkey);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        contactKey = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                        Reason = c.String(),
                        Service = c.String(),
                        Message = c.String(),
                        LoggedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.contactKey);
            
            CreateTable(
                "dbo.EmployeeOnBoardings",
                c => new
                    {
                        EmployeeKey = c.Int(nullable: false, identity: true),
                        EFirstName = c.String(),
                        ELastName = c.String(),
                        EFamilyName = c.String(),
                        EIdentityNumber = c.String(),
                        EContact = c.String(),
                        ESecondContact = c.String(),
                        EEmailAddress = c.String(),
                        EAddress = c.String(),
                        EStatus = c.String(),
                        EYearsofExperience = c.Int(nullable: false),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.EmployeeKey)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.QoutationRequests",
                c => new
                    {
                        qoutationKey = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ServiceType = c.String(),
                        Description = c.String(),
                        RequestedDate = c.DateTime(nullable: false),
                        AdditionalNotes = c.String(),
                        TechnicianAssigned = c.String(),
                        EstimatedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Paid = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.qoutationKey);
            
            CreateTable(
                "dbo.RequestQoutations",
                c => new
                    {
                        qoutationKey = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ServiceType = c.String(),
                        Description = c.String(),
                        RequestedDate = c.DateTime(nullable: false),
                        AdditionalNotes = c.String(),
                        TechnicianAssigned = c.String(),
                        EstimatedCost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Paid = c.String(),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.qoutationKey);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        FamilyName = c.String(),
                        Address = c.String(),
                        Status = c.String(),
                        Contact = c.String(),
                        SecondContact = c.String(),
                        Experience = c.Int(nullable: false),
                        IDNo = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegisteredDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.EmployeeOnBoardings", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.EmployeeOnBoardings", new[] { "RoleId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.RequestQoutations");
            DropTable("dbo.QoutationRequests");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.EmployeeOnBoardings");
            DropTable("dbo.Contacts");
            DropTable("dbo.CarOnboardings");
        }
    }
}
