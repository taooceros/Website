using System.Net.Http.Json;
using Website.Models;

public interface IBlogService
{
    Task<IEnumerable<BlogPost>> GetBlogPostsAsync();
}

public class BlogService : IBlogService
{
    private readonly HttpClient _httpClient;

    public BlogService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<BlogPost>> GetBlogPostsAsync()
    {
        var links = await _httpClient.GetFromJsonAsync<List<BlogLink>>("api/blogposts");

        if (links == null)
        {
            return new List<BlogPost>();
        }

        var blogPosts = new List<BlogPost>();
        await Parallel.ForEachAsync(links, async (link, token) =>
        {
            var content = await _httpClient.GetStringAsync(link.FilePath);
            blogPosts.Add(new()
            {
                Title = link.Title,
                Description = link.Description,
                Content = content
            });
            return;
        });

        return blogPosts;
    }
}