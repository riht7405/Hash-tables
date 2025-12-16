using System;
using System.Collections.Generic;

namespace HashTablesLab.Benchmark
{
    public static class DataGenerator
    {
        private static readonly Random _random = new Random();

        public static int[] GenerateUniqueKeys(int count, int min = 1, int max = 1000000)
        {
            HashSet<int> keys = new HashSet<int>();

            while (keys.Count < count)
            {
                keys.Add(_random.Next(min, max));
            }

            int[] result = new int[count];
            keys.CopyTo(result);

            // Перемешивание
            for (int i = result.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (result[i], result[j]) = (result[j], result[i]);
            }

            return result;
        }
    }
}