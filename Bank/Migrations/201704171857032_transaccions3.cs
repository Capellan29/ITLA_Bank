namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transaccions3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transaccion", "CuentaIDDestinatario", c => c.Int());
            AddColumn("dbo.Transaccion", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Transaccion", "CuentaIDDestinatario");
            AddForeignKey("dbo.Transaccion", "CuentaIDDestinatario", "dbo.Cuenta", "CuentaID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaccion", "CuentaIDDestinatario", "dbo.Cuenta");
            DropIndex("dbo.Transaccion", new[] { "CuentaIDDestinatario" });
            DropColumn("dbo.Transaccion", "Discriminator");
            DropColumn("dbo.Transaccion", "CuentaIDDestinatario");
        }
    }
}
