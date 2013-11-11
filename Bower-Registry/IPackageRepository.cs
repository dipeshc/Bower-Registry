namespace BowerRegistry
{
    public interface IPackageRepository
    {
        bool IsReadonly { get; }
        Package[] List();
        Package Get(string name);
        Package[] Search(string name);
        void Add(Package package);
    }
}