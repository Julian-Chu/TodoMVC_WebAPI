namespace TodoMVC_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CorrectColumnNameFromCompeletedToCompleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "Completed", c => c.Boolean(nullable: false));
            Sql("UPDATE TodoItems SET Completed = Compeleted");
            DropColumn("dbo.TodoItems", "Compeleted");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TodoItems", "Compeleted", c => c.Boolean(nullable: false));
            Sql("UPDATE TodoItems SET Compeleted = Completed");
            DropColumn("dbo.TodoItems", "Completed");
        }
    }
}
