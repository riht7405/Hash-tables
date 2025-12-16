using HashTablesLab.Core.Interfaces;
using System;

namespace HashTablesLab.HashFunctions
{
    public class DivisionHash : IHashFunction<int>
    {
        public string Name => "Метод деления (key % m)";

        public int Calculate(int key, int tableSize)
        {
            return System.Math.Abs(key) % tableSize;
        }
    }
}