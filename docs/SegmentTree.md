# SegmentTree

The `SegmentTree` will be operated via an `IOracle<TMonoid>` interface.  
The `IOracle<TMonoid>` interface should be implemented that meets the following requirements.

- The struct `TMonoid` of the monoid should be a `readonly struct`
- The monoid identity property `TMonoid MonoidIdentity`
- The binary operation `TMonoid Operate(in TMonoid a, in TMonoid b)`

The `SegmentTree` also will be work on that `TMonoid` is not the `readonly struct`.  
However, performance suffers from defensive copies.  
Reference: "[Write safe and efficient C# code](https://docs.microsoft.com/en-us/dotnet/csharp/write-safe-efficient-code)".

For example, [AtCoderLibrary Practice Contest Tasks-J](https://atcoder.jp/contests/practice2/tasks/practice2_j), the usage is:

```c#
public readonly struct S
{
    public readonly int Value;
    public S(int value) => Value = value;
}

public class Oracle : IOracle<S>
{
    public S MonoidIdentity { get; } = new S(-1);
    public S Operate(in S a, in S b) => new S(System.Math.Max(a.Value, b.Value));
}

public void Main()
{
    var st = new SegmentTree<S>(10, new Oracle());
    // var items = Enumerable.Range(0, 10).Select(x => new S(x));
    // var st = new SegmentTree<S>(items, new Oracle()); // initialized by IEnumerable<TMonoid>
    // query
}
```