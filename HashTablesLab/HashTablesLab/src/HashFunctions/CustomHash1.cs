using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.HashFunctions
{
    public class CustomHash1 : IHashFunction<int>
    {
        public string Name => "Собственный метод 1 (XOR сдвиг)";

        public int Calculate(int key, int tableSize)
        {
            // XOR с циклическим сдвигом для лучшего распределения
            uint hash = (uint)System.Math.Abs(key);
            hash = (hash << 15) ^ (hash >> 17);
            hash = hash * 0x85ebca6bu;
            hash = hash ^ (hash >> 16);
            return (int)(hash % (uint)tableSize);
        }
    }
}