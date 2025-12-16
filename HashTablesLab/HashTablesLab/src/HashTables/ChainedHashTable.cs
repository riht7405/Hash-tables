using HashTablesLab.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace HashTablesLab.HashTables
{
    public class ChainedHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly LinkedList<KeyValuePair<TKey, TValue>>[] _buckets;
        private readonly IHashFunction<TKey> _hashFunction;
        private int _count;
        private int _collisionCount;
        private int _totalInsertionTimeMs;

        public ChainedHashTable(int size, IHashFunction<TKey> hashFunction)
        {
            _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[size];
            _hashFunction = hashFunction;
            _count = 0;
            _collisionCount = 0;
            _totalInsertionTimeMs = 0;
        }

        public double LoadFactor => (double)_count / _buckets.Length;
        public int Count => _count;

        public bool Insert(TKey key, TValue value)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            int index = _hashFunction.Calculate(key, _buckets.Length);

            if (_buckets[index] == null)
                _buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();

            // Проверяем коллизии
            if (_buckets[index].Count > 0)
                _collisionCount++;

            // Проверка на существующий ключ
            foreach (var pair in _buckets[index])
            {
                if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                {
                    watch.Stop();
                    return false;
                }
            }

            _buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            _count++;

            watch.Stop();
            _totalInsertionTimeMs += (int)watch.ElapsedMilliseconds;

            return true;
        }

        public bool Search(TKey key, out TValue value)
        {
            int index = _hashFunction.Calculate(key, _buckets.Length);

            if (_buckets[index] != null)
            {
                foreach (var pair in _buckets[index])
                {
                    if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                    {
                        value = pair.Value;
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }

        public int GetTableSize() => _buckets.Length;

        public int CalculateLongestCluster()
        {
            // Для метода цепочек кластеры не применимы, возвращаем 0
            return 0;
        }

        public void PrintTableState()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════");
            Console.WriteLine($"Хеш-таблица (цепочки) | Размер: {_buckets.Length}");
            Console.WriteLine($"Элементов: {_count} | Заполнение: {LoadFactor:P2}");
            Console.WriteLine("═══════════════════════════════════════════════\n");

            for (int i = 0; i < _buckets.Length; i++)
            {
                Console.Write($"[{i,3}] → ");

                if (_buckets[i] == null || _buckets[i].Count == 0)
                {
                    Console.WriteLine("∅");
                }
                else
                {
                    bool first = true;
                    foreach (var pair in _buckets[i])
                    {
                        if (!first) Console.Write(" → ");
                        Console.Write($"{pair.Key}");
                        first = false;
                    }
                    Console.WriteLine($" ({_buckets[i].Count} элемент(ов))");
                }
            }
        }

        // Альтернативно - компактная визуализация
        public void PrintCompactView(int maxBucketsToShow = 20)
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Хеш-таблица с цепочками ({_buckets.Length} ячеек)   │");
            Console.WriteLine("├─────────────────────────────────────────────────────┤");

            int step = Math.Max(1, _buckets.Length / maxBucketsToShow);

            for (int i = 0; i < Math.Min(_buckets.Length, maxBucketsToShow); i++)
            {
                int idx = i * step;
                int chainLength = _buckets[idx]?.Count ?? 0;

                string bar = new string('█', Math.Min(chainLength * 2, 30));
                string emptyIndicator = chainLength == 0 ? "[ПУСТО]" : "";

                Console.WriteLine($"│ [{idx,4}] {bar,-30} {chainLength,2} элем. {emptyIndicator,-6} │");
            }

            Console.WriteLine("└─────────────────────────────────────────────────────┘");
        }

        public bool Delete(TKey key)
        {
            int index = _hashFunction.Calculate(key, _buckets.Length);

            if (_buckets[index] != null)
            {
                var node = _buckets[index].First;
                while (node != null)
                {
                    if (EqualityComparer<TKey>.Default.Equals(node.Value.Key, key))
                    {
                        _buckets[index].Remove(node);
                        _count--;
                        return true;
                    }
                    node = node.Next;
                }
            }

            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < _buckets.Length; i++)
                _buckets[i] = null;
            _count = 0;
            _collisionCount = 0;
            _totalInsertionTimeMs = 0;
        }

        public Core.Models.Statistics GetStatistics()
        {
            int longest = 0;
            int shortest = int.MaxValue;
            int empty = 0;

            foreach (var bucket in _buckets)
            {
                int count = bucket?.Count ?? 0;

                if (count == 0)
                {
                    empty++;
                }

                if (count > longest)
                    longest = count;

                if (count < shortest && count > 0)
                    shortest = count;
            }

            if (shortest == int.MaxValue)
                shortest = 0;

            return new Core.Models.Statistics
            {
                LoadFactor = LoadFactor,
                LongestChain = longest,
                ShortestChain = shortest,
                EmptyBuckets = empty,
                LongestCluster = 0, // Для метода цепочек не применимо
                InsertionTime = System.TimeSpan.FromMilliseconds(_totalInsertionTimeMs),
                SearchTime = System.TimeSpan.Zero,
                CollisionCount = _collisionCount,
                ProbeCount = 0 // Для метода цепочек не применимо
            };
        }

        public int[] GetChainLengths()
        {
            var lengths = new int[_buckets.Length];
            for (int i = 0; i < _buckets.Length; i++)
            {
                lengths[i] = _buckets[i]?.Count ?? 0;
            }
            return lengths;
        }
    }
}