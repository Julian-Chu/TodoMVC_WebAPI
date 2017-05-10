namespace TodoMVC_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeDescritpionColumnToNvarchar50InTodoItemsTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TodoItems", "Description", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TodoItems", "Description", c => c.String(nullable: false, maxLength: 255));
        }
    }
}
