namespace TaxManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class new1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdminUsers",
                c => new
                    {
                        AdminId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        LastLogin = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.AdminId);
            
            CreateTable(
                "dbo.ApplyForTaxes",
                c => new
                    {
                        BVN = c.String(nullable: false, maxLength: 128),
                        UserTin = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        TaxAmount = c.String(nullable: false),
                        CompnayName = c.String(nullable: false),
                        CompanyPhsicalAddress = c.String(nullable: false),
                        StaffId = c.String(nullable: false),
                        CurrentPosition = c.String(nullable: false),
                        CurrentSalary = c.String(nullable: false),
                        CompanyContactNumber = c.String(nullable: false),
                        CompanyWebsite = c.String(nullable: false),
                        Payment_Status = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BVN);
            
            CreateTable(
                "dbo.ContactInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        FeedBack = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PaymentRecords",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CardNumber = c.String(nullable: false, maxLength: 18),
                        Expired_Date = c.String(nullable: false),
                        Cv2 = c.String(nullable: false, maxLength: 3),
                        Pin = c.String(nullable: false, maxLength: 4),
                        Date = c.String(),
                        Amount = c.String(),
                        BVN = c.String(maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ApplyForTaxes", t => t.BVN)
                .ForeignKey("dbo.TaxRegistrations", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BVN)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TaxRegistrations",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        BVN = c.String(maxLength: 128),
                        Email = c.String(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 20),
                        PassworConfirm = c.String(nullable: false, maxLength: 20),
                        FullName = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        PhoneNo = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.ApplyForTaxes", t => t.BVN)
                .Index(t => t.BVN);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentRecords", "UserId", "dbo.TaxRegistrations");
            DropForeignKey("dbo.TaxRegistrations", "BVN", "dbo.ApplyForTaxes");
            DropForeignKey("dbo.PaymentRecords", "BVN", "dbo.ApplyForTaxes");
            DropIndex("dbo.TaxRegistrations", new[] { "BVN" });
            DropIndex("dbo.PaymentRecords", new[] { "UserId" });
            DropIndex("dbo.PaymentRecords", new[] { "BVN" });
            DropTable("dbo.TaxRegistrations");
            DropTable("dbo.PaymentRecords");
            DropTable("dbo.ContactInfoes");
            DropTable("dbo.ApplyForTaxes");
            DropTable("dbo.AdminUsers");
        }
    }
}
