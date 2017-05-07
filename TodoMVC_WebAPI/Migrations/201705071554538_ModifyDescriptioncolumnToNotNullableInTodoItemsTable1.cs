namespace TodoMVC_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyDescriptioncolumnToNotNullableInTodoItemsTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TodoItems", "Description", c => c.String(nullable: false, maxLength: 255));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TodoItems", "Description", c => c.String());
        }
    }
}
