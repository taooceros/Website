using Website.Models;

public interface IDiaryService
{
    Task<IEnumerable<DiaryLink>> GetDiaryLinksAsync();
}

