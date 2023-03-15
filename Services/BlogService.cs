using System.Collections.Concurrent;
using System.Net.Http.Json;
using Website.Models;

namespace Website.Services;

public interface IBlogService
{
    Task<IEnumerable<BlogLink>> GetBlogPostsAsync();
}

public class BlogService : IBlogService
{
    private readonly HttpClient _httpClient;

    public BlogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BlogLink>> GetBlogPostsAsync()
    {
        var links = await _httpClient.GetFromJsonAsync<List<BlogLink>>("Blogs/outline.json");

        return links ?? new List<BlogLink>();

    }
}