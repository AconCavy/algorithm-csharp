namespace Algorithm
{
    public interface IOracle<TMonoid>
    {
        TMonoid IdentityElement { get; }
        TMonoid Operate(TMonoid a, TMonoid b);
    }

    public interface IOracle<TMonoid, TMap> : IOracle<TMonoid>
    {
        TMap IdentityMapping { get; }
        TMonoid Map(TMap f, TMonoid x);
        TMap Compose(TMap f, TMap g);
    }
}