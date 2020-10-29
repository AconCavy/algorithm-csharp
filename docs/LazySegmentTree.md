# LazySegmentTree

The `LazySegmentTree` will be operated via an `IOracle<TMonoid, TMap>` interface.  
The `IOracle<TMonoid, TMap>` interface should be implemented that meets the following requirements.

- The struct `TMonoid` of the monoid should be a `readonly struct`
- The monoid identity property `TMonoid MonoidIdentity`
- The binary operation `TMonoid Operate(in TMonoid a, in TMonoid b)`
- The struct `TMap` of the map should be a `readonly struct`
- The map identity property `TMonoid MonoidIdentity`
- The function `TMonoid Map(in TMap f, in TMonoid x)` that returns `f(x)`
- The function `TMap Compose(in TMap f, in TMap g)` that returns `f o g`

The `LazySegmentTree` also will be work on that `TMonoid` and `TMap` are not the `readonly struct`.  
However, performance suffers from defensive copies.  
Reference: "[Write safe and efficient C# code](https://docs.microsoft.com/en-us/dotnet/csharp/write-safe-efficient-code)".

For example, [AtCoderLibrary Practice Contest Tasks-L](https://atcoder.jp/contests/practice2/tasks/practice2_l), the usage is:

```c#
public readonly struct S
{
    public readonly long Zero;
    public readonly long One;
    public readonly long Inversion;
    public S(long zero, long one, long inversion) => (Zero, One, Inversion) = (zero, one, inversion);
}

public readonly struct F
{
    public readonly bool Flag;
    public F(bool flag) => Flag = flag;
}

public class Oracle : IOracle<S, F>
{
    public S MonoidIdentity { get; } = new S(0, 0, 0);
    public S Operate(in S a, in S b)
        => new S(a.Zero + b.Zero, a.One + b.One, a.Inversion + b.Inversion + a.One * b.Zero);

    public F MapIdentity { get; } = new F(false);
    public S Map(in F f, in S x) => f.Flag ? new S(x.One, x.Zero, x.Zero * x.One - x.Inversion) : x;
    public F Compose(in F f, in F g) => new F(f.Flag && !g.Flag || !f.Flag && g.Flag);
}

public void Main()
{
    var lst = new LazySegmentTree<S, F>(10, new Oracle()); // initialized by length
    // var items = Enumerable.Repeat(new S(1, 0, 0), 10);
    // var lst = new LazySegmentTree<S, F>(items, new Oracle()); // initialized by IEnumerable<TMonoid>
    // query
}
```