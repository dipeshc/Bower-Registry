namespace BowerRegistry
{
    public interface IPackageRepository
    {
        bool Readonly { get; }
        Package[] List();
        Package Get(string name);
        Package[] Search(string name);
        void Add(Package package);
    }
}