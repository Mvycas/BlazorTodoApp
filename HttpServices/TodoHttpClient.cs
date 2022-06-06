using System.Text;
using System.Text.Json;
using Domain.Contracts;
using Domain.Models;

namespace HttpServices;

public class TodoHttpClient : ITodoHome
{
    public async Task<ICollection<Todo>> GetAsync()
    {
        using HttpClient client = new ();
        HttpResponseMessage response = await client.GetAsync("https://localhost:7296/Todos");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }

        ICollection<Todo> todos = JsonSerializer.Deserialize<ICollection<Todo>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return todos;
    }

    public async Task<Todo> GetById(int id)
    {
        using HttpClient httpClient = new();
        HttpResponseMessage responseMessage = await httpClient.GetAsync($"https://localhost:7228/Todos/{id}");
        string content = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {responseMessage.StatusCode}, {content}");
        }

        Todo todo = JsonSerializer.Deserialize<Todo>(content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        })!;
        return todo;
    }

    public async Task<Todo> AddAsync(Todo todo)
    {
        using HttpClient client = new();

        string todoAsJson = JsonSerializer.Serialize(todo);

        StringContent content = new(todoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PostAsync("https://localhost:7296/todos", content);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }
        
        Todo returned = JsonSerializer.Deserialize<Todo>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        
        return returned;
    }

    public async Task DeleteAsync(int id)
    {
        using HttpClient client = new();
        HttpResponseMessage response = await client.DeleteAsync($"https://localhost:7296/todos/{id}");
        string content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {content}");
        }
    }

    public async Task UpdateAsync(Todo todo)
    {
        using HttpClient client = new();

        string todoAsJson = JsonSerializer.Serialize(todo);

        StringContent content = new(todoAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await client.PatchAsync("https://localhost:7296/todos", content);
        string responseContent = await response.Content.ReadAsStringAsync();
        
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error: {response.StatusCode}, {responseContent}");
        }
    }
}