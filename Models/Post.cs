namespace Website.Models;

public record Post(string FileName, string FilePath, string Content)
{
    public bool DisplayFull { get; set; }
}