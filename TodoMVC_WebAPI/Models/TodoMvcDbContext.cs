using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TodoMVC_WebAPI.Models
{
    public class TodoMvcDbContext:DbContext
    {
        public virtual DbSet<TodoItem> TodoItems { get; set; }
                                            
        public TodoMvcDbContext():base("LocalConnection") {}
        public TodoMvcDbContext(string connnectionString):base(connnectionString)
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<TodoMvcDbContext>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(m => m.Description)
                .IsRequired();
            
            base.OnModelCreating(modelBuilder);
        }


    }
}