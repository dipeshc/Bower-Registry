namespace BowerRegistry
{
    public interface IPackageRepository
    {
        Package[] List();
        Package Get(string name);
        Package[] Search(string name);
    }
}