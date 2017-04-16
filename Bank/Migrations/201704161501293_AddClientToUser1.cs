namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientToUser1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUser", "Cliente_ClienteID", "dbo.Cliente");
            DropIndex("dbo.ApplicationUser", new[] { "Cliente_ClienteID" });
            RenameColumn(table: "dbo.ApplicationUser", name: "Cliente_ClienteID", newName: "ClienteID");
            AlterColumn("dbo.ApplicationUser", "ClienteID", c => c.Int(nullable: true));
            CreateIndex("dbo.ApplicationUser", "ClienteID");
            AddForeignKey("dbo.ApplicationUser", "ClienteID", "dbo.Cliente", "ClienteID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApplicationUser", "ClienteID", "dbo.Cliente");
            DropIndex("dbo.ApplicationUser", new[] { "ClienteID" });
            AlterColumn("dbo.ApplicationUser", "ClienteID", c => c.Int());
            RenameColumn(table: "dbo.ApplicationUser", name: "ClienteID", newName: "Cliente_ClienteID");
            CreateIndex("dbo.ApplicationUser", "Cliente_ClienteID");
            AddForeignKey("dbo.ApplicationUser", "Cliente_ClienteID", "dbo.Cliente", "ClienteID");
        }
    }
}
