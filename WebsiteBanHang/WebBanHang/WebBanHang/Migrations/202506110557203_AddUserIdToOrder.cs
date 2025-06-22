namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserIdToOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Order", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.tb_Order", "UserId");
            AddForeignKey("dbo.tb_Order", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Order", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.tb_Order", new[] { "UserId" });
            DropColumn("dbo.tb_Order", "UserId");
        }
    }
}
