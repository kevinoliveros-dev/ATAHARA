namespace AdminLteMvc.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Yard1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Yards", "yardStatus", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Yards", "yardStatus");
        }
    }
}
