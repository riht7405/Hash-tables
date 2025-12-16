using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.HashFunctions
{
    public class CustomHash4 : IHashFunction<int>
    {
        public string Name => "Собственный метод 4 (MD5-like упрощенный)";

        public int Calculate(int key, int tableSize)
        {
            // Упрощенный алгоритм, вдохновленный MD5
            int a = 0x67452301;
            int b = unchecked((int)0xEFCDAB89);
            int c = unchecked((int)0x98BADCFE);
            int d = 0x10325476;

            int x = System.Math.Abs(key);

            // Четыре раунда смешивания
            for (int i = 0; i < 4; i++)
            {
                int temp = d;
                d = c;
                c = b;
                b = b + ((a + x + (i * 0x5A827999)) << (i * 3));
                a = temp;
            }

            return System.Math.Abs(a ^ b ^ c ^ d) % tableSize;
        }
    }
}