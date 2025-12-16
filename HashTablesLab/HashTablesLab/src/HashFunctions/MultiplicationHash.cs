using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.HashFunctions
{
    public class MultiplicationHash : IHashFunction<int>
    {
        // Золотое сечение (√5-1)/2 ≈ 0.618033988749895
        private static readonly double A = 0.618033988749895;

        public string Name => "Метод умножения (⌊m*(key*A mod 1)⌋)";

        public int Calculate(int key, int tableSize)
        {
            // Используем дробную часть от умножения ключа на A
            double product = System.Math.Abs(key) * A;
            double fractional = product - System.Math.Floor(product);
            return (int)(tableSize * fractional);
        }
    }
}