namespace Algorithm
{
    public interface IOracle<TMonoid>
    {
        TMonoid IdentityElement { get; }
        TMonoid Operate(TMonoid a, TMonoid b);
    }

    public interface IOracle<TMonoid, TMapping> : IOracle<TMonoid>
    {
        TMapping IdentityMapping { get; }
        TMonoid Map(TMapping f, TMonoid x);
        TMapping Compose(TMapping f, TMapping g);
    }
}