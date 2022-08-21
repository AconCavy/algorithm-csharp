# LazySegmentTree

The `LazySegmentTree` will be operated via an `IOracle<TMonoid, TMapping>` interface.  
The `IOracle<TMonoid, TMapping>` interface should be implemented that meets the following requirements.

- The identity element property `TMonoid IdentityElement`
- The binary operation `TMonoid Operate(TMonoid a, TMonoid b)`
- The identity mapping property `TMapping IdentityMapping`
- The function `TMonoid Map(TMapping f, TMonoid x)` that returns `f(x)`
- The function `TMapping Compose(TMapping f, TMapping g)` that returns `f o g`

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
    public S IdentityElement { get; } = new S(0, 0, 0);
    public S Operate(S a, S b)
        => new S(a.Zero + b.Zero, a.One + b.One, a.Inversion + b.Inversion + a.One * b.Zero);

    public F IdentityMapping { get; } = new F(false);
    public S Map(F f, S x) => f.Flag ? new S(x.One, x.Zero, x.Zero * x.One - x.Inversion) : x;
    public F Compose(F f, F g) => new F(f.Flag && !g.Flag || !f.Flag && g.Flag);
}

public void Main()
{
    var lst = new LazySegmentTree<S, F>(10, new Oracle()); // initialized by length
    // var items = Enumerable.Repeat(new S(1, 0, 0), 10);
    // var lst = new LazySegmentTree<S, F>(items, new Oracle()); // initialized by IEnumerable<TMonoid>
    // query
}
```