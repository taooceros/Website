using MudBlazor;

namespace Website;

public static class ThemeSpecification
{
    public static async Task DetectSystemTheme()
    {
        DarkMode = await (Provider?.GetSystemPreference() ?? Task.FromResult(false));
    }

    public static bool DarkMode { get; set; }

    public static MudThemeProvider? Provider { get; set; } = default!;

    public static readonly MudTheme Theme = new()
    {
        Palette = new Palette()
        {
            Primary = "#006b5d",
            Secondary = "#4a635d",
            Tertiary = "#446278",
            Error = "#ba1a1a",
            AppbarText = "#ffffff",
            PrimaryContrastText = "#ffffff",
            SecondaryContrastText = "#ffffff",
            TertiaryContrastText = "#ffffff",
            TextPrimary = "#191c1b"
        },
        PaletteDark = new Palette()
        {
            Primary = "#57dbc4",
            Secondary = "#b1ccc5",
            Tertiary = "#abcae5",
            Error = "#ffb4ab",
            Surface = "#005046",
            Background = "#191c1b",
            TextPrimary = "#e0e3e1",
            AppbarText = "#00372f",
            PrimaryContrastText = "#00372f",
            SecondaryContrastText = "#1c3530",
        },
    };
}