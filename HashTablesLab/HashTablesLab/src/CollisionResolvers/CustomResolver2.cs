using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.CollisionResolvers
{
    public class CustomResolver2 : ICollisionResolver
    {
        public string Name => "Собственный метод 2 (Фиббоначчи хеширование)";

        public int Resolve(int hash, int attempt, int tableSize)
        {
            // Использование золотого сечения для равномерного распределения
            const double phi = 1.618033988749895;
            double offsetDouble = tableSize * (attempt * phi - System.Math.Floor(attempt * phi));
            int offset = (int)offsetDouble;

            return (hash + offset) % tableSize;
        }
    }
}