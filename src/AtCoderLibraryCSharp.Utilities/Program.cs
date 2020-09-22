using System.Threading.Tasks;

namespace AtCoderLibraryCSharp.Utilities
{
    public static class Program
    {
        private const string Name = "AtCoderLibraryCSharp";
        private const string TargetPath = @"./src/AtCoderLibraryCSharp/";

        public static async Task Main()
        {
            await new VisualStudioCodeSnippetsBuilder(Name, TargetPath).BuildAsync();
        }
    }
}