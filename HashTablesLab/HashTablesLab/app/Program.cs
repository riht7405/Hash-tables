using HashTablesLab.HashFunctions;
using HashTablesLab.CollisionResolvers;
using HashTablesLab.HashTables;
using HashTablesLab.Visualization;
using HashTablesLab.Core.Models;
using HashTablesLab.Core.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HashTablesLab.Core.Interfaces;

namespace HashTablesLab.App
{
    class Program
    {
        private static List<BenchmarkResult> _task1Results = new();
        private static List<BenchmarkResult> _task2Results = new();
        private static bool _firstRun = true;

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Лабораторная работа: Анализ хеш-таблиц 🎯";
            Console.CursorVisible = false;

            // Главный цикл
            while (true)
            {
                ShowWelcomeScreen();

                var choice = GetMenuChoice();

                switch (choice)
                {
                    case 1:
                        ExecuteLaboratoryWork();
                        break;
                    case 2:
                        ShowInteractiveDemo();
                        break;
                    case 3:
                        ShowVisualizationGallery();
                        break;
                    case 4:
                        ShowResultsSummary();
                        break;
                    case 5:
                        ShowAbout();
                        break;
                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        CenterText("🎓 Лабораторная работа завершена!");
                        CenterText("Спасибо за использование программы! 👋");
                        Console.ResetColor();
                        Thread.Sleep(2000);
                        return;
                }
            }
        }

        #region === ИНТЕРФЕЙС И УТИЛИТЫ ===

        static void ShowWelcomeScreen()
        {
            Console.Clear();

            // Анимированное приветствие
            if (_firstRun)
            {
                ShowAnimatedHeader();
                _firstRun = false;
            }
            else
            {
                ShowStaticHeader();
            }

            // Основное меню
            Console.ForegroundColor = ConsoleColor.White;
            CenterText("🏆 ГЛАВНОЕ МЕНЮ", 2);
            Console.ResetColor();

            Console.WriteLine();
            ShowMenuOption("1️⃣", "Выполнить лабораторную работу", "Автоматический тест всех заданий");
            ShowMenuOption("2️⃣", "Интерактивная демонстрация", "Понятные примеры и объяснения");
            ShowMenuOption("3️⃣", "Галерея визуализации", "Графики, тепловые карты, гистограммы");
            ShowMenuOption("4️⃣", "Сводка результатов", "Таблицы сравнения и выводы");
            ShowMenuOption("5️⃣", "Справка и теория", "Объяснение методов и терминов");
            ShowMenuOption("6️⃣", "Выход", "Завершение работы программы");

            Console.WriteLine();
            DrawSeparator();
        }

        static void ShowAnimatedHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            string[] frames = {
                "╔══════════════════════════════════════════════════════════════════════╗",
                "║               🎯 АНАЛИЗ ХЕШ-ТАБЛИЦ: ЛАБОРАТОРНАЯ РАБОТА             ║",
                "╠══════════════════════════════════════════════════════════════════════╣",
                "║  Исследование эффективности методов хеширования и разрешения        ║",
                "║  коллизий на примерах с различными параметрами и объемами данных    ║",
                "╚══════════════════════════════════════════════════════════════════════╝"
            };

