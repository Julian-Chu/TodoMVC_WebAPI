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
            var item = controller.GetTodoItem(id) as OkNegotiatedContentResult<TodoItem>;

            //Assert
            Assert.IsInstanceOfType(item, typeof(OkNegotiatedContentResult<TodoItem>));
            Assert.IsTrue(item.Content.Description.Contains("test description"));
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
            Assert.Inconclusive();
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
            Assert.AreEqual(4, controller.getItemCountInMockDB());
        }

        [TestMethod]
        public void PostTodoItem_invalidedModel_Return_InvalidModelStateResult()
        {
            //Assign
            StubTodoItemsControllers controller = new StubTodoItemsControllers();
            controller.ModelState.AddModelError("invalid", "Invalid Model");
            TodoItem item = new TodoItem() { Description = "test" };
            //Act
            var result = controller.PostTodoItem(item) as InvalidModelStateResult;

            //Assert
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
            Assert.AreEqual(3, controller.getItemCountInMockDB());
        }

        [TestMethod()]
        public void DeleteTodoItemTest()
        {
            Assert.Inconclusive();
        }
    }

    public class StubTodoItemsControllers : TodoItemsController
    {
        private List<TodoItem> mockItems;
        private IDbSet<TodoItem> mockDbSet;

        public StubTodoItemsControllers()
        {
            var x = getTodoItems();
            mockItems = getTodoItems();
            var queryableItems = mockItems.AsQueryable();
            mockDbSet = Substitute.For<DbSet<TodoItem>, IDbSet<TodoItem>>();
            mockDbSet.Provider.Returns(queryableItems.Provider);
            mockDbSet.Expression.Returns(queryableItems.Expression);
            mockDbSet.ElementType.Returns(queryableItems.ElementType);
            mockDbSet.GetEnumerator().Returns(queryableItems.GetEnumerator());
            mockDbSet.Find(Arg.Any<int>()).Returns(callinfo =>
            {
                object[] idValues = callinfo.Arg<object[]>();
                int tempId = (int)idValues[0];
                return mockItems.FirstOrDefault(p => p.Id == tempId);
            });

            mockDbSet.Add(Arg.Do<TodoItem>(arg => mockItems.Add(arg)));

            db = Substitute.For<TodoMvcDbContext>();
            db.TodoItems.Returns(mockDbSet);
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