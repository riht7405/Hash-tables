using HashTablesLab.Core.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HashTablesLab.HashTables
{
    public class ChainedHashTable<TKey, TValue> : IHashTable<TKey, TValue>
    {
        private readonly LinkedList<KeyValuePair<TKey, TValue>>[] _buckets;
        private readonly IHashFunction<TKey> _hashFunction;
        private int _count;
        private readonly Stopwatch _timer;

        public ChainedHashTable(int size, IHashFunction<TKey> hashFunction)
        {
            _buckets = new LinkedList<KeyValuePair<TKey, TValue>>[size];
            _hashFunction = hashFunction;
            _timer = new Stopwatch();
            _count = 0;
        }

        public double LoadFactor => (double)_count / _buckets.Length;
        public int Count => _count;

        public bool Insert(TKey key, TValue value)
        {
            _timer.Start();
            int index = _hashFunction.Calculate(key, _buckets.Length);

            if (_buckets[index] == null)
                _buckets[index] = new LinkedList<KeyValuePair<TKey, TValue>>();

            foreach (var pair in _buckets[index])
            {
                if (EqualityComparer<TKey>.Default.Equals(pair.Key, key))
                {
                    _timer.Stop();
                    return false;
                }
            }

            _buckets[index].AddLast(new KeyValuePair<TKey, TValue>(key, value));
            _count++;
            _timer.Stop();
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
                _buckets[i] = new LinkedList<KeyValuePair<TKey, TValue>>();
            _count = 0;
            _timer.Reset();
        }

        public Core.Models.Statistics GetStatistics()
        {
            int longest = 0;
            int shortest = int.MaxValue;
            int empty = 0;

            foreach (var bucket in _buckets)
            {
                if (bucket == null || bucket.Count == 0)
                {
                    empty++;
                    shortest = 0;
                }
                else
                {
                    if (bucket.Count > longest)
                        longest = bucket.Count;
                    if (bucket.Count < shortest)
                        shortest = bucket.Count;
                }
            }

            if (shortest == int.MaxValue)
                shortest = 0;

            return new Core.Models.Statistics
            {
                LoadFactor = LoadFactor,
                LongestChain = longest,
                ShortestChain = shortest,
                EmptyBuckets = empty,
                LongestCluster = 0,
                InsertionTime = _timer.Elapsed,
                SearchTime = TimeSpan.Zero,
                CollisionCount = 0,
                ProbeCount = 0
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