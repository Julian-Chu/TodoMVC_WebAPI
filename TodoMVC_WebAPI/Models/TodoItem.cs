using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoMVC_WebAPI.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; }

    }
}