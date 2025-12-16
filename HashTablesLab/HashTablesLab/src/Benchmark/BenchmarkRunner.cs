using HashTablesLab.Core.Interfaces;
using HashTablesLab.HashTables;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HashTablesLab.Benchmark
{
    public class BenchmarkRunner
    {
        public List<Core.Models.BenchmarkResult> RunChainedBenchmark(
            IHashFunction<int>[] functions,
            int tableSize,
            int elementCount)
        {
            var results = new List<Core.Models.BenchmarkResult>();
            var keys = DataGenerator.GenerateUniqueKeys(elementCount);

            Console.WriteLine($"\nГенерация {elementCount} уникальных ключей...");

            foreach (var function in functions)
            {
                Console.WriteLine($"\nТестирование: {function.Name}");
                Console.WriteLine(new string('-', 50));

                var table = new ChainedHashTable<int, string>(tableSize, function);
                var stopwatch = Stopwatch.StartNew();
                int inserted = 0;

                foreach (int key in keys)
                {
                    if (table.Insert(key, $"Value_{key}"))
                        inserted++;
                }

                stopwatch.Stop();
                var stats = table.GetStatistics();

                results.Add(new Core.Models.BenchmarkResult
                {
                    TestName = function.Name,
                    Statistics = stats,
                    InsertedCount = inserted,
                    Duration = stopwatch.Elapsed,
                    TableSize = tableSize,
                    ElementCount = elementCount
                });
            }

            return results;
        }

        public List<Core.Models.BenchmarkResult> RunOpenAddressingBenchmark(
            IHashFunction<int>[] functions,
            ICollisionResolver[] resolvers,
            int tableSize,
            int elementCount)
        {
            var results = new List<Core.Models.BenchmarkResult>();
            var keys = DataGenerator.GenerateUniqueKeys(elementCount);

            Console.WriteLine($"\nГенерация {elementCount} уникальных ключей...");

            foreach (var function in functions)
            {
                foreach (var resolver in resolvers)
                {
                    Console.WriteLine($"\nТестирование: {function.Name} + {resolver.Name}");
                    Console.WriteLine(new string('-', 60));

                    try
                    {
                        var table = new OpenAddressingHashTable<int, string>(tableSize, function, resolver);
                        var stopwatch = Stopwatch.StartNew();
                        int inserted = 0;

                        foreach (int key in keys)
                        {
                            try
                            {
                                if (table.Insert(key, $"Value_{key}"))
                                    inserted++;
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Ошибка при вставке: {ex.Message}");
                                break;
                            }
                        }

                        stopwatch.Stop();
                        var stats = table.GetStatistics();

                        results.Add(new Core.Models.BenchmarkResult
                        {
                            TestName = $"{function.Name}\n{resolver.Name}",
                            Statistics = stats,
                            InsertedCount = inserted,
                            Duration = stopwatch.Elapsed,
                            TableSize = tableSize,
                            ElementCount = elementCount
                        });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
            }

            return results;
        }
    }
}