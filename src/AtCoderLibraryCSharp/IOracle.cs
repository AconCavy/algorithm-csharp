namespace AtCoderLibraryCSharp
{
    public interface IOracle<TMonoid> where TMonoid : struct
    {
        TMonoid MonoidIdentity { get; }
        TMonoid Operation(in TMonoid a, in TMonoid b);
    }

    public interface IOracle<TMonoid, TMap> : IOracle<TMonoid> where TMap : struct where TMonoid : struct
    {
        TMap MapIdentity { get; }
        TMonoid Mapping(in TMap f, in TMonoid x);
        TMap Composition(in TMap f, in TMap g);
    }
}