namespace HashTablesLab.Core.Interfaces
{
    public interface ICollisionResolver
    {
        int Resolve(int hash, int attempt, int tableSize);
        string Name { get; }
    }
}