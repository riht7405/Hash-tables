using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.CollisionResolvers
{
    public class CustomResolver1 : ICollisionResolver
    {
        public string Name => "Собственный метод 1 (Псевдослучайные числа)";

        public int Resolve(int hash, int attempt, int tableSize)
        {
            // Генерация псевдослучайной последовательности на основе хеша
            uint seed = (uint)hash + (uint)attempt * 0x9E3779B9u;
            seed = (seed ^ (seed >> 16)) * 0x85EBCA6Bu;
            seed = (seed ^ (seed >> 13)) * 0xC2B2AE35u;
            seed = seed ^ (seed >> 16);

            return (hash + (int)(seed % (uint)tableSize)) % tableSize;
        }
    }
}