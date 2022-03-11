namespace Algorithm
{
    public interface IOracle<TMonoid>
    {
        TMonoid MonoidIdentity { get; }
        TMonoid Operate(TMonoid a, TMonoid b);
    }

    public interface IOracle<TMonoid, TMap> : IOracle<TMonoid>
    {
        TMap MapIdentity { get; }
        TMonoid Map(TMap f, TMonoid x);
        TMap Compose(TMap f, TMap g);
    }
}