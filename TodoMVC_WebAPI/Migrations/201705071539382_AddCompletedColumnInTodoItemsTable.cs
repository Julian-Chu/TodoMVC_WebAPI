namespace TodoMVC_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompletedColumnInTodoItemsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TodoItems", "Compeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TodoItems", "Compeleted");
        }
    }
}
