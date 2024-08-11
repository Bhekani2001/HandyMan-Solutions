namespace HandyMan_Solutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRegisteredDateToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "RegisteredDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "RegisteredDate");
        }
    }
}
