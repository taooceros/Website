using System.Collections.Concurrent;
using System.Net.Http.Json;
using Website.Models;

public interface IDiaryService
{
    Task<List<DiaryLink>> GetDiaryLinksAsync();

    Task<IEnumerable<DiaryPost>> LoadDiaryAsync(IEnumerable<DiaryLink> links);
}

public class DiaryService : IDiaryService
{
    private readonly HttpClient _httpClient;

    public DiaryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<DiaryLink>> GetDiaryLinksAsync()
    {
        var links = await _httpClient.GetFromJsonAsync<List<DiaryLink>>("Diarys/outline.json");
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