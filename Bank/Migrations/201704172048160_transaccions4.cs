namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transaccions4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transaccion", "CuentaIDDestinatario", "dbo.Cuenta");
            DropIndex("dbo.Transaccion", new[] { "CuentaIDDestinatario" });
            AddColumn("dbo.Transaccion", "CuentaIDFrom", c => c.Int(nullable: false));
            DropColumn("dbo.Transaccion", "CuentaIDDestinatario");
            DropColumn("dbo.Transaccion", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transaccion", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Transaccion", "CuentaIDDestinatario", c => c.Int());
            DropColumn("dbo.Transaccion", "CuentaIDFrom");
            CreateIndex("dbo.Transaccion", "CuentaIDDestinatario");
            AddForeignKey("dbo.Transaccion", "CuentaIDDestinatario", "dbo.Cuenta", "CuentaID", cascadeDelete: true);
        }
    }
}
