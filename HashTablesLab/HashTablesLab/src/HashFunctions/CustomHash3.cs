using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.HashFunctions
{
    public class CustomHash3 : IHashFunction<int>
    {
        public string Name => "Собственный метод 3 (Многочленный)";

        public int Calculate(int key, int tableSize)
        {
            long hash = 0;
            long x = System.Math.Abs((long)key);

            // Полиномиальный хеш с различными коэффициентами
            hash = (x * 31 + 17) % tableSize;
            hash = (hash * 37 + 19) % tableSize;

            return (int)hash;
        }
    }
}