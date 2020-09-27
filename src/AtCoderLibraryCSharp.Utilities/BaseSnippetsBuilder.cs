using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AtCoderLibraryCSharp.Utilities
{
    public abstract class BaseSnippetsBuilder : ISnippetsBuilder
    {
        protected string[] FilePaths { get; }
        protected abstract string Extension { get; }
        private readonly string _outputPath;
        private readonly string _outputName;

        protected BaseSnippetsBuilder(string outputName, string targetDirectory, string outputDirectory = null)
        {
            _outputName = outputName;
            targetDirectory = targetDirectory.Replace(@"\", @"/");
            if (targetDirectory[^1] != '/') targetDirectory += "/";
            if (!Directory.Exists(targetDirectory))
                throw new DirectoryNotFoundException(nameof(targetDirectory));
            FilePaths = Directory.GetFiles(targetDirectory, "*.cs");

            _outputPath = outputDirectory ?? @"./outputs/";
            if (_outputPath[^1] != '/') _outputPath += "/";
            if (!Directory.Exists(_outputPath)) Directory.CreateDirectory(_outputPath);
        }

        public async ValueTask BuildAsync(CancellationToken cancellationToken = default)
        {
            var outputPath = Path.Combine(_outputPath, _outputName + Extension);
            await using var fileStream = File.Exists(outputPath)
                ? new FileStream(outputPath, FileMode.Truncate)
                : File.Create(outputPath);
            await BuildSnippetsAsync(fileStream, cancellationToken);
            await fileStream.FlushAsync(cancellationToken);
        }

        protected abstract ValueTask BuildSnippetsAsync(FileStream fileStream, CancellationToken cancellationToken);
    }
}