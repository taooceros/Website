namespace Website.Models;

public record DiaryPost(string FileName, string FilePath, string Content)
{
    public bool DisplayFull { get; set; }
    public string DecryptedContent { get; set; } = "";
}