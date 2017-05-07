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

        public TodoMvcDbContext():base("name=LocalConnection")
        {

        }
        
    }
}