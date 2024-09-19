namespace HandyMan_Solutions.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveRegisteredDateColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "RegisteredDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "RegisteredDate", c => c.DateTime(nullable: false));
        }
    }
}
