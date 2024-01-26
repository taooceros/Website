using MudBlazor;

namespace Website;

public static class ThemeSpecification
{
    public static async Task DetectSystemTheme()
    {
        DarkMode = await (Provider?.GetSystemPreference() ?? Task.FromResult(false));
    }

    public static bool DarkMode { get; set; }

    public static MudThemeProvider? Provider { get; set; }
}