namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class transaccions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transaccion",
                c => new
                    {
                        TransaccionID = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, storeType: "money"),
                        Fecha = c.DateTime(nullable: false),
                        Comentario = c.String(maxLength: 100),
                        Tipo = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TransaccionID)
                .ForeignKey("dbo.Cliente", t => t.ClienteID, cascadeDelete: true)
                .Index(t => t.ClienteID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transaccion", "ClienteID", "dbo.Cliente");
            DropIndex("dbo.Transaccion", new[] { "ClienteID" });
            DropTable("dbo.Transaccion");
        }
    }
}
