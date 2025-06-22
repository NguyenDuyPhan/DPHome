namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSchema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tb_Order", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.tb_Order", new[] { "UserId" });
            DropColumn("dbo.tb_Order", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tb_Order", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.tb_Order", "UserId");
            AddForeignKey("dbo.tb_Order", "UserId", "dbo.AspNetUsers", "Id");
        }
    }
}
