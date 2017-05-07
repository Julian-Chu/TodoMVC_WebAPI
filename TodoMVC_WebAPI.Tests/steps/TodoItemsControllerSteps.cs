using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        }
        
        [Then(@"Http status (.*) should be returned")]
        public void ThenHttpStatusShouldBeReturned(int httpStatusCode)
        {
            if (httpStatusCode == 200)
            {
                var response = ScenarioContext.Current.Get<IHttpActionResult>("response");
                Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<TodoItem>));

            }
            else if(httpStatusCode == 404)
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

    }
}
