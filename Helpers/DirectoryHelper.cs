namespace DiscordTokenStealer.Helpers;

public static class DirectoryHelper
{
    public static bool TryFindSubDirectory(string directory, string toFind, out string? result)
    {
        result = Directory.EnumerateDirectories(directory, "*", SearchOption.AllDirectories)
            .FirstOrDefault(subDirectory => Directory.Exists(Path.Combine(subDirectory, toFind)));
        return Directory.Exists(result);
    }
}