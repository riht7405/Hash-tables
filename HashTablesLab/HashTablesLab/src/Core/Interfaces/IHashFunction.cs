using System;

namespace HashTablesLab.Core.Interfaces
{
    public interface IHashFunction<T>
    {
        int Calculate(T key, int tableSize);
        string Name { get; }
    }
}