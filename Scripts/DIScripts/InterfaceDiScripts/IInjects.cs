public interface IInjects<TDeps> where TDeps : IDependencyGroups
{
    void Inject(TDeps dependencies);
}