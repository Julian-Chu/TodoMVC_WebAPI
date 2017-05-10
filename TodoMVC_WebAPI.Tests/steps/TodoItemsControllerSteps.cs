using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using TechTalk.SpecFlow;
using TodoMVC_WebAPI.Models;

namespace TodoMVC_WebAPI.Tests.steps
{
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

            var response = client.GetAsync($"{localUrl}/TodoItems/{id}").Result;

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
                case 400:
                    Assert.AreEqual(System.Net.HttpStatusCode.NotFound, response.StatusCode);
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
        public void ThenResponseContextContains(string description)
        {
            TodoItem item;
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("response");
            item = response.Content.ReadAsAsync<TodoItem>().Result;

            Assert.IsTrue(item.Description.Contains(description));
        }

        [Given(@"existing issues")]
        public void GivenExistingIssues()
        {
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
            TodoItem item = new TodoItem { Description = desc };
            ScenarioContext.Current.Set(item, "item");
        }

        [When(@"a Post request is made")]
        public void WhenAPostRequestIsMade()
        {
            var item = ScenarioContext.Current.Get<TodoItem>("item");

            var response = client
                .PostAsync($"{localUrl}/TodoItems", new StringContent(JsonConvert.SerializeObject(item).ToString(),Encoding.UTF8,"application/json"))
                .Result;
            ScenarioContext.Current.Set<HttpResponseMessage>(response, "response");
        }

        [Then(@"The response location header will be set to the resource location")]
        public void ThenTheResponseLocationHeaderWillBeSetToTheResourceLocation()
        {
            var response = ScenarioContext.Current.Get<HttpResponseMessage>("response");
            var item =  JsonConvert.DeserializeObject<TodoItem>(response.Content.ReadAsStringAsync().Result);

            Assert.AreEqual($"{localUrl}/TodoItems/{item.Id}", response.Headers.Location.ToString());

        }


    }
}