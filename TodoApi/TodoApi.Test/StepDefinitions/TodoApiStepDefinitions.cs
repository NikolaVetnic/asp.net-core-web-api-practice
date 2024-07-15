using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UseVerbatimString

namespace TodoApi.Test.StepDefinitions;

[Binding]
public class TodoApiStepDefinitions
{
    private readonly Random _random = new();

    private readonly HttpClient _client;
    private HttpResponseMessage _response;

    private TodoItem _persistedTodoItem;
    private TodoItem _todoItem;

    public TodoApiStepDefinitions(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    #region Given
    [Given("the todo list is not empty")]
    public async Task GivenTheTodoListIsNotEmpty()
    {
        await CreateTodoItem();
    }

    [Given(@"a todo item t(\d)")]
    public async Task GivenATodoItemT(long todoItemId)
    {
        await CreateTodoItem();
    }

    [Given("a TodoItem with the following properties")]
    public void GivenIHaveATodoItemWithTheFollowingProperties(Table table)
    {
        var (name, isCompleted) = ExtractTodoItemValues(table.Rows[0]);
        _todoItem = new TodoItem { Name = name, IsCompleted = isCompleted };
    }
    #endregion

    #region When
    [When("a GET request is sent to the /todo endpoint")]
    public async Task WhenAGETRequestIsSentToTheTodoEndpoint()
    {
        _response = await _client.GetAsync("/api/todo");
    }

    [When(@"a GET request is sent to the /todo/\{id} endpoint with t(\d)\.Id")]
    public async Task WhenAGETRequestIsSentToTheTodoIdEndpointWithT_Id(long todoItemId)
    {
        _response = await _client.GetAsync($"/api/todo/{_persistedTodoItem.Id}");
    }

    [When(@"a GET request is sent to the /todo/\{id} endpoint with a nonexistent id")]
    public async Task WhenAGETRequestIsSentToTheTodoIdEndpointWithANonexistentId()
    {
        _response = await _client.GetAsync($"/api/todo/{RandomId}");
    }

    [When("a POST request is sent to the Todo endpoint")]
    public async Task WhenIPOSTTheTodoItem()
    {
        _response = await _client.PostAsJsonAsync("/api/todo", _todoItem);
    }

    [When(@"a PUT request is sent to the /todo/\{id} endpoint with t(\d)\.Id and the following data")]
    public async Task WhenAPUTRequestIsSentToTheTodoIdEndpointWithT_IdAndTheFollowingData(long todoItemId, Table table)
    {
        var (name, isCompleted) = ExtractTodoItemValues(table.Rows[0]);
        var updatedTodo = new TodoItem { Id = _persistedTodoItem.Id, Name = name, IsCompleted = isCompleted };

        _response = await _client.PutAsJsonAsync($"/api/todo/{_persistedTodoItem.Id}", updatedTodo);
    }

    [When(@"a DELETE request is sent to the /todo/\{id} endpoint with t(\d)\.Id")]
    public async Task WhenADELETERequestIsSentToTheTodoIdEndpointWithT_Id(long todoItemId)
    {
        _response = await _client.DeleteAsync($"/api/todo/{_persistedTodoItem.Id}");
    }

    [When(@"a DELETE request is sent to the /todo/\{id} endpoint with a nonexistent id")]
    public async Task WhenADELETERequestIsSentToTheTodoIdEndpointWithANonexistentId()
    {
        _response = await _client.DeleteAsync($"/api/todo/{RandomId}");
    }
    #endregion

    #region Then
    [Then("the response status code is (\\d{3}) \\((.+)\\)")]
    public void ThenTheResponseStatusCodeIs(int expectedStatusCode, string description)
    {
        ((int)_response.StatusCode).Should().Be(expectedStatusCode);
    }

    [Then("the response contains a list of todo items")]
    public async Task ThenTheResponseContainsAListOfTodoItems()
    {
        var todos = await ExtractTodoItemsFromResponse();

        todos.Should().NotBeNullOrEmpty();
    }

    [Then(@"the response contains todo item t(\d)")]
    public async Task ThenTheResponseContainsTodoItemT(long todoItemId)
    {
        var todo = await ExtractTodoItemFromResponse();

        todo.Should().NotBeNull();
        todo!.Name.Should().Be(_persistedTodoItem.Name);
        todo.IsCompleted.Should().Be(_persistedTodoItem.IsCompleted);
    }

    [Then(@"the response body is empty")]
    public async Task ThenTheResponseBodyIsEmpty()
    {
        var responseBody = await _response.Content.ReadAsStringAsync();
        responseBody.Should().BeNullOrEmpty();
    }

    [Then(@"the response body is a TodoItem with the following properties")]
    public async Task ThenTheResponseBodyIsATodoItemWithTheFollowingProperties(Table table)
    {
        var (name, isCompleted) = ExtractTodoItemValues(table.Rows[0]);
        var todo = await ExtractTodoItemFromResponse();

        todo.Should().NotBeNull();
        todo.Name.Should().Be(name);
        todo.IsCompleted.Should().Be(isCompleted);
    }

    [Then(@"the response contains the updated todo item t(\d)\ with the following properties")]
    public async Task ThenTheResponseContainsTheUpdatedTodoItemWithTheFollowingProperties(long todoItemId, Table table)
    {
        var todo = await ExtractTodoItemFromResponse();
        var (name, isCompleted) = ExtractTodoItemValues(table.Rows[0]);

        todo.Should().NotBeNull();
        todo!.Id.Should().Be(_persistedTodoItem.Id);
        todo.Name.Should().Be(name);
        todo.IsCompleted.Should().Be(isCompleted);
    }
    #endregion

    #region Utility
    private async Task CreateTodoItem()
    {
        await CreateTodoItem("Test Todo", false);
    }

    private async Task CreateTodoItem(string name, bool isCompleted)
    {
        var newTodo = new TodoItem { Name = name, IsCompleted = isCompleted };
        var postResponse = await _client.PostAsJsonAsync("/api/todo", newTodo);
        postResponse.EnsureSuccessStatusCode();

        _persistedTodoItem = await postResponse.Content.ReadFromJsonAsync<TodoItem>();
    }

    private async Task<TodoItem> ExtractTodoItemFromResponse()
    {
        var responseBody = await _response.Content.ReadAsStringAsync();
        var todo = JsonSerializer.Deserialize<TodoItem>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return todo;
    }

    private async Task<List<TodoItem>> ExtractTodoItemsFromResponse()
    {
        var responseBody = await _response.Content.ReadAsStreamAsync();
        var todos = await JsonSerializer.DeserializeAsync<List<TodoItem>>(responseBody);

        return todos;
    }

    private static (string name, bool todoItemIsCompleted) ExtractTodoItemValues(TableRow row)
    {
        return (
            row.TryGetValue("Name", out var name) ? name : string.Empty,
            Convert.ToBoolean(row.TryGetValue("IsCompleted", out var isCompletedString) ? isCompletedString : "false")
        );
    }

    private long RandomId => _random.NextInt64(1, int.MaxValue);
    #endregion
}