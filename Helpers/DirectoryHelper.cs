namespace DiscordTokenStealer.Helpers;
public static class DirectoryHelper
{
    public static bool TryFindSubDirectory(string directory, string toFind, out string? result)
    {
        foreach (string subDir in Directory.GetDirectories(directory, "*", SearchOption.AllDirectories))
        {
            string combined = Path.Combine(subDir, toFind);
            if (Directory.Exists(combined))
            {
                result = combined;
                return true;
            }
        }
        result = null;
        return false;
    }
}