using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace AtCoderLibraryCSharp.Utilities
{
    public class VisualStudioCodeSnippetsBuilder : BaseSnippetsBuilder
    {
        protected override string Extension { get; } = ".code-snippets";
        private readonly Dictionary<string, Snippet> _dictionary;

        private class Snippet
        {
            [JsonPropertyName("scope")] public string Scope { get; set; } = "csharp";
            [JsonPropertyName("prefix")] public string Prefix { get; set; }
            [JsonPropertyName("body")] public string[] Body { get; set; }
        }

        public VisualStudioCodeSnippetsBuilder(string outputName, string targetDirectory, string outputDirectory = null)
            : base(outputName, targetDirectory, outputDirectory)
        {
            _dictionary = new Dictionary<string, Snippet>();
        }

        protected override async ValueTask BuildSnippetsAsync(FileStream fileStream,
            CancellationToken cancellationToken)
        {
            foreach (var filePath in FilePaths)
            {
                var (section, snippet) = await CreateSnippetAsync(filePath, cancellationToken);
                _dictionary[section] = snippet;
                if (cancellationToken.IsCancellationRequested) break;
            }

            var options = new JsonSerializerOptions {WriteIndented = true};
            await JsonSerializer.SerializeAsync(fileStream, _dictionary, options, cancellationToken);
        }

        private static async ValueTask<(string, Snippet)> CreateSnippetAsync(string filePath,
            CancellationToken cancellationToken)
        {
            await using var input = new FileStream(filePath, FileMode.Open);
            using var streamReader = new StreamReader(input);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var skip = true;
            var body = new List<string>();
            string line;
            while ((line = await streamReader.ReadLineAsync()) != null && !cancellationToken.IsCancellationRequested)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) continue;
                if (skip && line.Contains("using")) continue;
                skip = false;
                body.Add(line);
            }

            return (fileName, new Snippet {Prefix = fileName.ToLower(), Body = body.ToArray()});
        }
    }
}