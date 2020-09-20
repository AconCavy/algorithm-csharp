using System.Threading;
using System.Threading.Tasks;

namespace AtCoderLibraryCSharp.Utilities
{
    public interface ISnippetsBuilder
    {
        ValueTask BuildAsync(CancellationToken cancellationToken);
    }
}