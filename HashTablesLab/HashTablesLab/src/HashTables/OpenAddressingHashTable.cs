using HashTablesLab.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace HashTablesLab.HashTables
{
    public class OpenAddressingHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly Entry[] _table;
        private readonly IHashFunction<TKey> _hashFunction;
        private readonly ICollisionResolver _resolver;
        private int _count;
        private int _collisionCount;
        private int _probeCount;
        private int _totalInsertionTimeMs;

        private class Entry
        {
            public TKey Key { get; }
            public TValue Value { get; set; }
            public EntryStatus Status { get; set; }

            public Entry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
                Status = EntryStatus.Occupied;
            }
        }

        private enum EntryStatus
        {
            Empty,
            Occupied,
            Deleted
        }

        public OpenAddressingHashTable(int size, IHashFunction<TKey> hashFunction,
            ICollisionResolver resolver)
        {
            _table = new Entry[size];
            _hashFunction = hashFunction;
            _resolver = resolver;
            _count = 0;
            _collisionCount = 0;
            _probeCount = 0;
            _totalInsertionTimeMs = 0;
        }

        public double LoadFactor => (double)_count / _table.Length;
        public int Count => _count;

        public bool Insert(TKey key, TValue value)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Убираем ограничение 75% для возможности заполнения таблицы
            if (_count >= _table.Length)
            {
                watch.Stop();
                throw new InvalidOperationException("Таблица переполнена");
            }

            for (int i = 0; i < _table.Length; i++)
            {
                int index = _resolver.Resolve(_hashFunction.Calculate(key, _table.Length), i, _table.Length);

                // Считаем пробу
                _probeCount++;

                if (_table[index] == null || _table[index].Status == EntryStatus.Empty ||
                    _table[index].Status == EntryStatus.Deleted)
                {
                    _table[index] = new Entry(key, value);
                    _count++;

                    watch.Stop();
                    _totalInsertionTimeMs += (int)watch.ElapsedMilliseconds;
                    return true;
                }

                if (_table[index].Status == EntryStatus.Occupied &&
                    EqualityComparer<TKey>.Default.Equals(_table[index].Key, key))
                {
                    watch.Stop();
                    _totalInsertionTimeMs += (int)watch.ElapsedMilliseconds;
                    return false; // Ключ уже существует
                }

                // Коллизия при попытке вставки
                if (i > 0)
                    _collisionCount++;
            }

            watch.Stop();
            _totalInsertionTimeMs += (int)watch.ElapsedMilliseconds;
            throw new InvalidOperationException("Не удалось найти свободную ячейку");
        }

        public bool Search(TKey key, out TValue value)
        {
            for (int i = 0; i < _table.Length; i++)
            {
                int index = _resolver.Resolve(_hashFunction.Calculate(key, _table.Length), i, _table.Length);

                if (_table[index] == null)
                    break;

                if (_table[index].Status == EntryStatus.Occupied &&
                    EqualityComparer<TKey>.Default.Equals(_table[index].Key, key))
                {
                    value = _table[index].Value;
                    return true;
                }

                _probeCount++;
            }

            value = default;
            return false;
        }

        public bool Delete(TKey key)
        {
            for (int i = 0; i < _table.Length; i++)
            {
                int index = _resolver.Resolve(_hashFunction.Calculate(key, _table.Length), i, _table.Length);

                if (_table[index] == null)
                    break;

                if (_table[index].Status == EntryStatus.Occupied &&
                    EqualityComparer<TKey>.Default.Equals(_table[index].Key, key))
                {
                    _table[index].Status = EntryStatus.Deleted;
                    _count--;
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            Array.Clear(_table, 0, _table.Length);
            _count = 0;
            _collisionCount = 0;
            _probeCount = 0;
            _totalInsertionTimeMs = 0;
        }

        public int GetTableSize() => _table.Length;

        public int CalculateLongestCluster()
        {
            int longestCluster = 0;
            int currentCluster = 0;

            for (int i = 0; i < _table.Length; i++)
            {
                if (_table[i] == null || _table[i].Status != EntryStatus.Occupied)
                {
                    if (currentCluster > longestCluster)
                        longestCluster = currentCluster;
                    currentCluster = 0;
                }
                else
                {
                    currentCluster++;
                }
            }

            if (currentCluster > longestCluster)
                longestCluster = currentCluster;

            return longestCluster;
        }

        public void PrintTableState()
        {
            Console.WriteLine("\n═══════════════════════════════════════════════");
            Console.WriteLine($"Хеш-таблица (открытая адресация) | Размер: {_table.Length}");
            Console.WriteLine($"Элементов: {_count} | Заполнение: {LoadFactor:P2}");
            Console.WriteLine($"Самый длинный кластер: {CalculateLongestCluster()}");
            Console.WriteLine("═══════════════════════════════════════════════\n");

            int cols = 10;
            int rows = (int)Math.Ceiling((double)_table.Length / cols);

            for (int row = 0; row < rows; row++)
            {
                Console.Write("    ");
                for (int col = 0; col < cols; col++)
                {
                    int idx = row * cols + col;
                    if (idx >= _table.Length) break;

                    if (_table[idx] == null || _table[idx].Status != EntryStatus.Occupied)
                    {
                        Console.Write("[  ] ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        string keyStr = _table[idx].Key.ToString();
                        string display = keyStr.Length > 2 ? keyStr.Substring(0, 2) : keyStr.PadLeft(2);
                        Console.Write($"[{display}] ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }

        public void PrintHeatmap()
        {
            Console.WriteLine("\n🔥 Тепловая карта заполнения таблицы:");
            Console.WriteLine("  Пусто [ ]  Мало [░]  Средне [▒]  Много [▓]  Полно [█]\n");

            int blockSize = Math.Max(1, _table.Length / 50); // 50 символов для визуализации

            for (int i = 0; i < _table.Length; i += blockSize)
            {
                int occupiedCount = 0;
                for (int j = 0; j < blockSize && i + j < _table.Length; j++)
                {
                    if (_table[i + j] != null && _table[i + j].Status == EntryStatus.Occupied)
                        occupiedCount++;
                }

                double ratio = (double)occupiedCount / blockSize;
                Console.ForegroundColor = GetHeatmapColor(ratio);
                Console.Write(GetHeatmapChar(ratio));
                Console.ResetColor();
            }
            Console.WriteLine("\n");
        }

        private ConsoleColor GetHeatmapColor(double ratio)
        {
            return ratio switch
            {
                < 0.25 => ConsoleColor.DarkBlue,
                < 0.5 => ConsoleColor.Blue,
                < 0.75 => ConsoleColor.Green,
                < 0.9 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red
            };
        }

        private char GetHeatmapChar(double ratio)
        {
            return ratio switch
            {
                0 => ' ',
                < 0.25 => '░',
                < 0.5 => '▒',
                < 0.75 => '▓',
                _ => '█'
            };
        }

        public Core.Models.Statistics GetStatistics()
        {
            int longestCluster = 0;
            int currentCluster = 0;
            int empty = 0;

            for (int i = 0; i < _table.Length; i++)
            {
                if (_table[i] == null || _table[i].Status != EntryStatus.Occupied)
                {
                    empty++;
                    if (currentCluster > longestCluster)
                        longestCluster = currentCluster;
                    currentCluster = 0;
                }
                else
                {
                    currentCluster++;
                }
            }

            if (currentCluster > longestCluster)
                longestCluster = currentCluster;

            return new Core.Models.Statistics
            {
                LoadFactor = LoadFactor,
                LongestChain = 0, // Для открытой адресации не применимо
                ShortestChain = 0, // Для открытой адресации не применимо
                EmptyBuckets = empty,
                LongestCluster = longestCluster,
                InsertionTime = System.TimeSpan.FromMilliseconds(_totalInsertionTimeMs),
                SearchTime = System.TimeSpan.Zero,
                CollisionCount = _collisionCount,
                ProbeCount = _probeCount
            };
        }

        public bool[] GetOccupancyMap()
        {
            var map = new bool[_table.Length];
            for (int i = 0; i < _table.Length; i++)
            {
                map[i] = _table[i] != null && _table[i].Status == EntryStatus.Occupied;
            }
            return map;
        }
    }
}