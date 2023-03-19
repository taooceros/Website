using System.Collections.Concurrent;
using System.Net.Http.Json;
using Website.Models;

public interface IDiarieservice
{
    Task<List<DiaryLink>> GetDiaryLinksAsync();

    Task<IEnumerable<DiaryPost>> LoadDiaryAsync(IEnumerable<DiaryLink> links);
}

public class Diarieservice : IDiarieservice
{
    private readonly HttpClient _httpClient;

    public Diarieservice(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DiaryLink>> GetDiaryLinksAsync()
    {
        var links = await _httpClient.GetFromJsonAsync<List<DiaryLink>>("Diaries/outline.json");
        return links ?? new List<DiaryLink>();
    }

    public async Task<IEnumerable<DiaryPost>> LoadDiaryAsync(IEnumerable<DiaryLink> links)
    {
        var diaryPosts = new ConcurrentBag<DiaryPost>();

        await Parallel.ForEachAsync(links, async (link, token) =>
        {
            var content = await _httpClient.GetStringAsync(link.FilePath);
            diaryPosts.Add(new(link.FileName, link.FilePath, content));
        });

        return diaryPosts;
    }
}