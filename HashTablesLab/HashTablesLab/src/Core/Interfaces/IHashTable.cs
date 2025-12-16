using System;

namespace HashTablesLab.Core.Interfaces
{
    public interface IHashTable<TKey, TValue>
    {
        bool Insert(TKey key, TValue value);
        bool Search(TKey key, out TValue value);
        bool Delete(TKey key);
        double LoadFactor { get; }
        int Count { get; }
        void Clear();
        Core.Models.Statistics GetStatistics();
    }
}