using HashTablesLab.HashTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HashTablesLab.Visualization
{
    public static class HashTableVisualizer
    {
        public static void DrawChainedHashTable<TKey, TValue>(
            ChainedHashTable<TKey, TValue> table,
            string title = "Хеш-таблица с цепочками",
            int maxWidth = 80)
        {
            Console.Clear();
            DrawBox(title, maxWidth);

            var chainLengths = table.GetChainLengths();
            int maxChainLength = chainLengths.Max();

            Console.WriteLine($"\n  Размер таблицы: {chainLengths.Length} ячеек");
            Console.WriteLine($"  Элементов: {table.Count}");
            Console.WriteLine($"  Коэффициент заполнения: {table.LoadFactor:P2}");
            Console.WriteLine($"  Самая длинная цепочка: {maxChainLength}");

            DrawHistogram(chainLengths, "Распределение цепочек", maxWidth);

            // Компактное представление
            Console.WriteLine("\n  Компактное представление:");
            Console.WriteLine("  " + new string('─', 60));

            int itemsPerRow = 10;
            for (int i = 0; i < chainLengths.Length; i += itemsPerRow)
            {
                Console.Write("  ");
                for (int j = 0; j < itemsPerRow && i + j < chainLengths.Length; j++)
                {
                    int length = chainLengths[i + j];
                    Console.BackgroundColor = GetBucketColor(length, maxChainLength);
                    Console.Write(length == 0 ? "  " : $"{length,2}");
                    Console.ResetColor();
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }

        public static void DrawOpenAddressingHashTable<TKey, TValue>(
            OpenAddressingHashTable<TKey, TValue> table,
            string title = "Хеш-таблица с открытой адресацией",
            int maxWidth = 80)
        {
            Console.Clear();
            DrawBox(title, maxWidth);

            var occupancyMap = table.GetOccupancyMap();
            var stats = table.GetStatistics();

            Console.WriteLine($"\n  Размер таблицы: {occupancyMap.Length} ячеек");
            Console.WriteLine($"  Элементов: {table.Count}");
            Console.WriteLine($"  Коэффициент заполнения: {table.LoadFactor:P2}");
            Console.WriteLine($"  Самый длинный кластер: {stats.LongestCluster}");

            // Визуализация кластеров
            Console.WriteLine("\n  Кластеры (последовательные занятые ячейки):");
            Console.WriteLine("  " + new string('─', 60));

            int clusterCounter = 0;
            int currentCluster = 0;
            int maxClustersPerLine = 15;

            for (int i = 0; i < occupancyMap.Length; i++)
            {
                if (occupancyMap[i])
                {
                    currentCluster++;
                }
                else if (currentCluster > 0)
                {
                    Console.BackgroundColor = GetClusterColor(currentCluster);
                    Console.Write($"{currentCluster,2}");
                    Console.ResetColor();
                    Console.Write(" ");

                    clusterCounter++;
                    currentCluster = 0;

                    if (clusterCounter % maxClustersPerLine == 0)
                        Console.WriteLine();
                }
            }

            if (currentCluster > 0)
            {
                Console.BackgroundColor = GetClusterColor(currentCluster);
                Console.Write($"{currentCluster,2}");
                Console.ResetColor();
            }

            Console.WriteLine("\n");

            // Тепловая карта
            DrawHeatmap(occupancyMap, "Тепловая карта заполнения", maxWidth);
        }

        private static void DrawHistogram(int[] data, string title, int maxWidth)
        {
            Console.WriteLine($"\n  {title}:");
            Console.WriteLine("  " + new string('─', maxWidth - 2));

            int maxValue = data.Max();
            if (maxValue == 0) return;

            var frequency = new Dictionary<int, int>();
            foreach (var value in data)
            {
                if (frequency.ContainsKey(value))
                    frequency[value]++;
                else
                    frequency[value] = 1;
            }

            int maxBarLength = maxWidth - 15;

            foreach (var kvp in frequency.OrderBy(k => k.Key))
            {
                int barLength = (int)((double)kvp.Value / data.Length * maxBarLength);
                Console.Write($"  {kvp.Key,2}: ");
                Console.BackgroundColor = GetFrequencyColor(kvp.Key);
                Console.Write(new string(' ', Math.Max(1, barLength)));
                Console.ResetColor();
                Console.WriteLine($" {kvp.Value,4} ({kvp.Value * 100.0 / data.Length,5:F1}%)");
            }
        }

        private static void DrawHeatmap(bool[] occupancy, string title, int maxWidth)
        {
            Console.WriteLine($"\n  {title}:");
            Console.WriteLine("  " + new string('─', maxWidth - 2));

            int blockSize = Math.Max(1, occupancy.Length / 60); // 60 символов ширины
            int height = 8;

            // Создаем матрицу для отображения
            char[,] grid = new char[height, 60];

            for (int col = 0; col < 60; col++)
            {
                int startIdx = col * blockSize;
                int occupiedCount = 0;

                for (int j = 0; j < blockSize && startIdx + j < occupancy.Length; j++)
                {
                    if (occupancy[startIdx + j])
                        occupiedCount++;
                }

                double ratio = (double)occupiedCount / blockSize;
                int fillHeight = (int)(ratio * height);

                for (int row = 0; row < height; row++)
                {
                    grid[row, col] = row < fillHeight ? '█' : ' ';
                }
            }

            // Рисуем тепловую карту
            for (int row = height - 1; row >= 0; row--)
            {
                Console.Write("  ");
                for (int col = 0; col < 60; col++)
                {
                    if (grid[row, col] == '█')
                    {
                        double ratio = 1.0 - (double)row / height;
                        Console.ForegroundColor = GetHeatmapColor(ratio);
                        Console.Write('█');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }

            // Легенда
            Console.WriteLine("\n  Легенда: ░ 0-12% ▒ 13-37% ▓ 38-62% █ 63-100%");
        }

        private static void DrawBox(string title, int width)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔" + new string('═', width - 2) + "╗");
            Console.WriteLine("║" + title.PadRight(width - 2).PadLeft(width - 2 + title.Length / 2) + "║");
            Console.WriteLine("╚" + new string('═', width - 2) + "╝");
            Console.ResetColor();
        }

        private static ConsoleColor GetBucketColor(int length, int maxLength)
        {
            if (length == 0) return ConsoleColor.Black;

            double ratio = (double)length / maxLength;
            return ratio switch
            {
                < 0.25 => ConsoleColor.DarkGreen,
                < 0.5 => ConsoleColor.Green,
                < 0.75 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red
            };
        }

        private static ConsoleColor GetClusterColor(int clusterSize)
        {
            return clusterSize switch
            {
                1 => ConsoleColor.DarkGreen,
                <= 3 => ConsoleColor.Green,
                <= 7 => ConsoleColor.Yellow,
                <= 15 => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Red
            };
        }

        private static ConsoleColor GetFrequencyColor(int frequency)
        {
            return frequency switch
            {
                0 => ConsoleColor.DarkGray,
                1 => ConsoleColor.Green,
                2 => ConsoleColor.Yellow,
                3 => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Red
            };
        }

        private static ConsoleColor GetHeatmapColor(double ratio)
        {
            return ratio switch
            {
                < 0.25 => ConsoleColor.DarkBlue,
                < 0.5 => ConsoleColor.Blue,
                < 0.75 => ConsoleColor.Green,
                < 0.9 => ConsoleColor.Yellow,
                _ => ConsoleColor.Red
            };
        }
    }
}