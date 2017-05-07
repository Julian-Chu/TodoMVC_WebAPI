using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using TechTalk.SpecFlow;
using TodoMVC_WebAPI.Controllers.Api;
using TodoMVC_WebAPI.Models;

namespace TodoMVC_WebAPI.Tests.steps
{
    [Binding]
    public class TodoItemsControllerSteps
    {
        TodoMvcDbContext context = new TodoMvcDbContext();

        [Given(@"id equals (.*) for existing description")]
        public void GivenIdEqualsForExistingDescription(int id)
        {
            ScenarioContext.Current.Set<int>(id, "id");
        }

        [Given(@"id equals (.*) for non-existing description")]
        public void GivenIdEqualsForNon_ExistingDescription(int id)
        {
            ScenarioContext.Current.Set<int>(id, "id");
        }


        [When(@"it is retrieved")]
        public void WhenItIsRetrieved()
        {
            int id = ScenarioContext.Current.Get<int>("id");
            var controller = new TodoItemsController();
            var response = controller.GetTodoItem(id);
            ScenarioContext.Current.Set<IHttpActionResult>(response, "response");
            ScenarioContext.Current.Set<bool>(false, "IsGetAllItem");
        }

        [Then(@"Http status (.*) should be returned")]
        public void ThenHttpStatusShouldBeReturned(int httpStatusCode)
        {
            bool IsGetAllItem = ScenarioContext.Current.Get<bool>("IsGetAllItem");

            if (httpStatusCode == 200)
            {
                if (IsGetAllItem)
                {
                }
                else
                {
                    var response = ScenarioContext.Current.Get<IHttpActionResult>("response");
                    Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<TodoItem>));
                }

            }

            else if (httpStatusCode == 404)
            {
                var response = ScenarioContext.Current.Get<IHttpActionResult>("response");
                Assert.IsInstanceOfType(response, typeof(NotFoundResult));
            }
            else
            {
                Assert.Fail();
            }

        }

        [Then(@"Response context contains ""(.*)""")]
        public void ThenResponseContextContains(string description)
        {
            var response = ScenarioContext.Current.Get<IHttpActionResult>("response");
            var ContentResult = response as OkNegotiatedContentResult<TodoItem>;
            Assert.IsTrue(ContentResult.Content.Description.Contains(description));
        }

        [Given(@"existing issues")]
        public void GivenExistingIssues()
        {
        }

        [When(@"all items are retrieved")]
        public void WhenAllItemsAreRetrieved()
        {
            var controller = new TodoItemsController();
            var response = controller.GetTodoItems();
            ScenarioContext.Current.Set<IQueryable<TodoItem>>(response, "response");
            ScenarioContext.Current.Set<bool>(true, "IsGetAllItem");
        }

        [Then(@"all items are returned")]
        public void ThenAllItemsAreReturned()
        {
            var items = ScenarioContext.Current.Get<IQueryable<TodoItem>>("response");
            int expectedCount = 3;
            Assert.AreEqual(expectedCount, items.Count());
        }


    }
}
