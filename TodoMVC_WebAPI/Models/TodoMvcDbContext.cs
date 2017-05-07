using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TodoMVC_WebAPI.Models
{
    public class TodoMvcDbContext:DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }

        public TodoMvcDbContext():base("name=LocalConnection"){}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(m => m.Description)
                .IsRequired()
                .HasMaxLength(255);
            
            base.OnModelCreating(modelBuilder);
        }


    }
}