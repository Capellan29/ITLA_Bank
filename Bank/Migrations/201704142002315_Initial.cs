namespace Bank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cliente",
                c => new
                    {
                        ClienteID = c.Int(nullable: false, identity: true),
                        Nombre = c.String(maxLength: 30),
                        Apellido = c.String(maxLength: 30),
                        Cedula = c.String(nullable: false, maxLength: 13),
                        Direccion = c.String(maxLength: 100),
                        NumeroCuenta = c.Int(nullable: false),
                        Sexo = c.Int(nullable: false),
                        Edad = c.Int(nullable: false),
                        EstadoCivil = c.Int(nullable: false),
                        Correo = c.String(maxLength: 60),
                        Telefono = c.String(nullable: false, maxLength: 12),
                        Celular = c.String(maxLength: 12),
                    })
                .PrimaryKey(t => t.ClienteID);
            
            CreateTable(
                "dbo.Prestamo",
                c => new
                    {
                        PrestamoID = c.Int(nullable: false, identity: true),
                        Plazo = c.Int(nullable: false),
                        Monto = c.Decimal(nullable: false, storeType: "money"),
                        TasaInteres = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TasaMora = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Deuda = c.Decimal(nullable: false, storeType: "money"),
                        ProximoPago = c.DateTime(nullable: false),
                        Estado = c.Int(nullable: false),
                        GaranteID = c.Int(nullable: false),
                        ClienteID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrestamoID)
                .ForeignKey("dbo.Cliente", t => t.ClienteID, cascadeDelete: true)
                .Index(t => t.ClienteID);
            
            CreateTable(
                "dbo.Garante",
                c => new
                    {
                        GaranteID = c.Int(nullable: false),
                        Nombre = c.String(maxLength: 30),
                        Apellido = c.String(maxLength: 30),
                        Cedula = c.String(nullable: false, maxLength: 13),
                        Direccion = c.String(maxLength: 100),
                        Telefono = c.String(),
                        PrestamoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GaranteID)
                .ForeignKey("dbo.Prestamo", t => t.GaranteID)
                .Index(t => t.GaranteID);
            
            CreateTable(
                "dbo.PagoPrestamo",
                c => new
                    {
                        PagoPrestamoID = c.Int(nullable: false, identity: true),
                        Periodo = c.Int(nullable: false),
                        Cuota = c.Decimal(nullable: false, storeType: "money"),
                        Mora = c.Decimal(nullable: false, storeType: "money"),
                        Fecha = c.DateTime(nullable: false),
                        PrestamoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PagoPrestamoID)
                .ForeignKey("dbo.Prestamo", t => t.PrestamoID, cascadeDelete: true)
                .Index(t => t.PrestamoID);
            
            CreateTable(
                "dbo.IdentityRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserRole",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                        IdentityRole_Id = c.String(maxLength: 128),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RoleId, t.UserId })
                .ForeignKey("dbo.IdentityRole", t => t.IdentityRole_Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.IdentityRole_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentityUserClaim",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.IdentityUserLogin",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(),
                        ProviderKey = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplicationUser", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityUserRole", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserLogin", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserClaim", "ApplicationUser_Id", "dbo.ApplicationUser");
            DropForeignKey("dbo.IdentityUserRole", "IdentityRole_Id", "dbo.IdentityRole");
            DropForeignKey("dbo.PagoPrestamo", "PrestamoID", "dbo.Prestamo");
            DropForeignKey("dbo.Garante", "GaranteID", "dbo.Prestamo");
            DropForeignKey("dbo.Prestamo", "ClienteID", "dbo.Cliente");
            DropIndex("dbo.IdentityUserLogin", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserClaim", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.IdentityUserRole", new[] { "IdentityRole_Id" });
            DropIndex("dbo.PagoPrestamo", new[] { "PrestamoID" });
            DropIndex("dbo.Garante", new[] { "GaranteID" });
            DropIndex("dbo.Prestamo", new[] { "ClienteID" });
            DropTable("dbo.IdentityUserLogin");
            DropTable("dbo.IdentityUserClaim");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.IdentityUserRole");
            DropTable("dbo.IdentityRole");
            DropTable("dbo.PagoPrestamo");
            DropTable("dbo.Garante");
            DropTable("dbo.Prestamo");
            DropTable("dbo.Cliente");
        }
    }
}
