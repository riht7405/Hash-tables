using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.CollisionResolvers
{
    public class DoubleHashing : ICollisionResolver
    {
        public string Name => "Двойное хеширование";

        public int Resolve(int hash, int attempt, int tableSize)
        {
            int hash2 = 1 + (hash % (tableSize - 1));
            return (hash + attempt * hash2) % tableSize;
        }
    }
}