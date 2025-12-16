using HashTablesLab.Core.Interfaces;
using System;

namespace HashTablesLab.HashFunctions
{
    public class CustomHash2 : IHashFunction<int>
    {
        public string Name => "Собственный метод 2 (FNV-1a inspired)";

        public int Calculate(int key, int tableSize)
        {
            // Упрощенная версия FNV-1a хеша
            const uint prime = 16777619u;
            uint hash = 2166136261u;

            byte[] bytes = BitConverter.GetBytes(key);
            foreach (byte b in bytes)
            {
                hash = (hash ^ b) * prime;
            }

            return (int)(hash % (uint)tableSize);
        }
    }
}