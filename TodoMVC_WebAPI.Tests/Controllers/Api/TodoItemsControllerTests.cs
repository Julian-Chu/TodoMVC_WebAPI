using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using TodoMVC_WebAPI.Models;

namespace TodoMVC_WebAPI.Controllers.Api.Tests
{
    [TestClass()]
    public class TodoItemsControllerTests
    {
        [TestMethod()]
        public void GetTodoItems_Return_allItems()
        {
            //Assign
            StubTodoItemsControllers controller = new StubTodoItemsControllers();
            //Act
            var items = controller.GetTodoItems();

            int itemsCount = 3;

            //Assert
            Assert.IsInstanceOfType(items, typeof(IQueryable<TodoItem>));
            Assert.AreEqual(items.AsEnumerable().Count(), itemsCount);
        }

        [TestMethod()]
        public void GetTodoItem_ExistingItem_Return_OKwithItem()
        {
            //Assign
            StubTodoItemsControllers controller = new StubTodoItemsControllers();
            int id = 1;
            //Act
            var item = controller.GetTodoItem(id);
            var result = item as OkNegotiatedContentResult<TodoItem>;

            //Assert
            Assert.IsInstanceOfType(item, typeof(OkNegotiatedContentResult<TodoItem>));
            Assert.IsTrue(result.Content.Description.Contains("test description"));
        }

        [TestMethod()]
        public void GetTodoItem_NonExistingItem_Return_NotFound()
        {
            //Assign
            StubTodoItemsControllers controller = new StubTodoItemsControllers();
            int id = 100;
            //Act
            var items = controller.GetTodoItem(id);
            //Assert
            Assert.IsInstanceOfType(items, typeof(NotFoundResult));
        }

        [TestMethod()]
        public void PutTodoItemTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void PostTodoItem_Succeed()
        {
            //Assign
            StubTodoItemsControllers controller = new StubTodoItemsControllers();
            TodoItem item = new TodoItem() { Description = "test" };
            //Act
            var result = controller.PostTodoItem(item);
            //Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<TodoItem>));
            Assert.AreEqual(4 , controller.getItemCountInMockDB());
        }

        [TestMethod()]
        public void DeleteTodoItemTest()
        {
            Assert.Fail();
        }
    }

    public class StubTodoItemsControllers : TodoItemsController
    {
        private List<TodoItem> stubItems;
        private IDbSet<TodoItem> stubDbSet;
        
        public StubTodoItemsControllers()
        {
            var x = getTodoItems();
            stubItems = getTodoItems();
            var queryableItems = stubItems.AsQueryable();
            stubDbSet = Substitute.For<DbSet<TodoItem>, IDbSet<TodoItem>>();
            stubDbSet.Provider.Returns(queryableItems.Provider);
            stubDbSet.Expression.Returns(queryableItems.Expression);
            stubDbSet.ElementType.Returns(queryableItems.ElementType);
            stubDbSet.GetEnumerator().Returns(queryableItems.GetEnumerator());
            stubDbSet.Find(Arg.Any<int>()).Returns(callinfo =>
            {
                object[] idValues = callinfo.Arg<object[]>();
                int tempId = (int)idValues[0];
                return stubItems.FirstOrDefault(p => p.Id == tempId);
            });

            stubDbSet.Add(Arg.Do<TodoItem>(arg => stubItems.Add(arg)));


                db = Substitute.For<TodoMvcDbContext>();
            db.TodoItems.Returns(stubDbSet);
        }

        public List<TodoItem> getTodoItems()
        {
            return new List<TodoItem>()
            {
                new TodoItem { Id = 1, Description="test description 1", Completed= false },
                new TodoItem { Id = 2, Description = "test description 2", Completed = false },
                new TodoItem { Id = 3, Description="test description 3", Completed= false }
            };
        }

        public int getItemCountInMockDB()
        {
            return db.TodoItems.Count();
        }
    }
}