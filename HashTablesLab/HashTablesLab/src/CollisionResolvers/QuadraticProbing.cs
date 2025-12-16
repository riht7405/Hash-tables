using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.CollisionResolvers
{
    public class QuadraticProbing : ICollisionResolver
    {
        private const int C1 = 1;
        private const int C2 = 3;

        public string Name => $" вадратичное исследование (c1={C1}, c2={C2})";

        public int Resolve(int hash, int attempt, int tableSize)
        {
            return (hash + C1 * attempt + C2 * attempt * attempt) % tableSize;
        }
    }
}