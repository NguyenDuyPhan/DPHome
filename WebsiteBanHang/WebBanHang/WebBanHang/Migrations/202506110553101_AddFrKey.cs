namespace WebBanHang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFrKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tb_Adv", "ProductId", c => c.Int());
            CreateIndex("dbo.tb_Adv", "ProductId");
            AddForeignKey("dbo.tb_Adv", "ProductId", "dbo.tb_Product", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tb_Adv", "ProductId", "dbo.tb_Product");
            DropIndex("dbo.tb_Adv", new[] { "ProductId" });
            DropColumn("dbo.tb_Adv", "ProductId");
        }
    }
}
