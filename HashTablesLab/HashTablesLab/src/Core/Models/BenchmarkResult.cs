using System;
using HashTablesLab.Core.Enums;

namespace HashTablesLab.Core.Models
{
    /// <summary>
    /// Результат бенчмарка для хеш-таблицы
    /// </summary>
    public class BenchmarkResult
    {
        /// <summary>
        /// Название теста
        /// </summary>
        public string TestName { get; set; } = string.Empty;

        /// <summary>
        /// Метод хеширования
        /// </summary>
        public HashMethodType HashMethod { get; set; }

        /// <summary>
        /// Метод разрешения коллизий
        /// </summary>
        public CollisionResolutionType ResolutionMethod { get; set; }

        /// <summary>
        /// Время выполнения теста
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Длительность теста
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Размер таблицы
        /// </summary>
        public int TableSize { get; set; }

        /// <summary>
        /// Количество элементов для вставки
        /// </summary>
        public int ElementCount { get; set; }

        /// <summary>
        /// Количество успешно вставленных элементов
        /// </summary>
        public int InsertedCount { get; set; }

        /// <summary>
        /// Статистика теста
        /// </summary>
        public Statistics Statistics { get; set; } = new Statistics();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public BenchmarkResult()
        {
            Timestamp = DateTime.Now;
        }

        /// <summary>
        /// Строковое представление результата
        /// </summary>
        public override string ToString()
        {
            return $"Тест: {TestName}\n" +
                   $"Метод хеширования: {HashMethod}\n" +
                   $"Метод разрешения коллизий: {ResolutionMethod}\n" +
                   $"Размер таблицы: {TableSize}\n" +
                   $"Количество элементов: {ElementCount}\n" +
                   $"Вставлено элементов: {InsertedCount}\n" +
                   $"Длительность теста: {Duration.TotalMilliseconds:F2} мс\n" +
                   $"Статистика:\n{Statistics}";
        }

        /// <summary>
        /// Краткий отчет
        /// </summary>
        public string ToShortReport()
        {
            return $"{TestName,-30} | {HashMethod,-15} | {ResolutionMethod,-20} | " +
                   $"{Statistics.LoadFactor,10:P2} | {Statistics.LongestChain,5} | " +
                   $"{Statistics.LongestCluster,5} | {Duration.TotalMilliseconds,10:F2} мс";
        }

        /// <summary>
        /// Сериализация в JSON
        /// </summary>
        public string ToJson()
        {
            return $"{{" +
                   $"\"TestName\":\"{TestName}\"," +
                   $"\"HashMethod\":\"{HashMethod}\"," +
                   $"\"ResolutionMethod\":\"{ResolutionMethod}\"," +
                   $"\"Timestamp\":\"{Timestamp:yyyy-MM-dd HH:mm:ss}\"," +
                   $"\"DurationMs\":{Duration.TotalMilliseconds}," +
                   $"\"TableSize\":{TableSize}," +
                   $"\"ElementCount\":{ElementCount}," +
                   $"\"InsertedCount\":{InsertedCount}," +
                   $"\"Statistics\":{{" +
                   $"\"LoadFactor\":{Statistics.LoadFactor}," +
                   $"\"LongestChain\":{Statistics.LongestChain}," +
                   $"\"ShortestChain\":{Statistics.ShortestChain}," +
                   $"\"EmptyBuckets\":{Statistics.EmptyBuckets}," +
                   $"\"LongestCluster\":{Statistics.LongestCluster}," +
                   $"\"CollisionCount\":{Statistics.CollisionCount}," +
                   $"\"ProbeCount\":{Statistics.ProbeCount}," +
                   $"\"InsertionTimeMs\":{Statistics.InsertionTime.TotalMilliseconds}," +
                   $"\"SearchTimeMs\":{Statistics.SearchTime.TotalMilliseconds}" +
                   $"}}" +
                   $"}}";
        }
    }
}