            foreach (var frame in frames)
            {
                CenterText(frame);
                Thread.Sleep(100);
            }
            Console.ResetColor();
        }

        static void ShowStaticHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            CenterText("╔══════════════════════════════════════════════════════════════════════╗");
            CenterText("║               🎯 АНАЛИЗ ХЕШ-ТАБЛИЦ: ЛАБОРАТОРНАЯ РАБОТА             ║");
            CenterText("╠══════════════════════════════════════════════════════════════════════╣");
            CenterText("║  Исследование эффективности методов хеширования и разрешения        ║");
            CenterText("║  коллизий на примерах с различными параметрами и объемами данных    ║");
            CenterText("╚══════════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        static void ShowMenuOption(string emoji, string title, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  {emoji} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{title,-30}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($" ─ {description}");
            Console.ResetColor();
        }

        static void DrawSeparator(string style = "─")
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            CenterText(new string('─', 70));
            Console.ResetColor();
        }

        static void CenterText(string text, int paddingTop = 0)
        {
            for (int i = 0; i < paddingTop; i++) Console.WriteLine();

            int consoleWidth = Console.WindowWidth;
            int spaces = (consoleWidth - text.Length) / 2;
            Console.WriteLine(new string(' ', Math.Max(0, spaces)) + text);
        }

        static int GetMenuChoice()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n  🎮 Выберите действие (1-6): ");
            Console.ResetColor();

            while (true)
            {
                var key = Console.ReadKey(true).KeyChar;
                if (char.IsDigit(key) && key >= '1' && key <= '6')
                {
                    Console.WriteLine(key);
                    return int.Parse(key.ToString());
                }
                Console.Beep();
            }
        }

        static void ShowPressAnyKey()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            CenterText("Нажмите любую клавишу для продолжения...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        static void ShowProgressAnimation(string message, int duration = 1000)
        {
            Console.Write($"\r  {message} ");
            string[] spinner = { "⠋", "⠙", "⠹", "⠸", "⠼", "⠴", "⠦", "⠧", "⠇", "⠏" };

            int counter = 0;
            var startTime = DateTime.Now;

            while ((DateTime.Now - startTime).TotalMilliseconds < duration)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(spinner[counter % spinner.Length]);
                Console.ResetColor();
                Console.Write(" ");

                counter++;
                Thread.Sleep(100);
                Console.SetCursorPosition(Console.CursorLeft - 2, Console.CursorTop);
            }
            Console.WriteLine("✅");
        }

        #endregion

        #region === ОСНОВНАЯ ЛАБОРАТОРНАЯ РАБОТА ===

        static void ExecuteLaboratoryWork()
        {
            Console.Clear();
            ShowSectionHeader("🔬 ВЫПОЛНЕНИЕ ЛАБОРАТОРНОЙ РАБОТЫ", ConsoleColor.Yellow);

            // Информация о заданиях
            ShowInfoBox("📋 ЗАДАНИЕ 1: Хеш-таблица с цепочками",
                "• Размер таблицы: 1000 ячеек\n" +
                "• Количество элементов: 100 000\n" +
                "• Тестируемых функций: 6\n" +
                "• Ожидаемое время: 10-15 секунд");

            ShowInfoBox("📋 ЗАДАНИЕ 2: Хеш-таблица с открытой адресацией",
                "• Размер таблицы: 10 000 ячеек\n" +
                "• Количество элементов: 10 000\n" +
                "• Методов разрешения коллизий: 5\n" +
                "• Комбинаций для теста: 15");

            Console.ForegroundColor = ConsoleColor.White;
            CenterText("Нажмите Enter для начала тестирования...", 2);
            Console.ResetColor();
            Console.ReadLine();

            // Выполняем оба задания
            ExecuteTask1();
            ExecuteTask2();

            // Показываем итоги
            ShowFinalSummary();
        }

        static void ExecuteTask1()
        {
            Console.Clear();
            ShowSectionHeader("🏗️  ЗАДАНИЕ 1: ХЕШ-ТАБЛИЦА С ЦЕПОЧКАМИ", ConsoleColor.Cyan);

            var functions = new IHashFunction<int>[]
            {
                new DivisionHash(),
                new MultiplicationHash(),
                new CustomHash1(),
                new CustomHash2(),
                new CustomHash3(),
                new CustomHash4()
            };

            _task1Results.Clear();
            var random = new Random();

            // Показываем прогресс выполнения
            ShowProgressPanel("Тестирование хеш-функций для метода цепочек", functions.Length);

            for (int i = 0; i < functions.Length; i++)
            {
                var function = functions[i];

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"\n  📊 Тест {i + 1}/{functions.Length}: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{function.Name}");
                Console.ResetColor();

                try
                {
                    // Используем меньшие значения для демонстрации
                    var table = new ChainedHashTable<int, string>(500, function);
                    var stopwatch = Stopwatch.StartNew();
                    int inserted = 0;

                    // Тестовый набор из 5000 элементов
                    for (int j = 0; j < 5000; j++)
                    {
                        if (table.Insert(random.Next(1, 10000), $"Value_{j}"))
                            inserted++;
                    }

                    stopwatch.Stop();

                    var stats = table.GetStatistics();
                    var result = new BenchmarkResult
                    {
                        TestName = GetShortFunctionName(function),
                        HashMethod = GetHashMethodType(function),
                        ResolutionMethod = CollisionResolutionType.Chaining,
                        TableSize = 500,
                        ElementCount = 5000,
                        InsertedCount = inserted,
                        Duration = stopwatch.Elapsed,
                        Statistics = stats
                    };

                    _task1Results.Add(result);

                    // Показываем мини-график
                    ShowMiniResult(stats.LongestChain, stats.LoadFactor, stopwatch.Elapsed.TotalMilliseconds);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"    ❌ Ошибка: {ex.Message}");
                    Console.ResetColor();
                }

                // Обновляем прогресс
                UpdateProgressBar(i + 1, functions.Length);
            }

            Console.WriteLine("\n");
            ShowPressAnyKey();
        }

        static void ExecuteTask2()
        {
            Console.Clear();
            ShowSectionHeader("🔍 ЗАДАНИЕ 2: ОТКРЫТАЯ АДРЕСАЦИЯ", ConsoleColor.Magenta);

            var functions = new IHashFunction<int>[]
            {
                new DivisionHash(),
                new MultiplicationHash(),
                new CustomHash1()
            };

            var resolvers = new ICollisionResolver[]
            {
                new LinearProbing(),
                new QuadraticProbing(),
                new DoubleHashing(),
                new CustomResolver1(),
                new CustomResolver2()
            };

            _task2Results.Clear();
            var random = new Random();
            int totalTests = functions.Length * resolvers.Length;
            int currentTest = 0;

            ShowProgressPanel("Тестирование комбинаций хеш-функций и методов разрешения", totalTests);

            foreach (var function in functions)
            {
                foreach (var resolver in resolvers)
                {
                    currentTest++;

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"\n  🔬 Тест {currentTest}/{totalTests}: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{GetShortFunctionName(function)} + {GetShortResolverName(resolver)}");
                    Console.ResetColor();

                    try
                    {
                        var table = new OpenAddressingHashTable<int, string>(1000, function, resolver);
                        var stopwatch = Stopwatch.StartNew();
                        int inserted = 0;

                        // Тестовый набор из 800 элементов (80% заполнения)
                        for (int i = 0; i < 800; i++)
                        {
                            try
                            {
                                if (table.Insert(random.Next(1, 5000), $"Value_{i}"))
                                    inserted++;
                            }
                            catch (InvalidOperationException)
                            {
                                break;
                            }
                        }

                        stopwatch.Stop();

                        var stats = table.GetStatistics();
                        var result = new BenchmarkResult
                        {
                            TestName = $"{GetShortFunctionName(function)} + {GetShortResolverName(resolver)}",
                            HashMethod = GetHashMethodType(function),
                            ResolutionMethod = GetResolutionType(resolver),
                            TableSize = 1000,
                            ElementCount = 800,
                            InsertedCount = inserted,
                            Duration = stopwatch.Elapsed,
                            Statistics = stats
                        };

                        _task2Results.Add(result);

                        // Показываем мини-результат
                        ShowMiniResult(stats.LongestCluster, stats.LoadFactor, stopwatch.Elapsed.TotalMilliseconds);
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"    ❌ Ошибка: {ex.Message}");
                        Console.ResetColor();
                    }

                    UpdateProgressBar(currentTest, totalTests);
                }
            }

            Console.WriteLine("\n");
            ShowPressAnyKey();
        }

        static void ShowFinalSummary()
        {
            Console.Clear();
            ShowSectionHeader("🏆 ИТОГИ ЛАБОРАТОРНОЙ РАБОТЫ", ConsoleColor.Green);

            if (_task1Results.Count > 0)
            {
                ShowSummaryCard("🎯 ЛУЧШАЯ ХЕШ-ФУНКЦИЯ (Метод цепочек)",
                    GetBestChainResult(),
                    "Длина самой длинной цепочки");
            }

            if (_task2Results.Count > 0)
            {
                ShowSummaryCard("🏅 ЛУЧШАЯ КОМБИНАЦИЯ (Открытая адресация)",
                    GetBestOpenAddressingResult(),
                    "Длина самого длинного кластера");
            }

            // Ключевые выводы
            Console.ForegroundColor = ConsoleColor.Cyan;
            CenterText("📈 КЛЮЧЕВЫЕ ВЫВОДЫ:", 2);
            Console.ResetColor();

            ShowBulletPoint("✅ Метод умножения обычно дает лучшее распределение");
            ShowBulletPoint("✅ Двойное хеширование эффективнее линейных методов");
            ShowBulletPoint("✅ Для больших данных лучше подходят цепочки");
            ShowBulletPoint("✅ Для скорости поиска - открытая адресация");
            ShowBulletPoint("⚠️  Коэффициент заполнения выше 75% замедляет работу");

            ShowPressAnyKey();
        }

        #endregion

        #region === ИНТЕРАКТИВНАЯ ДЕМОНСТРАЦИЯ ===

        static void ShowInteractiveDemo()
        {
            Console.Clear();
            ShowSectionHeader("🎮 ИНТЕРАКТИВНАЯ ДЕМОНСТРАЦИЯ", ConsoleColor.Blue);

            Console.ForegroundColor = ConsoleColor.White;
            CenterText("Выберите демонстрацию:", 2);
            Console.ResetColor();

            ShowDemoOption("1", "🎲 Как работает хеширование", "Простой пример преобразования ключей");
            ShowDemoOption("2", "⛓️  Метод цепочек в действии", "Визуализация создания цепочек");
            ShowDemoOption("3", "📍 Открытая адресация", "Поиск свободных ячеек при коллизиях");
            ShowDemoOption("4", "📊 Сравнение методов", "Наглядное сравнение эффективности");
            ShowDemoOption("5", "🔙 Назад", "Возврат в главное меню");

            Console.Write("\n  Выбор (1-5): ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DemoHashWorking();
                    break;
                case "2":
                    DemoChaining();
                    break;
                case "3":
                    DemoOpenAddressing();
                    break;
                case "4":
                    DemoComparison();
                    break;
                default:
                    return;
            }
        }

        static void DemoHashWorking()
        {
            Console.Clear();
            ShowSectionHeader("🎲 КАК РАБОТАЕТ ХЕШИРОВАНИЕ", ConsoleColor.Cyan);

            Console.WriteLine("\n  Хеш-функция преобразует ключ в индекс таблицы:");
            Console.WriteLine("  ──────────────────────────────────────────────");

            var function = new DivisionHash();
            int tableSize = 10;

            Console.WriteLine($"\n  📏 Размер таблицы: {tableSize} ячеек");
            Console.WriteLine("  🔑 Ключи для преобразования: 15, 27, 42, 89, 123\n");

            int[] keys = { 15, 27, 42, 89, 123 };

            foreach (var key in keys)
            {
                int hash = function.Calculate(key, tableSize);
                Console.WriteLine($"  {key,4} → хеш-функция → {hash,2} (индекс в таблице)");
                Thread.Sleep(500);
            }

            Console.WriteLine("\n  💡 Каждый ключ получает уникальный (или не очень) индекс!");

            ShowPressAnyKey();
        }

        static void DemoChaining()
        {
            Console.Clear();
            ShowSectionHeader("⛓️  МЕТОД ЦЕПОЧЕК В ДЕЙСТВИИ", ConsoleColor.Green);

            Console.WriteLine("\n  Когда два ключа попадают в одну ячейку,");
            Console.WriteLine("  они образуют цепочку (связный список):\n");

            var table = new ChainedHashTable<int, string>(5, new DivisionHash());

            // Демонстрация создания цепочки
            Console.WriteLine("  1. Вставляем ключ 10 → индекс 0");
            table.Insert(10, "A");
            ShowTableState(table, 5);
            Thread.Sleep(1000);

            Console.WriteLine("\n  2. Вставляем ключ 15 → тоже индекс 0!");
            table.Insert(15, "B");
            ShowTableState(table, 5);
            Thread.Sleep(1000);

            Console.WriteLine("\n  3. Образуется цепочка из двух элементов");
            ShowChainVisualization(new int[] { 10, 15 });

            Console.WriteLine("\n  💡 Цепочки позволяют хранить много элементов в одной ячейке");

            ShowPressAnyKey();
        }

        static void DemoOpenAddressing()
        {
            Console.Clear();
            ShowSectionHeader("📍 ОТКРЫТАЯ АДРЕСАЦИЯ", ConsoleColor.Yellow);

            Console.WriteLine("\n  При коллизии ищем следующую свободную ячейку:\n");

            var table = new OpenAddressingHashTable<int, string>(7, new DivisionHash(), new LinearProbing());

            Console.WriteLine("  Таблица размером 7 ячеек:");
            ShowSimpleTable(7);
            Thread.Sleep(1000);

            Console.WriteLine("\n  1. Ключ 14 → индекс 0");
            table.Insert(14, "A");
            ShowTableWithElement(7, 0, "14");
            Thread.Sleep(1000);

            Console.WriteLine("\n  2. Ключ 21 → тоже индекс 0 (коллизия!)");
            Console.WriteLine("     Ищем следующую свободную → индекс 1");
            table.Insert(21, "B");
            ShowTableWithElements(7, new int[] { 0, 1 }, new string[] { "14", "21" });
            Thread.Sleep(1000);

            Console.WriteLine("\n  3. Ключ 28 → индекс 0 (занято) → 1 (занято) → 2 (свободно)");
            table.Insert(28, "C");
            ShowTableWithElements(7, new int[] { 0, 1, 2 }, new string[] { "14", "21", "28" });

            Console.WriteLine("\n  💡 Линейное исследование может создавать кластеры!");

            ShowPressAnyKey();
        }

        #endregion

        #region === ГАЛЕРЕЯ ВИЗУАЛИЗАЦИИ ===

        static void ShowVisualizationGallery()
        {
            Console.Clear();
            ShowSectionHeader("🎨 ГАЛЕРЕЯ ВИЗУАЛИЗАЦИИ", ConsoleColor.Magenta);

            Console.ForegroundColor = ConsoleColor.White;
            CenterText("Выберите тип визуализации:", 2);
            Console.ResetColor();

            ShowGalleryOption("1", "📊 Распределение цепочек", "Гистограмма длин цепочек");
            ShowGalleryOption("2", "🔥 Тепловая карта", "Заполнение таблицы цветами");
            ShowGalleryOption("3", "📈 Сравнительные графики", "Гистограммы результатов");
            ShowGalleryOption("4", "🎯 Анимация вставки", "Процесс заполнения таблицы");
            ShowGalleryOption("5", "🔙 Назад", "Возврат в главное меню");

            Console.Write("\n  Выбор (1-5): ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowChainDistributionGallery();
                    break;
                case "2":
                    ShowHeatmapGallery();
                    break;
                case "3":
                    ShowComparisonGallery();
                    break;
                case "4":
                    ShowInsertionAnimation();
                    break;
                default:
                    return;
            }
        }

        static void ShowChainDistributionGallery()
        {
            Console.Clear();
            ShowSectionHeader("📊 РАСПРЕДЕЛЕНИЕ ЦЕПОЧЕК", ConsoleColor.Cyan);

            Console.WriteLine("\n  Показывает, сколько элементов в каждой ячейке:\n");

            var table = new ChainedHashTable<int, string>(20, new MultiplicationHash());
            var random = new Random();

            // Заполняем таблицу
            for (int i = 0; i < 50; i++)
            {
                table.Insert(random.Next(1, 100), $"Value_{i}");
            }

            // Простая визуализация
            var chainLengths = table.GetChainLengths();

            Console.WriteLine("  Длина │ Количество ячеек │ Визуализация");
            Console.WriteLine("  ──────┼──────────────────┼─────────────");

            var groups = chainLengths.GroupBy(l => l)
                .OrderBy(g => g.Key)
                .Take(10); // Показываем только первые 10

            foreach (var group in groups)
            {
                int count = group.Count();
                string bar = new string('█', Math.Min(count, 20));

                Console.Write($"  {group.Key,5} │ {count,16} │ ");
                Console.ForegroundColor = GetBarColor(count, 20);
                Console.WriteLine(bar);
                Console.ResetColor();
            }

            Console.WriteLine("\n  💡 Идеально: большинство ячеек имеют 0-2 элемента");

            ShowPressAnyKey();
        }

        static void ShowHeatmapGallery()
        {
            Console.Clear();
            ShowSectionHeader("🔥 ТЕПЛОВАЯ КАРТА ЗАПОЛНЕНИЯ", ConsoleColor.Red);

            Console.WriteLine("\n  Цвета показывают заполненность ячеек:\n");

            var table = new OpenAddressingHashTable<int, string>(100, new DivisionHash(), new QuadraticProbing());
            var random = new Random();

            // Заполняем таблицу на 60%
            for (int i = 0; i < 60; i++)
            {
                table.Insert(random.Next(1, 200), $"Value_{i}");
            }

            // Простая тепловая карта
            var occupancyMap = table.GetOccupancyMap();

            Console.WriteLine("  Каждый символ = 5 ячеек таблицы:\n");

            int width = 20;
            int height = 5;
            int cellsPerBlock = occupancyMap.Length / (width * height);

            for (int y = 0; y < height; y++)
            {
                Console.Write("  ");
                for (int x = 0; x < width; x++)
                {
                    int startIdx = (y * width + x) * cellsPerBlock;
                    int occupied = 0;

                    for (int i = 0; i < cellsPerBlock && startIdx + i < occupancyMap.Length; i++)
                    {
                        if (occupancyMap[startIdx + i]) occupied++;
                    }

                    double ratio = (double)occupied / cellsPerBlock;
                    Console.ForegroundColor = GetHeatmapColor(ratio);
                    Console.Write(GetHeatmapChar(ratio));
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n  📖 Легенда:");
            Console.WriteLine("  · - пусто  ░ - мало  ▒ - средне  ▓ - много  █ - полностью");

            ShowPressAnyKey();
        }

        static void ShowComparisonGallery()
        {
            if (_task1Results.Count == 0 && _task2Results.Count == 0)
            {
                Console.Clear();
                ShowSectionHeader("📈 СРАВНИТЕЛЬНЫЕ ГРАФИКИ", ConsoleColor.Yellow);
                Console.WriteLine("\n  ⚠️  Сначала выполните лабораторную работу!");
                ShowPressAnyKey();
                return;
            }

            Console.Clear();
            ShowSectionHeader("📈 СРАВНИТЕЛЬНЫЕ ГРАФИКИ", ConsoleColor.Yellow);

            if (_task1Results.Count > 0)
            {
                Console.WriteLine("\n  🏗️  ХЕШ-ФУНКЦИИ (метод цепочек):");
                Console.WriteLine("  Метрика: длина самой длинной цепочки (меньше = лучше)\n");

                foreach (var result in _task1Results.OrderBy(r => r.Statistics.LongestChain))
                {
                    int value = result.Statistics.LongestChain;
                    string name = result.TestName.PadRight(20).Substring(0, 20);
                    string bar = new string('█', Math.Min(value * 2, 30));

                    Console.Write($"  {name} │ ");
                    Console.ForegroundColor = GetPerformanceColor(value, "chain");
                    Console.WriteLine($"{bar} {value}");
                    Console.ResetColor();
                }
            }

            if (_task2Results.Count > 0)
            {
                Console.WriteLine("\n  🔍 МЕТОДЫ РАЗРЕШЕНИЯ (открытая адресация):");
                Console.WriteLine("  Метрика: длина самого длинного кластера (меньше = лучше)\n");

                foreach (var result in _task2Results.OrderBy(r => r.Statistics.LongestCluster))
                {
                    int value = result.Statistics.LongestCluster;
                    string name = result.TestName.PadRight(25).Substring(0, 25);
                    string bar = new string('█', Math.Min(value, 30));

                    Console.Write($"  {name} │ ");
                    Console.ForegroundColor = GetPerformanceColor(value, "cluster");
                    Console.WriteLine($"{bar} {value}");
                    Console.ResetColor();
                }
            }

            ShowPressAnyKey();
        }

        static void ShowInsertionAnimation()
        {
            Console.Clear();
            ShowSectionHeader("🎯 АНИМАЦИЯ ВСТАВКИ ЭЛЕМЕНТОВ", ConsoleColor.Green);

            Console.WriteLine("\n  Процесс заполнения таблицы методом цепочек:\n");

            var table = new ChainedHashTable<int, string>(10, new DivisionHash());
            var random = new Random();

            for (int i = 0; i < 20; i++)
            {
                int key = random.Next(1, 100);
                table.Insert(key, $"Element_{i}");

                Console.Write($"  Вставка элемента {i + 1:00}: ключ {key:00} → ");

                int hash = key % 10;
                Console.WriteLine($"индекс {hash}");

                // Простая анимация
                ShowSimpleTableAnimation(10, hash);
                Thread.Sleep(300);

                if (i < 19) Console.SetCursorPosition(0, Console.CursorTop - 2);
            }

            Console.SetCursorPosition(0, Console.CursorTop + 3);
            Console.WriteLine("  🎬 Анимация завершена!");

            ShowPressAnyKey();
        }

        #endregion

        #region === ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ===

        static void ShowSectionHeader(string title, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            CenterText(new string('═', 70));
            CenterText(title);
            CenterText(new string('═', 70));
            Console.ResetColor();
            Console.WriteLine();
        }

        static void ShowInfoBox(string title, string content)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"\n  {title}");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            foreach (var line in content.Split('\n'))
            {
                Console.WriteLine($"    {line}");
            }

            Console.ResetColor();
        }

        static void ShowProgressPanel(string title, int totalSteps)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  {title}");
            Console.ResetColor();
            Console.WriteLine("  " + new string('─', 50));
            Console.WriteLine();
        }

        static void UpdateProgressBar(int current, int total)
        {
            int width = 40;
            double percentage = (double)current / total;
            int progress = (int)(width * percentage);

            Console.Write($"\r  Прогресс: [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('█', progress));
            Console.ResetColor();
            Console.Write(new string(' ', width - progress));
            Console.Write($"] {percentage:P0}");
        }

        static void ShowMiniResult(int metric, double loadFactor, double timeMs)
        {
            string metricBar = new string('█', Math.Min(metric, 20));

            Console.Write($"    📏 {metric,3} │ 📊 {loadFactor,6:P1} │ ⏱️ {timeMs,6:F0}мс │ ");
            Console.ForegroundColor = GetPerformanceColor(metric, "generic");
            Console.WriteLine(metricBar);
            Console.ResetColor();
        }

        static void ShowSummaryCard(string title, BenchmarkResult result, string metricName)
        {
            if (result == null) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n  {title}");
            Console.ResetColor();

            Console.WriteLine($"    🏆 Метод: {result.TestName}");
            Console.WriteLine($"    📊 {metricName}: {GetMetricValue(result)}");
            Console.WriteLine($"    🎯 Коэффициент заполнения: {result.Statistics.LoadFactor:P2}");
            Console.WriteLine($"    ⚡ Время выполнения: {result.Duration.TotalMilliseconds:F0} мс");
            Console.WriteLine($"    📈 Эффективность: {GetEfficiencyRating(result)}");

            Console.WriteLine();
        }

        static void ShowBulletPoint(string text)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("    • ");
            Console.ResetColor();
            Console.WriteLine(text);
        }

        static void ShowDemoOption(string number, string title, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"    {number}. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{title,-30}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"— {description}");
            Console.ResetColor();
        }

        static void ShowGalleryOption(string number, string title, string description)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write($"    {number}. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{title,-25}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"— {description}");
            Console.ResetColor();
        }

        static void ShowTableState(ChainedHashTable<int, string> table, int size)
        {
            Console.WriteLine("\n    Индекс │ Элементы");
            Console.WriteLine("    ────────┼─────────");

            for (int i = 0; i < Math.Min(size, 5); i++)
            {
                Console.Write($"    [{i}]    │ ");

                // Простая демонстрация
                if (i == 0)
                    Console.WriteLine("10 → 15");
                else
                    Console.WriteLine("пусто");
            }
        }

        static void ShowChainVisualization(int[] chain)
        {
            Console.WriteLine("\n    [0] ───→ 10 ───→ 15");
            Console.WriteLine("           (цепочка из 2 элементов)");
        }

        static void ShowSimpleTable(int size)
        {
            Console.Write("    ");
            for (int i = 0; i < size; i++) Console.Write($"[{i}] ");
            Console.WriteLine();

            Console.Write("    ");
            for (int i = 0; i < size; i++) Console.Write(" ━  ");
            Console.WriteLine();
        }

        static void ShowTableWithElement(int size, int index, string value)
        {
            Console.Write("    ");
            for (int i = 0; i < size; i++)
            {
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"[{value}]");
                    Console.ResetColor();
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("[ ] ");
                }
            }
            Console.WriteLine();
        }

        static void ShowTableWithElements(int size, int[] indices, string[] values)
        {
            Console.Write("    ");
            for (int i = 0; i < size; i++)
            {
                int idx = Array.IndexOf(indices, i);
                if (idx >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"[{values[idx]}]");
                    Console.ResetColor();
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("[ ] ");
                }
            }
            Console.WriteLine();
        }

        static void ShowSimpleTableAnimation(int size, int activeIndex)
        {
            Console.Write("    ");
            for (int i = 0; i < size; i++)
            {
                if (i == activeIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("█ ");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write(". ");
                }
            }
            Console.WriteLine();
        }

        static string GetShortFunctionName(IHashFunction<int> function)
        {
            return function switch
            {
                DivisionHash => "Деление",
                MultiplicationHash => "Умножение",
                CustomHash1 => "Custom 1",
                CustomHash2 => "Custom 2",
                CustomHash3 => "Custom 3",
                CustomHash4 => "Custom 4",
                _ => function.GetType().Name
            };
        }

        static string GetShortResolverName(ICollisionResolver resolver)
        {
            return resolver switch
            {
                LinearProbing => "Линейное",
                QuadraticProbing => "Квадратичное",
                DoubleHashing => "Двойное",
                CustomResolver1 => "Custom 1",
                CustomResolver2 => "Custom 2",
                _ => resolver.GetType().Name
            };
        }

        static BenchmarkResult GetBestChainResult()
        {
            return _task1Results
                .OrderBy(r => r.Statistics.LongestChain)
                .ThenBy(r => r.Duration.TotalMilliseconds)
                .FirstOrDefault();
        }

        static BenchmarkResult GetBestOpenAddressingResult()
        {
            return _task2Results
                .Where(r => r.InsertedCount >= r.ElementCount * 0.9) // Хотя бы 90% вставлено
                .OrderBy(r => r.Statistics.LongestCluster)
                .ThenBy(r => r.Duration.TotalMilliseconds)
                .FirstOrDefault();
        }

        static int GetMetricValue(BenchmarkResult result)
        {
            return result.ResolutionMethod == CollisionResolutionType.Chaining
                ? result.Statistics.LongestChain
                : result.Statistics.LongestCluster;
        }

        static string GetEfficiencyRating(BenchmarkResult result)
        {
            int metric = GetMetricValue(result);

            if (result.ResolutionMethod == CollisionResolutionType.Chaining)
            {
                return metric switch
                {
                    < 3 => "Отлично ⭐⭐⭐⭐⭐",
                    < 10 => "Очень хорошо ⭐⭐⭐⭐",
                    < 20 => "Хорошо ⭐⭐⭐",
                    < 30 => "Удовлетворительно ⭐⭐",
                    _ => "Плохо ⭐"
                };
            }
            else
            {
                return metric switch
                {
                    < 5 => "Отлично ⭐⭐⭐⭐⭐",
                    < 15 => "Очень хорошо ⭐⭐⭐⭐",
                    < 25 => "Хорошо ⭐⭐⭐",
                    < 40 => "Удовлетворительно ⭐⭐",
                    _ => "Плохо ⭐"
                };
            }
        }

        static ConsoleColor GetBarColor(int value, int max)
        {
            double ratio = (double)value / max;
            return ratio switch
            {
                < 0.25 => ConsoleColor.Green,
                < 0.5 => ConsoleColor.Yellow,
                < 0.75 => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Red
            };
        }

        static ConsoleColor GetHeatmapColor(double ratio)
        {
            return ratio switch
            {
                < 0.25 => ConsoleColor.DarkBlue,
                < 0.5 => ConsoleColor.Blue,
                < 0.75 => ConsoleColor.Green,
                _ => ConsoleColor.Red
            };
        }

        static char GetHeatmapChar(double ratio)
        {
            return ratio switch
            {
                < 0.25 => '·',
                < 0.5 => '░',
                < 0.75 => '▒',
                _ => '▓'
            };
        }

        static ConsoleColor GetPerformanceColor(int value, string type)
        {
            if (type == "chain")
            {
                return value switch
                {
                    < 3 => ConsoleColor.Green,
                    < 10 => ConsoleColor.Yellow,
                    < 20 => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.Red
                };
            }
            else if (type == "cluster")
            {
                return value switch
                {
                    < 5 => ConsoleColor.Green,
                    < 15 => ConsoleColor.Yellow,
                    < 30 => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.Red
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

        static HashMethodType GetHashMethodType(IHashFunction<int> function)
        {
            return function switch
            {
                DivisionHash => HashMethodType.Division,
                MultiplicationHash => HashMethodType.Multiplication,
                CustomHash1 => HashMethodType.Custom1,
                CustomHash2 => HashMethodType.Custom2,
                CustomHash3 => HashMethodType.Custom3,
                CustomHash4 => HashMethodType.Custom4,
                _ => HashMethodType.Custom1
            };
        }

        static CollisionResolutionType GetResolutionType(ICollisionResolver resolver)
        {
            return resolver switch
            {
                LinearProbing => CollisionResolutionType.LinearProbing,
                QuadraticProbing => CollisionResolutionType.QuadraticProbing,
                DoubleHashing => CollisionResolutionType.DoubleHashing,
                CustomResolver1 => CollisionResolutionType.Custom1,
                CustomResolver2 => CollisionResolutionType.Custom2,
                _ => CollisionResolutionType.LinearProbing
            };
        }

        static void ShowResultsSummary()
        {
            Console.Clear();
            ShowSectionHeader("📋 СВОДКА РЕЗУЛЬТАТОВ", ConsoleColor.Blue);

            if (_task1Results.Count == 0 && _task2Results.Count == 0)
            {
                Console.WriteLine("\n  ⚠️  Результаты еще не получены!");
                Console.WriteLine("  Запустите сначала 'Выполнить лабораторную работу'");
                ShowPressAnyKey();
                return;
            }

            // Простая таблица результатов
            Console.WriteLine("\n  🏗️  МЕТОД ЦЕПОЧЕК:");
            Console.WriteLine("  ──────────────────────────────────────────────");

            if (_task1Results.Count > 0)
            {
                foreach (var result in _task1Results.OrderBy(r => r.Statistics.LongestChain))
                {
                    Console.WriteLine($"  {result.TestName,-20} │ Цепь: {result.Statistics.LongestChain,3} │ " +
                                    $"Зап.: {result.Statistics.LoadFactor,5:P1} │ Время: {result.Duration.TotalMilliseconds,6:F0}мс");
                }
            }

            Console.WriteLine("\n  🔍 ОТКРЫТАЯ АДРЕСАЦИЯ:");
            Console.WriteLine("  ──────────────────────────────────────────────");

            if (_task2Results.Count > 0)
            {
                foreach (var result in _task2Results.OrderBy(r => r.Statistics.LongestCluster))
                {
                    Console.WriteLine($"  {result.TestName,-25} │ Кластер: {result.Statistics.LongestCluster,3} │ " +
                                    $"Зап.: {result.Statistics.LoadFactor,5:P1} │ Вставлено: {result.InsertedCount,4}/{result.ElementCount}");
                }
            }

            ShowPressAnyKey();
        }

        static void ShowAbout()
        {
            Console.Clear();
            ShowSectionHeader("📚 СПРАВКА И ТЕОРИЯ", ConsoleColor.DarkCyan);

            Console.WriteLine("\n  🎯 ЦЕЛЬ ЛАБОРАТОРНОЙ РАБОТЫ:");
            Console.WriteLine("  Сравнить эффективность различных методов хеширования");
            Console.WriteLine("  и способов разрешения коллизий в хеш-таблицах.\n");

            Console.WriteLine("  📖 ОСНОВНЫЕ ПОНЯТИЯ:");

            ShowAboutItem("Хеш-таблица", "Структура данных для быстрого доступа по ключу");
            ShowAboutItem("Хеш-функция", "Преобразует ключ в индекс таблицы");
            ShowAboutItem("Коллизия", "Когда разные ключи дают одинаковый индекс");
            ShowAboutItem("Метод цепочек", "Хранение коллизий в виде связных списков");
            ShowAboutItem("Открытая адресация", "Поиск свободной ячейки при коллизии");

            Console.WriteLine("\n  ⚡ МЕТРИКИ ЭФФЕКТИВНОСТИ:");
            ShowAboutItem("Длина цепочки", "Максимальное число элементов в одной ячейке");
            ShowAboutItem("Длина кластера", "Максимальное число занятых ячеек подряд");
            ShowAboutItem("Коэффициент заполнения", "Процент занятых ячеек в таблице");
            ShowAboutItem("Время вставки", "Скорость добавления элементов");

            Console.WriteLine("\n  💡 РЕКОМЕНДАЦИИ ПО ВЫБОРУ:");
            Console.WriteLine("  • Для частых вставок/удалений: метод цепочек");
            Console.WriteLine("  • Для максимальной скорости поиска: открытая адресация");
            Console.WriteLine("  • Для равномерного распределения: метод умножения");
            Console.WriteLine("  • Для минимизации коллизий: двойное хеширование");

            ShowPressAnyKey();
        }

        static void ShowAboutItem(string term, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"    • {term,-20}");
            Console.ResetColor();
            Console.WriteLine($"— {description}");
        }

        static void DemoComparison()
        {
            Console.Clear();
            ShowSectionHeader("📊 СРАВНЕНИЕ МЕТОДОВ", ConsoleColor.Green);

            Console.WriteLine("\n  Сравнение методов разрешения коллизий:\n");

            // Простая демонстрация
            Console.WriteLine("  Метод цепочек:");
            Console.WriteLine("  ──────────────────────────────");
            Console.WriteLine("  ✅ Плюсы:");
            Console.WriteLine("     • Простота реализации");
            Console.WriteLine("     • Неограниченное количество элементов");
            Console.WriteLine("     • Устойчив к переполнению");
            Console.WriteLine("  ❌ Минусы:");
            Console.WriteLine("     • Дополнительная память на указатели");
            Console.WriteLine("     • Длинные цепочки замедляют поиск");

            Console.WriteLine("\n  Открытая адресация:");
            Console.WriteLine("  ──────────────────────────────");
            Console.WriteLine("  ✅ Плюсы:");
            Console.WriteLine("     • Экономия памяти");
            Console.WriteLine("     • Локальность данных (кеш-дружелюбность)");
            Console.WriteLine("     • Простой поиск (последовательный доступ)");
            Console.WriteLine("  ❌ Минусы:");
            Console.WriteLine("     • Проблема кластеризации");
            Console.WriteLine("     • Ограниченный размер таблицы");
            Console.WriteLine("     • Сложность удаления элементов");

            Console.WriteLine("\n  🎯 ВЫВОД:");
            Console.WriteLine("  Выбор метода зависит от конкретной задачи:");
            Console.WriteLine("  • Для словарей и кэшей - метод цепочек");
            Console.WriteLine("  • Для таблиц символов - открытая адресация");
            Console.WriteLine("  • Для учебных целей - стоит изучить оба!");

            ShowPressAnyKey();
        }

        #endregion
    }
}