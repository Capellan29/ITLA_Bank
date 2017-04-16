namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testingChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cliente", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Cliente", "User_Id");
            AddForeignKey("dbo.Cliente", "User_Id", "dbo.ApplicationUser", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cliente", "User_Id", "dbo.ApplicationUser");
            DropIndex("dbo.Cliente", new[] { "User_Id" });
            DropColumn("dbo.Cliente", "User_Id");
        }
    }
}
