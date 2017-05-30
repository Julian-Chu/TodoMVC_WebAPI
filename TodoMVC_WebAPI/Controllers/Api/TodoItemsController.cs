using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using TodoMVC_WebAPI.Models;

namespace TodoMVC_WebAPI.Controllers.Api
{
    /// <summary>
    /// 
    /// </summary>
    public class TodoItemsController : ApiController
    {
        protected TodoMvcDbContext db = new TodoMvcDbContext("TestDbConnection");
        /// <summary>
        /// Get all todos
        /// </summary>
        /// <returns></returns>
        // GET: api/TodoItems
        public IQueryable<TodoItem> GetTodoItems()
        {
            return db.TodoItems;
        }
        /// <summary>
        /// // todo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/TodoItems/5
        [ResponseType(typeof(TodoItem))]
        public IHttpActionResult GetTodoItem(int id)
        {
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        /// <summary>
        /// // todo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoItem"></param>
        /// <returns></returns>
        // PUT: api/TodoItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTodoItem(int id, TodoItem todoItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            db.Entry(todoItem).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="todoItems"></param>
        /// <returns></returns>
        // POST: api/TodoItems
        [ResponseType(typeof(TodoItem[]))]
        [HttpPost]
        public IHttpActionResult PostTodoItems(TodoItem[] todoItems)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ClearTodoItemsTable();
            foreach (var item in todoItems)
            {
                db.TodoItems.Add(item);
            }
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { }, todoItems);
        }

        virtual protected void ClearTodoItemsTable()
        {
            db.Database.ExecuteSqlCommand("Truncate Table TodoItems");
        }

        /// <summary>
        /// // todo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/TodoItems/5
        [ResponseType(typeof(TodoItem))]
        public IHttpActionResult DeleteTodoItem(int id)
        {
            TodoItem todoItem = db.TodoItems.Find(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            db.TodoItems.Remove(todoItem);
            db.SaveChanges();

            return Ok(todoItem);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TodoItemExists(int id)
        {
            return db.TodoItems.Count(e => e.Id == id) > 0;
        }
    }
}