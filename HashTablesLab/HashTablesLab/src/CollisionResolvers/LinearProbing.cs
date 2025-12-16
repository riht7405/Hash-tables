using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.CollisionResolvers
{
    public class LinearProbing : ICollisionResolver
    {
        public string Name => "Линейное исследование (h(k,i) = (h'(k) + i) mod m)";

        public int Resolve(int hash, int attempt, int tableSize)
        {
            return (hash + attempt) % tableSize;
        }
    }
}