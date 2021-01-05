namespace AlgorithmSharp
{
    public interface IOracle<TMonoid> where TMonoid : struct
    {
        TMonoid MonoidIdentity { get; }
        TMonoid Operate(in TMonoid a, in TMonoid b);
    }

    public interface IOracle<TMonoid, TMap> : IOracle<TMonoid> where TMap : struct where TMonoid : struct
    {
        TMap MapIdentity { get; }
        TMonoid Map(in TMap f, in TMonoid x);
        TMap Compose(in TMap f, in TMap g);
    }
}