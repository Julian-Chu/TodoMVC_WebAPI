using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TodoMVC_WebAPI.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Description { get; set; }
        public bool Completed { get; set; }

    }
}