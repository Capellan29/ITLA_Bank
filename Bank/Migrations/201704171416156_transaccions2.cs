namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transaccions2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transaccion", "ClienteID", "dbo.Cliente");
            DropIndex("dbo.Transaccion", new[] { "ClienteID" });
            AddColumn("dbo.Transaccion", "CuentaID", c => c.Int(nullable: false));
            CreateIndex("dbo.Transaccion", "CuentaID");
            AddForeignKey("dbo.Transaccion", "CuentaID", "dbo.Cuenta", "CuentaID", cascadeDelete: true);
            DropColumn("dbo.Transaccion", "ClienteID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaccion", "ClienteID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Transaccion", "CuentaID", "dbo.Cuenta");
            DropIndex("dbo.Transaccion", new[] { "CuentaID" });
            DropColumn("dbo.Transaccion", "CuentaID");
            CreateIndex("dbo.Transaccion", "ClienteID");
            AddForeignKey("dbo.Transaccion", "ClienteID", "dbo.Cliente", "ClienteID", cascadeDelete: true);
        }
    }
}
