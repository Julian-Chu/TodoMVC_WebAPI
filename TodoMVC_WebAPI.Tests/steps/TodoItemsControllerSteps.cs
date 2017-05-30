using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TechTalk.SpecFlow;
using TodoMVC_WebAPI.Models;

namespace TodoMVC_WebAPI.Tests.steps
{

    public class JsonTodoIems
    {
        public TodoItem todoItem { get; set; }
    }


    [Binding]
    public class TodoItemsControllerSteps
    {
        private static HttpClient client = new HttpClient();

        private readonly string localUrl = "http://localhost:2000/api";

        private TodoMvcDbContext context;

        [BeforeScenario]
        public void ClearAndAddNewDataIntoTestDB()
        {
            using (context = new TodoMvcDbContext("TestDbConnection"))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE TodoItems");
                context.Database.ExecuteSqlCommand("Insert into TodoItems VALUES('test description 1', 0)");
                context.Database.ExecuteSqlCommand("INSERT INTO TodoItems Values('test description 2', 0)");
                context.Database.ExecuteSqlCommand("INSERT INTO TodoItems Values('test description 3', 0)");
                context.Database.ExecuteSqlCommand("INSERT INTO TodoItems Values('test description 4', 0)");
            }
        }

        [AfterScenario]
        public void ClearTestDB()
        {
            using (context = new TodoMvcDbContext("TestDbConnection"))
            {
                context.Database.ExecuteSqlCommand("TRUNCATE TABLE TodoItems");
            }
        }



        [When(@"it is retrieved")]
        public void WhenItIsRetrieved()
        {
            int Id = ScenarioContext.Current.Get<int>("Id");

            var response = client.GetAsync($"{localUrl}/TodoItems/{Id}").Result;

            ScenarioContext.Current.Set<HttpResponseMessage>(response, "response");
            //ScenarioContext.Current.Set<bool>(false, "IsGetAllItem");
        }

        [Then(@"Http status (.*) should be returned")]
        public void ThenHttpStatusShouldBeReturned(int httpStatusCode)
        {
            //bool IsGetAllItem = ScenarioContext.Current.Get<bool>("IsGetAllItem");
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("response");

            switch (httpStatusCode)
            {
                case 200:
                    Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
                    break;

                case 201:
                    Assert.AreEqual(System.Net.HttpStatusCode.Created, response.StatusCode);
                    break;

                case 204:
                    Assert.AreEqual(System.Net.HttpStatusCode.NoContent, response.StatusCode);
                    break;


                case 400:
                    Assert.AreEqual(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
                    break;

                case 404:
                    Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
                    break;

                default:
                    Assert.Fail();
                    break;
            }
        }

        [Then(@"Response context contains ""(.*)""")]
        async public void ThenResponseContextContains(string description)
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("response");
            //var jsonString = await response.Content.ReadAsStringAsync();
            TodoItem item = JsonConvert.DeserializeObject<TodoItem>(await response.Content.ReadAsStringAsync());

            //TodoItem item = null;
            //JsonTodoIems item = null;
            //var task = response.Content.ReadAsStringAsync().ContinueWith((Response) =>
            //{
            //    var jsonString = Response.Result;
            //    item = JsonConvert.DeserializeObject<TodoItem>(jsonString);
            //});
            //task.Wait();

            Assert.IsTrue(item.Description.Contains(description));
        }

        [Given(@"existing TodoItems")]
        public void GivenExistingTodoItems()
        {
        }

        [Given(@"a existing TodoItem with Id (.*)")]
        public void GivenAExistingTodoItemWithId(int id)
        {
            ScenarioContext.Current.Set(id, "Id");
        }

        [Given(@"a non-existing TodoItem with Id (.*)")]
        public void GivenANon_ExistingTodoItemWithId(int id)
        {
            ScenarioContext.Current.Set(id, "Id");
        }

        [When(@"all items are retrieved")]
        public void WhenAllItemsAreRetrieved()
        {
            var response = client.GetAsync($"{localUrl}/TodoItems").Result;

            ScenarioContext.Current.Set<HttpResponseMessage>(response, "response");
            ScenarioContext.Current.Set<bool>(false, "IsGetAllItem");
        }

        [Then(@"all items are returned")]
        public void ThenAllItemsAreReturned()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("response");
            var items = response.Content.ReadAsAsync(typeof(IEnumerable<TodoItem>)).Result as IEnumerable<TodoItem>;

            int expectedCount = 4;
            Assert.AreEqual(expectedCount, items.Count());
        }

        [Given(@"a new TodoItem with description ""(.*)""")]
        public void GivenANewTodoItemWithDescription(string desc)
        {
            //TodoItem item = new TodoItem { Description = desc };
            List<TodoItem> items = new List<TodoItem>() { new TodoItem { Description = desc } };
            ScenarioContext.Current.Set(items, "items");
        }

        [When(@"a Post request is made")]
        public void WhenAPostRequestIsMade()
        {
            var item = ScenarioContext.Current.Get<List<TodoItem>>("items");

            var response = client
                .PostAsync($"{localUrl}/TodoItems", new StringContent(JsonConvert.SerializeObject(item).ToString(), Encoding.UTF8, "application/json"))
                .Result;
            ScenarioContext.Current.Set<HttpResponseMessage>(response, "response");
        }

        [Then(@"The response location header will be set to the resource location")]
        async public void ThenTheResponseLocationHeaderWillBeSetToTheResourceLocation()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("response");
            var item = JsonConvert.DeserializeObject<TodoItem>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual($"{localUrl}/TodoItems/{item.Id}", response.Headers.Location.ToString());
        }

        [When(@"a Delete request is made")]
        public void WhenADeleteRequestIsMade()
        {
            int Id = ScenarioContext.Current.Get<int>("Id");
            HttpResponseMessage response = client.DeleteAsync($"{localUrl}/TodoItems/{Id}").Result;

            ScenarioContext.Current.Set<HttpResponseMessage>(response, "response");
        }

        [Then(@"the TodoItem should be removed")]
        public void ThenTheTodoItemShouldBeRemoved()
        {
            int Id = ScenarioContext.Current.Get<int>("Id");

            HttpResponseMessage response = client.GetAsync($"{localUrl}/TodoItems/{Id}").Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [When(@"a Put request is made")]
        public void WhenAPutRequestIsMade()
        {
            int Id = ScenarioContext.Current.Get<int>("Id");
            HttpResponseMessage response = client
                .PutAsync<TodoItem>(
                $"{localUrl}/TodoItems/{Id}",
                new TodoItem { Id = Id, Description = "Put", Completed = false },
                new JsonMediaTypeFormatter()
                ).Result;

            ScenarioContext.Current.Set<HttpResponseMessage>(response, "response");
        }

    }
}