using HashTablesLab.Core.Interfaces;
using HashTablesLab.Core.Models;
using HashTablesLab.HashTables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HashTablesLab.Visualization
{
    public static class ConsoleVisualizer
    {
        private const int ChartWidth = 60;

        /// <summary>
        /// Показать подробную статистику таблицы
        /// </summary>
        public static void DisplayHashTableStats(IHashTable<int, string> table, string title)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n╔{new string('═', 78)}╗");
            Console.WriteLine($"║ {title,-76} ║");
            Console.WriteLine($"╚{new string('═', 78)}╝");
            Console.ResetColor();

            var stats = table.GetStatistics();
            Console.WriteLine(stats);
        }

        /// <summary>
        /// Нарисовать понятный график распределения длин цепочек
        /// </summary>
        public static void DrawChainDistributionChart(IHashTable<int, string> table)
        {
            Console.WriteLine("\n📊 РАСПРЕДЕЛЕНИЕ ДЛИН ЦЕПОЧЕК:");
            Console.WriteLine("(показывает сколько ячеек имеют цепочки разной длины)");
            Console.WriteLine("┌" + new string('─', 64) + "┐");

            if (table is ChainedHashTable<int, string> chainedTable)
            {
                var chainLengths = chainedTable.GetChainLengths();
                DrawChainLengthHistogram(chainLengths);
            }

            Console.WriteLine("└" + new string('─', 64) + "┘");
        }

        /// <summary>
        /// Гистограмма длин цепочек (понятная версия)
        /// </summary>
        private static void DrawChainLengthHistogram(int[] chainLengths)
        {
            if (chainLengths.Length == 0) return;

            // Группируем по длинам цепочек
            var lengthGroups = new Dictionary<int, int>();
            foreach (var length in chainLengths)
            {
                if (lengthGroups.ContainsKey(length))
                    lengthGroups[length]++;
                else
                    lengthGroups[length] = 1;
            }

            // Сортируем по длине цепочки
            var sortedGroups = lengthGroups.OrderBy(g => g.Key).ToList();

            int maxCount = sortedGroups.Max(g => g.Value);
            if (maxCount == 0) return;

            Console.WriteLine(" Длина │ Количество ячеек │ График");
            Console.WriteLine("───────┼──────────────────┼" + new string('─', 40));

            foreach (var group in sortedGroups)
            {
                if (group.Key > 20) continue; // Ограничиваем для наглядности

                int barLength = (int)((double)group.Value / maxCount * 40);
                string bar = new string('█', barLength);

                Console.WriteLine($" {group.Key,5} │ {group.Value,16} │ {bar} {group.Value}");
            }

            // Сводная статистика
            int emptyBuckets = chainLengths.Count(l => l == 0);
            int maxChain = chainLengths.Max();
            double avgChain = chainLengths.Average();

            Console.WriteLine("\n📈 СВОДНАЯ СТАТИСТИКА:");
            Console.WriteLine($" • Пустых ячеек: {emptyBuckets} ({emptyBuckets * 100.0 / chainLengths.Length:F1}%)");
            Console.WriteLine($" • Максимальная длина цепочки: {maxChain}");
            Console.WriteLine($" • Средняя длина цепочки: {avgChain:F2}");
            Console.WriteLine($" • Всего ячеек: {chainLengths.Length}");
        }

        /// <summary>
        /// Понятная тепловая карта заполнения таблицы
        /// </summary>
        public static void DrawHeatmap(IHashTable<int, string> table, int width = 60, int height = 15)
        {
            Console.WriteLine("\n🔥 ТЕПЛОВАЯ КАРТА ЗАПОЛНЕНИЯ ТАБЛИЦЫ:");
            Console.WriteLine("(показывает какие ячейки заняты, а какие пусты)");

            if (table is OpenAddressingHashTable<int, string> openTable)
            {
                var occupancyMap = openTable.GetOccupancyMap();
                DrawSimpleHeatmap(occupancyMap, width, height);
            }
        }

        /// <summary>
        /// Простая и понятная тепловая карта
        /// </summary>
        private static void DrawSimpleHeatmap(bool[] occupancyMap, int width, int height)
        {
            int cellsPerBlock = (int)Math.Ceiling((double)occupancyMap.Length / (width * height));

            Console.WriteLine($"\nКаждый символ представляет {cellsPerBlock} ячеек таблицы");
            Console.WriteLine("┌" + new string('─', width) + "┐");

            int totalOccupied = 0;

            for (int y = 0; y < height; y++)
            {
                Console.Write("│");
                for (int x = 0; x < width; x++)
                {
                    int startIdx = (y * width + x) * cellsPerBlock;
                    int endIdx = Math.Min(startIdx + cellsPerBlock, occupancyMap.Length);

                    int occupied = 0;
                    for (int i = startIdx; i < endIdx; i++)
                    {
                        if (occupancyMap[i])
                            occupied++;
                    }

                    totalOccupied += occupied;

                    // Простая визуализация: █ - занято, · - пусто
                    if (occupied == 0)
                    {
                        Console.Write("·");
                    }
                    else if (occupied == cellsPerBlock)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                    else
                    {
                        double ratio = (double)occupied / cellsPerBlock;
                        if (ratio > 0.75)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("▓");
                        }
                        else if (ratio > 0.5)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.Write("▒");
                        }
                        else if (ratio > 0.25)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("░");
                        }
                        else
                        {
                            Console.Write("·");
                        }
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("│");
            }

            Console.WriteLine("└" + new string('─', width) + "┘");

            // Легенда
            Console.WriteLine("\n📖 ЛЕГЕНДА:");
            Console.WriteLine(" · - полностью пустой блок (0% занято)");
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("░");
            Console.ResetColor();
            Console.WriteLine(" - мало занято (< 25%)");
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("▒");
            Console.ResetColor();
            Console.WriteLine(" - средне занято (25-50%)");
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("▓");
            Console.ResetColor();
            Console.WriteLine(" - много занято (50-75%)");
            Console.Write(" ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("█");
            Console.ResetColor();
            Console.WriteLine(" - полностью занят (100%)");

            // Статистика
            double occupancyRate = (double)totalOccupied / occupancyMap.Length;
            Console.WriteLine($"\n📊 СТАТИСТИКА:");
            Console.WriteLine($" • Занято ячеек: {totalOccupied} из {occupancyMap.Length}");
            Console.WriteLine($" • Процент заполнения: {occupancyRate:P1}");
        }

        /// <summary>
        /// Показать сравнительную таблицу результатов
        /// </summary>
        public static void DisplayComparisonTable(List<BenchmarkResult> results)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                             СРАВНИТЕЛЬНАЯ ТАБЛИЦА                                   ║");
            Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Тест                     │ Метод хеша    │ Разр. коллизий  │ Зап. │ Цепь │ Кластер │ Время   ║");
            Console.WriteLine("╟──────────────────────────┼───────────────┼─────────────────┼──────┼──────┼─────────┼─────────╢");

            foreach (var result in results)
            {
                string testName = result.TestName.Length > 25 ? result.TestName.Substring(0, 22) + "..." : result.TestName;
                string hashMethod = result.HashMethod.ToString();
                if (hashMethod.Length > 13) hashMethod = hashMethod.Substring(0, 10) + "...";
                string resolution = result.ResolutionMethod.ToString();
                if (resolution.Length > 15) resolution = resolution.Substring(0, 12) + "...";

                Console.WriteLine($"║ {testName,-25} │ {hashMethod,-13} │ {resolution,-15} │ " +
                                $"{result.Statistics.LoadFactor,4:P0} │ {result.Statistics.LongestChain,4} │ " +
                                $"{result.Statistics.LongestCluster,7} │ {result.Duration.TotalMilliseconds,7:F1} ║");
            }

            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        /// <summary>
        /// Понятная гистограмма сравнения результатов
        /// </summary>
        public static void DrawComparisonHistogram(List<BenchmarkResult> results, string title, string metricType)
        {
            Console.WriteLine($"\n📊 {title}");
            Console.WriteLine($"(сравнение по метрике: {metricType})");
            Console.WriteLine(new string('═', 70));

            // Определяем метрику для сравнения
            var sortedResults = metricType == "Цепь" ?
                results.OrderBy(result => result.Statistics.LongestChain).ToList() :
                results.OrderBy(result => result.Statistics.LongestCluster).ToList();

            int maxValue = metricType == "Цепь" ?
                sortedResults.Max(result => result.Statistics.LongestChain) :
                sortedResults.Max(result => result.Statistics.LongestCluster);

            if (maxValue == 0) maxValue = 1; // Чтобы избежать деления на ноль

            Console.WriteLine(" Метод                          │ Значение │ График");
            Console.WriteLine("────────────────────────────────┼──────────┼" + new string('─', 40));

            foreach (var result in sortedResults)
            {
                int value = metricType == "Цепь" ?
                    result.Statistics.LongestChain :
                    result.Statistics.LongestCluster;

                string methodName = result.TestName.Length > 30 ?
                    result.TestName.Substring(0, 27) + "..." :
                    result.TestName;

                int barLength = (int)((double)value / maxValue * 40);
                string bar = new string('█', barLength);

                Console.ForegroundColor = GetPerformanceColor(value, metricType);
                Console.WriteLine($" {methodName,-30} │ {value,8} │ {bar}");
                Console.ResetColor();
            }

            // Объяснение
            Console.WriteLine("\n💡 КАК ЧИТАТЬ ЭТУ ГИСТОГРАММУ:");
            if (metricType == "Цепь")
            {
                Console.WriteLine(" • Чем КОРОЧЕ столбец, тем ЛУЧШЕ (меньше коллизий)");
                Console.WriteLine(" • Идеально: все цепочки длиной 1-2 элемента");
                Console.WriteLine(" • Плохо: длинные цепочки замедляют поиск");
            }
            else // Кластер
            {
                Console.WriteLine(" • Чем КОРОЧЕ столбец, тем ЛУЧШЕ (меньше кластеризации)");
                Console.WriteLine(" • Идеально: отдельные занятые ячейки");
                Console.WriteLine(" • Плохо: длинные кластеры увеличивают время поиска");
            }
        }

        /// <summary>
        /// Показать прогресс выполнения
        /// </summary>
        public static void ShowProgressBar(string message, int current, int total)
        {
            int width = 50;
            double percentage = (double)current / total;
            int progress = (int)(width * percentage);

            Console.Write($"\r{message}: [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('█', progress));
            Console.ResetColor();
            Console.Write(new string(' ', width - progress));
            Console.Write($"] {percentage:P1}");

            if (current >= total)
                Console.WriteLine();
        }

        /// <summary>
        /// Нарисовать простую полосу для одного значения
        /// </summary>
        public static void DrawSimpleBar(string label, int value, int maxWidth = 50)
        {
            int barLength = Math.Min(value, maxWidth);
            Console.Write($"{label,-30}: ");
            Console.ForegroundColor = GetPerformanceColor(value, "generic");
            Console.Write(new string('█', barLength));
            Console.ResetColor();
            Console.WriteLine($" {value}");
        }

        /// <summary>
        /// Получить цвет в зависимости от производительности
        /// </summary>
        private static ConsoleColor GetPerformanceColor(int value, string metricType)
        {
            if (metricType == "Цепь")
            {
                return value switch
                {
                    < 3 => ConsoleColor.Green,    // Отлично
                    < 10 => ConsoleColor.Yellow,  // Хорошо
                    < 20 => ConsoleColor.DarkYellow, // Средне
                    _ => ConsoleColor.Red         // Плохо
                };
            }
            else if (metricType == "Кластер")
            {
                return value switch
                {
                    < 5 => ConsoleColor.Green,    // Отлично
                    < 15 => ConsoleColor.Yellow,  // Хорошо
                    < 30 => ConsoleColor.DarkYellow, // Средне
                    _ => ConsoleColor.Red         // Плохо
                };
            }
            else
            {
                return value switch
                {
                    < 10 => ConsoleColor.Green,
                    < 20 => ConsoleColor.Yellow,
                    < 30 => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.Red
                };
            }
        }
    }
}