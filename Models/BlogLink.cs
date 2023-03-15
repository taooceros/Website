namespace Website.Models;


public record BlogLink(string FileName, string Title, string Description)
{
    public string FilePath => $"Blogs/Post/{FileName}";
}