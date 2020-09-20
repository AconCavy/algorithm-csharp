using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AtCoderLibraryCSharp.Utilities
{
    public class VisualStudioCodeSnippetsBuilder : BaseSnippetsBuilder
    {
        protected override string Extension { get; } = ".code-snippets";

        private const string SectionTemplate = @"
""<name>"": {
    ""scope"": ""csharp"",
    ""prefix"": ""<name>"",
    ""body"": [
        <body>
    ]
},";

        public VisualStudioCodeSnippetsBuilder(string outputName, string targetDirectory, string outputDirectory = null)
            : base(outputName, targetDirectory, outputDirectory)
        {
        }

        protected override async ValueTask BuildBodyAsync(StreamWriter streamWriter,
            CancellationToken cancellationToken)
        {
            await streamWriter.WriteLineAsync("{");
            foreach (var filePath in FilePaths)
            {
                var section = await BuildSectionAsync(filePath, cancellationToken);
                await streamWriter.WriteLineAsync(section);

                if (cancellationToken.IsCancellationRequested) break;
            }

            await streamWriter.WriteLineAsync("}");
        }

        private static async ValueTask<string> BuildSectionAsync(string filePath, CancellationToken cancellationToken)
        {
            await using var input = new FileStream(filePath, FileMode.Open);
            using var streamReader = new StreamReader(input);
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var section = SectionTemplate;
            section = section.Replace("<name>", fileName.ToLower());
            var body = new StringBuilder();
            string line;
            var skip = true;
            while ((line = await streamReader.ReadLineAsync()) != null && !cancellationToken.IsCancellationRequested)
            {
                if (string.IsNullOrEmpty(line) || string.IsNullOrWhiteSpace(line)) continue;
                if (skip && line.Contains("using")) continue;
                skip = false;

                line = "\"" + line + "\",\n";
                body.Append(line);
            }

            section = section.Replace("<body>", body.ToString());
            return section;
        }
    }
}