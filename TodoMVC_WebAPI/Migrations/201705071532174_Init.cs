namespace TodoMVC_WebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TodoItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);

            Sql("Insert into TodoItems VALUES('test description 1')");
            Sql("Insert into TodoItems VALUES('test description 2')");
            Sql("Insert into TodoItems VALUES('test description 3')");
        }

        public override void Down()
        {
            DropTable("dbo.TodoItems");
        }
    }
}
