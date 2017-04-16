namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientToUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cliente", "User_Id", "dbo.ApplicationUser");
            DropIndex("dbo.Cliente", new[] { "User_Id" });
            AddColumn("dbo.ApplicationUser", "Cliente_ClienteID", c => c.Int());
            CreateIndex("dbo.ApplicationUser", "Cliente_ClienteID");
            AddForeignKey("dbo.ApplicationUser", "Cliente_ClienteID", "dbo.Cliente", "ClienteID");
            DropColumn("dbo.Cliente", "User_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cliente", "User_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUser", "Cliente_ClienteID", "dbo.Cliente");
            DropIndex("dbo.ApplicationUser", new[] { "Cliente_ClienteID" });
            DropColumn("dbo.ApplicationUser", "Cliente_ClienteID");
            CreateIndex("dbo.Cliente", "User_Id");
            AddForeignKey("dbo.Cliente", "User_Id", "dbo.ApplicationUser", "Id");
        }
    }
}
