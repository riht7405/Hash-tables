using HashTablesLab.Core.Interfaces;
using HashTablesLab.Core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HashTablesLab.HashTables
{
    public class OpenAddressingHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly Entry[] _table;
        private readonly IHashFunction<TKey> _hashFunction;
        private readonly ICollisionResolver _resolver;
        private int _count;
        private readonly Stopwatch _timer;

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
            _timer = new Stopwatch();
            _count = 0;
        }

        public double LoadFactor => (double)_count / _table.Length;
        public int Count => _count;

        public bool Insert(TKey key, TValue value)
        {
            _timer.Start();

            if (_count >= _table.Length * 0.75)
                throw new InvalidOperationException("Таблица близка к переполнению");

            for (int i = 0; i < _table.Length; i++)
            {
                int index = _resolver.Resolve(_hashFunction.Calculate(key, _table.Length), i, _table.Length);

                if (_table[index] == null || _table[index].Status == EntryStatus.Deleted)
                {
                    _table[index] = new Entry(key, value);
                    _count++;
                    _timer.Stop();
                    return true;
                }

                if (_table[index].Status == EntryStatus.Occupied &&
                    EqualityComparer<TKey>.Default.Equals(_table[index].Key, key))
                {
                    _timer.Stop();
                    return false;
                }
            }

            _timer.Stop();
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
            for (int i = 0; i < _table.Length; i++)
                _table[i] = new Entry(default, default) { Status = EntryStatus.Empty };
            _count = 0;
            _timer.Reset();
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
                LongestChain = 0,
                ShortestChain = 0,
                EmptyBuckets = empty,
                LongestCluster = longestCluster,
                InsertionTime = _timer.Elapsed,
                SearchTime = TimeSpan.Zero,
                CollisionCount = 0,
                ProbeCount = 0
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