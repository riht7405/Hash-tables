using System;

namespace HashTablesLab.Core.Models
{
    /// <summary>
    /// Статистика работы хеш-таблицы
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Коэффициент заполнения таблицы
        /// </summary>
        public double LoadFactor { get; set; }

        /// <summary>
        /// Длина самой длинной цепочки
        /// </summary>
        public int LongestChain { get; set; }

        /// <summary>
        /// Длина самой короткой цепочки
        /// </summary>
        public int ShortestChain { get; set; }

        /// <summary>
        /// Количество пустых ячеек
        /// </summary>
        public int EmptyBuckets { get; set; }

        /// <summary>
        /// Длина самого длинного кластера
        /// </summary>
        public int LongestCluster { get; set; }

        /// <summary>
        /// Время вставки элементов
        /// </summary>
        public TimeSpan InsertionTime { get; set; }

        /// <summary>
        /// Время поиска элементов
        /// </summary>
        public TimeSpan SearchTime { get; set; }

        /// <summary>
        /// Количество коллизий
        /// </summary>
        public int CollisionCount { get; set; }

        /// <summary>
        /// Количество проб при разрешении коллизий
        /// </summary>
        public int ProbeCount { get; set; }

        /// <summary>
        /// Строковое представление статистики
        /// </summary>
        public override string ToString()
        {
            return $"┌──────────────────────────────────────────┐\n" +
                   $"│            СТАТИСТИКА                   │\n" +
                   $"├──────────────────────────────────────────┤\n" +
                   $"│ Коэффициент заполнения: {LoadFactor,15:P2} │\n" +
                   $"│ Самая длинная цепочка:  {LongestChain,15} │\n" +
                   $"│ Самая короткая цепочка: {ShortestChain,15} │\n" +
                   $"│ Пустых ячеек:           {EmptyBuckets,15} │\n" +
                   $"│ Самый длинный кластер:  {LongestCluster,15} │\n" +
                   $"│ Количество коллизий:    {CollisionCount,15} │\n" +
                   $"│ Количество проб:        {ProbeCount,15} │\n" +
                   $"│ Время вставки:          {InsertionTime.TotalMilliseconds,12:F2} мс │\n" +
                   $"│ Время поиска:           {SearchTime.TotalMilliseconds,12:F2} мс │\n" +
                   $"└──────────────────────────────────────────┘";
        }

        /// <summary>
        /// Краткое строковое представление
        /// </summary>
        public string ToShortString()
        {
            return $"{LoadFactor:P2} | Цепь: {LongestChain} | Кластер: {LongestCluster} | Время: {InsertionTime.TotalMilliseconds:F2}мс";
        }

        /// <summary>
        /// Сериализация в CSV строку
        /// </summary>
        public string ToCsv()
        {
            return $"{LoadFactor:F4},{LongestChain},{ShortestChain},{EmptyBuckets},{LongestCluster},{CollisionCount},{ProbeCount},{InsertionTime.TotalMilliseconds:F2},{SearchTime.TotalMilliseconds:F2}";
        }

        /// <summary>
        /// Заголовок CSV файла
        /// </summary>
        public static string CsvHeader()
        {
            return "LoadFactor,LongestChain,ShortestChain,EmptyBuckets,LongestCluster,CollisionCount,ProbeCount,InsertionTimeMs,SearchTimeMs";
        }
    }
}