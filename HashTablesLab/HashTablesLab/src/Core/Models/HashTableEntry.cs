namespace HashTablesLab.Core.Models
{
    public class HashTableEntry<TKey, TValue>
    {
        public TKey Key { get; }
        public TValue Value { get; set; }
        public bool IsDeleted { get; set; }
        public EntryStatus Status { get; set; }

        public HashTableEntry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            IsDeleted = false;
            Status = EntryStatus.Occupied;
        }

        public enum EntryStatus
        {
            Empty,
            Occupied,
            Deleted
        }
    }
}