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
                        ShowVisualizationMenu();
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
            ShowMenuOption("3️⃣", "Галерея визуализации", "Интерактивные демонстрации и отчеты");
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

        #region === УЛУЧШЕННАЯ ЛАБОРАТОРНАЯ РАБОТА ===

        static void ExecuteLaboratoryWork()
        {
            Console.Clear();
            ShowLaboratoryWorkHeader();

            // Пошаговый мастер выполнения
            if (ShowLaboratoryWorkWizard())
            {
                // Если пользователь подтвердил выполнение
                ExecuteFullLaboratoryWork();
            }
        }

        static void ShowLaboratoryWorkHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            string[] header = {
        "╔══════════════════════════════════════════════════════════════════════╗",
        "║                    🔬 ЛАБОРАТОРНАЯ РАБОТА                          ║",
        "╠══════════════════════════════════════════════════════════════════════╣",
        "║  Полный анализ эффективности хеш-таблиц с разными методами         ║",
        "║  и параметрами. Автоматическое тестирование и визуализация.        ║",
        "╚══════════════════════════════════════════════════════════════════════╝"
    };

            foreach (var line in header)
            {
                CenterText(line);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        static bool ShowLaboratoryWorkWizard()
        {
            Console.ForegroundColor = ConsoleColor.White;
            CenterText("🎯 МАСТЕР ВЫПОЛНЕНИЯ ЛАБОРАТОРНОЙ РАБОТЫ", 2);
            Console.ResetColor();

            // Шаг 1: Показываем что будет выполнено
            ShowWizardStep("Шаг 1", "Обзор заданий", ConsoleColor.Blue);

            ShowTaskCard("📋 ЗАДАНИЕ 1: Хеш-таблица с цепочками",
                "Тестирование 6 хеш-функций",
                "• Таблица: 1000 ячеек\n" +
                "• Элементов: 100 000\n" +
                "• Метод: цепочки\n" +
                "• Время: ~15 секунд",
                "🏗️");

            ShowTaskCard("📋 ЗАДАНИЕ 2: Открытая адресация",
                "Тестирование 15 комбинаций",
                "• Таблица: 10 000 ячеек\n" +
                "• Элементов: 10 000\n" +
                "• Методы: 5 видов\n" +
                "• Время: ~20 секунд",
                "🔍");

            Console.WriteLine();
            ShowWizardStep("Шаг 2", "Настройка параметров", ConsoleColor.Green);

            // Настраиваемые параметры
            var config = ShowConfigurationPanel();

            Console.WriteLine();
            ShowWizardStep("Шаг 3", "Подтверждение запуска", ConsoleColor.Yellow);

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n  📊 БУДЕТ ВЫПОЛНЕНО:");
            Console.ResetColor();

            Console.WriteLine($"  • Тестов: {config.TotalTests}");
            Console.WriteLine($"  • Всего элементов: {config.TotalElements:N0}");
            Console.WriteLine($"  • Ожидаемое время: {config.EstimatedTime} секунд");
            Console.WriteLine($"  • Будет сохранено: {(config.SaveResults ? "Да" : "Нет")}");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n  🚀 Запустить выполнение? (Да/Нет): ");
            Console.ResetColor();

            return Console.ReadLine()?.ToLower().StartsWith("д") ?? false;
        }

        static (int TotalTests, int TotalElements, string EstimatedTime, bool SaveResults) ShowConfigurationPanel()
        {
            Console.WriteLine("\n  ⚙️  НАСТРОЙКИ ВЫПОЛНЕНИЯ:");

            // Выбор масштаба тестирования
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  📈 Масштаб тестирования:");
            Console.ResetColor();

            ShowConfigOption("1", "Полный (рекомендуется)", "Точные результаты, больше времени");
            ShowConfigOption("2", "Быстрый", "Быстрые результаты, меньше точности");
            ShowConfigOption("3", "Демонстрационный", "Только для ознакомления");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  Выбор (1-3): ");
            Console.ResetColor();

            var scaleChoice = Console.ReadLine();
            var scale = GetTestScale(scaleChoice);

            // Дополнительные опции
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  🎛️ Дополнительные опции:");
            Console.ResetColor();

            Console.Write("  💾 Сохранить результаты в файл? (Да/Нет): ");
            bool saveResults = Console.ReadLine()?.ToLower().StartsWith("д") ?? false;

            Console.Write("  📊 Показывать промежуточные графики? (Да/Нет): ");
            bool showCharts = Console.ReadLine()?.ToLower().StartsWith("д") ?? false;

            return (scale.TotalTests, scale.TotalElements, scale.EstimatedTime, saveResults);
        }

        static void ExecuteFullLaboratoryWork()
        {
            // Этап 1: Подготовка
            Console.Clear();
            ShowExecutionStage("🔧 ПОДГОТОВКА", ConsoleColor.Blue);

            ShowProgressAnimation("Инициализация компонентов", 800);
            ShowProgressAnimation("Подготовка тестовых данных", 600);
            ShowProgressAnimation("Настройка окружения", 400);

            // Этап 2: Задание 1
            Console.Clear();
            ShowExecutionStage("🏗️  ВЫПОЛНЕНИЕ ЗАДАНИЯ 1", ConsoleColor.Cyan);

            ExecuteTask1WithProgress();

            // Этап 3: Задание 2  
            Console.Clear();
            ShowExecutionStage("🔍 ВЫПОЛНЕНИЕ ЗАДАНИЯ 2", ConsoleColor.Magenta);

            ExecuteTask2WithProgress();

            // Этап 4: Анализ результатов
            Console.Clear();
            ShowExecutionStage("📊 АНАЛИЗ РЕЗУЛЬТАТОВ", ConsoleColor.Green);

            AnalyzeAndShowResults();
        }

        static void ExecuteTask1WithProgress()
        {
            Console.WriteLine("\n  🎯 ЦЕЛЬ: Сравнить 6 хеш-функций для метода цепочек\n");
            Console.WriteLine("  📊 ПАРАМЕТРЫ ПО ТЗ:");
            Console.WriteLine("  • Размер таблицы: 1000 ячеек");
            Console.WriteLine("  • Количество элементов: 100000");
            Console.WriteLine("  • Метод разрешения коллизий: цепочки\n");

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

            // Показываем красивую панель прогресса
            ShowEnhancedProgressBar("Тестирование хеш-функций", functions.Length);

            for (int i = 0; i < functions.Length; i++)
            {
                var function = functions[i];

                // Показываем текущий тест
                ShowCurrentTestInfo(i + 1, functions.Length, function.Name);

                try
                {
                    // ПАРАМЕТРЫ ПО ТЗ: m=1000, n=100000
                    int tableSize = 1000;      // m = 1000
                    int elementCount = 100000; // n = 100000

                    var table = new ChainedHashTable<int, string>(tableSize, function);
                    var stopwatch = Stopwatch.StartNew();
                    int inserted = 0;

                    // Генерируем 100000 УНИКАЛЬНЫХ ключей
                    var uniqueKeys = GenerateUniqueKeys(elementCount, random);

                    for (int j = 0; j < elementCount; j++)
                    {
                        if (table.Insert(uniqueKeys[j], $"Value_{j}"))
                            inserted++;

                        // Показываем прогресс каждые 10000 элементов
                        if (j % 10000 == 0 && j > 0)
                        {
                            ShowMiniProgress(j, elementCount);
                        }
                    }

                    stopwatch.Stop();

                    var stats = table.GetStatistics();
                    var result = new BenchmarkResult
                    {
                        TestName = GetShortFunctionName(function),
                        HashMethod = GetHashMethodType(function),
                        ResolutionMethod = CollisionResolutionType.Chaining,
                        TableSize = tableSize,
                        ElementCount = elementCount,
                        InsertedCount = inserted,
                        Duration = stopwatch.Elapsed,
                        Statistics = stats
                    };

                    _task1Results.Add(result);

                    // Показываем результат
                    ShowTestResultCard(stats.LongestChain, stats.LoadFactor,
                        stopwatch.Elapsed.TotalMilliseconds, GetShortFunctionName(function));
                }
                catch (Exception ex)
                {
                    ShowErrorCard($"Ошибка в тесте {function.Name}", ex.Message);
                }

                UpdateEnhancedProgressBar(i + 1, functions.Length);

                if (i < functions.Length - 1)
                {
                    Thread.Sleep(300);
                }
            }

            Console.WriteLine("\n  ✅ Задание 1 завершено!");
            ShowPressAnyKey();
        }

        static void ExecuteTask2WithProgress()
        {
            Console.Clear();
            ShowExecutionStage("🔍 ЗАДАНИЕ 2: ОТКРЫТАЯ АДРЕСАЦИЯ", ConsoleColor.Magenta);

            Console.WriteLine("\n  🎯 ЦЕЛЬ: Сравнить 5 методов разрешения коллизий\n");
            Console.WriteLine("  📊 ПАРАМЕТРЫ ПО ТЗ:");
            Console.WriteLine("  • Размер таблицы: 10000 ячеек");
            Console.WriteLine("  • Количество элементов: 10000");
            Console.WriteLine("  • Хеш-функция: Метод деления (единая для всех тестов)");
            Console.WriteLine("  • Тестируемые методы: 5 (3 базовых + 2 собственных)\n");

            // ТОЛЬКО ОДНА хеш-функция согласно ТЗ
            IHashFunction<int> hashFunction = new DivisionHash();

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

            ShowEnhancedProgressBar("Тестирование методов разрешения коллизий", resolvers.Length);

            for (int i = 0; i < resolvers.Length; i++)
            {
                var resolver = resolvers[i];

                ShowCurrentTestInfo(i + 1, resolvers.Length, resolver.Name);

                try
                {
                    // ПАРАМЕТРЫ ПО ТЗ: m=10000, n=10000
                    int tableSize = 10000;    // m = 10000
                    int elementCount = 10000; // n = 10000

                    var table = new OpenAddressingHashTable<int, string>(tableSize, hashFunction, resolver);
                    var stopwatch = Stopwatch.StartNew();
                    int inserted = 0;

                    // Генерируем 10000 УНИКАЛЬНЫХ ключей заранее
                    Console.Write("    Генерация ключей... ");
                    var uniqueKeys = GenerateUniqueKeys(elementCount, random);
                    Console.WriteLine("✅");

                    Console.Write("    Вставка элементов: ");

                    for (int j = 0; j < elementCount; j++)
                    {
                        try
                        {
                            if (table.Insert(uniqueKeys[j], $"Value_{j}"))
                                inserted++;
                        }
                        catch (InvalidOperationException ex) when (ex.Message.Contains("переполнения") || ex.Message.Contains("близка"))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n    ⚠️  Таблица заполнена на {table.Count}/{tableSize} элементов");
                            Console.ResetColor();
                            break;
                        }
                        catch (InvalidOperationException ex) when (ex.Message.Contains("Не удалось найти"))
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"\n    ⚠️  Не удалось найти свободную ячейку");
                            Console.ResetColor();
                            break;
                        }

                        // Показываем прогресс
                        if (j % 2000 == 0 && j > 0)
                        {
                            ShowMiniProgress(j, elementCount);
                        }
                    }

                    stopwatch.Stop();
                    Console.WriteLine(); // Новая строка после прогресс-бара

                    var stats = table.GetStatistics();
                    var result = new BenchmarkResult
                    {
                        TestName = resolver.Name,
                        HashMethod = HashMethodType.Division, // Все используют метод деления
                        ResolutionMethod = GetResolutionType(resolver),
                        TableSize = tableSize,
                        ElementCount = elementCount,
                        InsertedCount = inserted,
                        Duration = stopwatch.Elapsed,
                        Statistics = stats
                    };

                    _task2Results.Add(result);

                    // Показываем детальный результат
                    ShowDetailedResultCard(result, resolver.Name);
                }
                catch (Exception ex)
                {
                    ShowErrorCard($"Ошибка в методе {resolver.Name}", ex.Message);
                }

                UpdateEnhancedProgressBar(i + 1, resolvers.Length);

                if (i < resolvers.Length - 1)
                {
                    Thread.Sleep(300);
                }
            }

            Console.WriteLine("\n  ✅ Задание 2 завершено!");

            // Анализ результатов Задания 2
            AnalyzeTask2Results();

            ShowPressAnyKey();
        }

        static void AnalyzeTask2Results()
        {
            if (_task2Results.Count == 0) return;

            Console.WriteLine("\n  📈 АНАЛИЗ РЕЗУЛЬТАТОВ ЗАДАНИЯ 2:");
            Console.WriteLine("  ──────────────────────────────────────────────");

            // Группируем по успешности вставки
            var successful = _task2Results.Where(r => r.InsertedCount == r.ElementCount).ToList();
            var partial = _task2Results.Where(r => r.InsertedCount < r.ElementCount).ToList();

            if (successful.Count > 0)
            {
                Console.WriteLine("\n  ✅ УСПЕШНО ВСТАВЛЕНЫ ВСЕ 10000 ЭЛЕМЕНТОВ:");

                var bestSuccessful = successful
                    .OrderBy(r => r.Statistics.LongestCluster)
                    .ThenBy(r => r.Duration.TotalMilliseconds)
                    .First();

                Console.WriteLine($"    🏆 Лучший метод: {bestSuccessful.TestName}");
                Console.WriteLine($"       • Длина кластера: {bestSuccessful.Statistics.LongestCluster}");
                Console.WriteLine($"       • Время: {bestSuccessful.Duration.TotalMilliseconds:F0} мс");
                Console.WriteLine($"       • Коэффициент: {bestSuccessful.Statistics.LoadFactor:P2}");
            }

            if (partial.Count > 0)
            {
                Console.WriteLine("\n  ⚠️  ЧАСТИЧНО ЗАПОЛНЕННЫЕ ТАБЛИЦЫ:");

                foreach (var result in partial.OrderByDescending(r => r.InsertedCount))
                {
                    double fillPercentage = (double)result.InsertedCount / result.ElementCount;
                    Console.WriteLine($"    {result.TestName}:");
                    Console.WriteLine($"       • Вставлено: {result.InsertedCount} ({fillPercentage:P1})");
                    Console.WriteLine($"       • Причина: переполнение при {result.Statistics.LoadFactor:P2} заполнения");
                }
            }

            // Общие выводы
            Console.WriteLine("\n  💡 ВЫВОДЫ ПО ЗАДАНИЮ 2:");

            if (successful.Count == _task2Results.Count)
            {
                Console.WriteLine("    • Все методы справились с вставкой 10000 элементов");
                Console.WriteLine("    • Это хороший показатель для таблицы размером 10000 ячеек");
            }
            else if (successful.Count > 0)
            {
                Console.WriteLine("    • Некоторые методы не справились с полной вставкой");
                Console.WriteLine("    • Это демонстрирует проблему кластеризации");
            }
            else
            {
                Console.WriteLine("    • Ни один метод не смог вставить все 10000 элементов");
                Console.WriteLine("    • Возможно, размер таблицы слишком мал или много коллизий");
            }
        }

        static void ShowDetailedResultCard(BenchmarkResult result, string methodName)
        {
            Console.WriteLine("\n    📊 РЕЗУЛЬТАТЫ ТЕСТА:");
            Console.WriteLine("    ──────────────────────────────────────");

            Console.WriteLine($"    Метод: {methodName}");
            Console.WriteLine($"    Вставлено элементов: {result.InsertedCount}/{result.ElementCount}");

            if (result.InsertedCount < result.ElementCount)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"    ❗ Не вставлено: {result.ElementCount - result.InsertedCount} элементов");
                Console.ResetColor();
            }

            Console.WriteLine($"    Коэффициент заполнения: {result.Statistics.LoadFactor:P2}");
            Console.WriteLine($"    Длина самого длинного кластера: {result.Statistics.LongestCluster}");
            Console.WriteLine($"    Пустых ячеек: {result.Statistics.EmptyBuckets}");
            Console.WriteLine($"    Время выполнения: {result.Duration.TotalMilliseconds:F0} мс");

            // Визуализация кластера
            Console.Write($"    Эффективность: ");
            Console.ForegroundColor = GetEfficiencyColor(result.Statistics.LongestCluster, false);
            Console.WriteLine(GetEfficiencyRating(result.Statistics.LongestCluster, false));
            Console.ResetColor();

            // Мини-график кластеров
            if (result.Statistics.LongestCluster > 0)
            {
                Console.Write($"    Кластеры: ");
                Console.ForegroundColor = GetMetricColor(result.Statistics.LongestCluster, false);
                Console.WriteLine(new string('█', Math.Min(result.Statistics.LongestCluster / 2, 20)));
                Console.ResetColor();
            }
        }

        static int[] GenerateUniqueKeys(int count, Random random)
        {
            // Для производительности генерируем набор уникальных ключей заранее
            var keys = new HashSet<int>();

            // Генерируем ключи в диапазоне, чтобы минимизировать коллизии
            int minKey = 1;
            int maxKey = count * 10; // Большой диапазон для уникальности

            while (keys.Count < count)
            {
                keys.Add(random.Next(minKey, maxKey));
            }

            // Преобразуем в массив и перемешиваем
            var result = keys.ToArray();

            // Перемешиваем для случайного порядка
            for (int i = result.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (result[i], result[j]) = (result[j], result[i]);
            }

            return result;
        }

        static void AnalyzeAndShowResults()
        {
            Console.WriteLine("\n  📈 АНАЛИЗ РЕЗУЛЬТАТОВ ТЕСТИРОВАНИЯ\n");

            // Анимация анализа
            ShowProgressAnimation("Сбор статистики", 600);
            ShowProgressAnimation("Анализ эффективности", 800);
            ShowProgressAnimation("Формирование выводов", 400);

            // Показываем топ-результаты
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  🏆 ТОП-3 ХЕШ-ФУНКЦИИ (метод цепочек):");
            Console.ResetColor();

            if (_task1Results.Count > 0)
            {
                var topChainResults = _task1Results
                    .OrderBy(r => r.Statistics.LongestChain)
                    .Take(3)
                    .ToList();

                for (int i = 0; i < topChainResults.Count; i++)
                {
                    var result = topChainResults[i];
                    string medal = i == 0 ? "🥇" : i == 1 ? "🥈" : "🥉";

                    Console.WriteLine($"  {medal} {result.TestName,-20} │ Цепь: {result.Statistics.LongestChain,3} │ " +
                                    $"Эффективность: {GetEfficiencyStars(result.Statistics.LongestChain, true)}");
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n  🏆 ТОП-3 КОМБИНАЦИИ (открытая адресация):");
            Console.ResetColor();

            if (_task2Results.Count > 0)
            {
                var topOpenResults = _task2Results
                    .Where(r => r.InsertedCount >= r.ElementCount * 0.8)
                    .OrderBy(r => r.Statistics.LongestCluster)
                    .Take(3)
                    .ToList();

                for (int i = 0; i < topOpenResults.Count; i++)
                {
                    var result = topOpenResults[i];
                    string medal = i == 0 ? "🥇" : i == 1 ? "🥈" : "🥉";

                    Console.WriteLine($"  {medal} {result.TestName,-25} │ Кластер: {result.Statistics.LongestCluster,3} │ " +
                                    $"Эффективность: {GetEfficiencyStars(result.Statistics.LongestCluster, false)}");
                }
            }

            // Показываем сравнительную визуализацию
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  📊 СРАВНИТЕЛЬНАЯ ВИЗУАЛИЗАЦИЯ:");
            Console.ResetColor();

            if (_task1Results.Count > 0)
            {
                Console.WriteLine("\n  Метод цепочек (длина цепочки):");
                ShowComparisonVisualization(_task1Results, true);
            }

            if (_task2Results.Count > 0)
            {
                Console.WriteLine("\n  Открытая адресация (длина кластера):");
                ShowComparisonVisualization(_task2Results, false);
            }

            // Итоговые выводы
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n  🎯 ИТОГОВЫЕ ВЫВОДЫ:");
            Console.ResetColor();

            ShowConclusionCard("Лучшая хеш-функция",
                GetBestChainResult()?.TestName ?? "Не определено",
                "Для метода цепочек");

            ShowConclusionCard("Лучшая комбинация",
                GetBestOpenAddressingResult()?.TestName ?? "Не определено",
                "Для открытой адресации");

            ShowConclusionCard("Общая рекомендация",
                GetOverallRecommendation(),
                "Для практического применения");

            // Предложение сохранить результаты
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\n  💾 Сохранить полный отчет? (Да/Нет): ");
            Console.ResetColor();

            if (Console.ReadLine()?.ToLower().StartsWith("д") ?? false)
            {
                SaveResultsToFile();
                Console.WriteLine("  ✅ Отчет сохранен в файл 'results.txt'");
            }

            ShowPressAnyKey();
        }

        #endregion

        #region === ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ ДЛЯ ЛАБОРАТОРНОЙ РАБОТЫ ===

        static void ShowWizardStep(string stepNumber, string stepName, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"  {stepNumber}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(stepName);
            Console.WriteLine("  " + new string('─', 50));
            Console.ResetColor();
        }

        static void ShowTaskCard(string title, string subtitle, string details, string emoji)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\n  {emoji} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine($"    {subtitle}");
            Console.ForegroundColor = ConsoleColor.DarkGray;

            foreach (var line in details.Split('\n'))
            {
                Console.WriteLine($"      {line}");
            }

            Console.ResetColor();
        }

        static void ShowConfigOption(string number, string title, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"    {number}. ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{title,-20}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"— {description}");
            Console.ResetColor();
        }

        static (int TotalTests, int TotalElements, string EstimatedTime) GetTestScale(string choice)
        {
            return choice switch
            {
                "2" => (8, 50000, "10"),
                "3" => (4, 10000, "5"),
                _ => (21, 150000, "25") // По умолчанию полный тест
            };
        }

        static void ShowExecutionStage(string stageName, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            CenterText(new string('═', 60));
            CenterText(stageName);
            CenterText(new string('═', 60));
            Console.ResetColor();
            Console.WriteLine();
        }

        static void ShowEnhancedProgressBar(string taskName, int totalSteps)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n  📋 {taskName}");
            Console.ResetColor();
            Console.WriteLine("  " + new string('─', 50));
            Console.WriteLine();
        }

        static void ShowCurrentTestInfo(int current, int total, string testName)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"\n\n  Тест {current}/{total}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(testName);
            Console.ResetColor();
        }

        static void ShowMiniProgress(int current, int total)
        {
            int width = 20;
            double percentage = (double)current / total;
            int progress = (int)(width * percentage);

            Console.Write($"\r    [");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('░', progress));
            Console.ResetColor();
            Console.Write(new string(' ', width - progress));
            Console.Write($"] {percentage:P0}");
        }

        static void UpdateEnhancedProgressBar(int current, int total)
        {
            int width = 40;
            double percentage = (double)current / total;
            int progress = (int)(width * percentage);

            string[] phases = { "▏", "▎", "▍", "▌", "▋", "▊", "▉", "█" };
            string progressChar = phases[Math.Min((int)(percentage * phases.Length), phases.Length - 1)];

            Console.Write($"\r  Общий прогресс: [");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(new string('█', progress));
            if (progress < width) Console.Write(progressChar);
            Console.ResetColor();
            Console.Write(new string(' ', width - progress));
            Console.Write($"] {percentage:P0}");
        }

        static void ShowTestResultCard(int metric, double loadFactor, double timeMs, string testName)
        {
            // Ограничиваем коэффициент заполнения 100%
            double displayLoadFactor = Math.Min(loadFactor, 1.0);

            Console.Write($"\n    📊 ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{testName,-25}");
            Console.ResetColor();

            Console.Write($" │ 📏 {metric,3} ");

            // Цветовая индикация метрики
            bool isChain = testName.Contains("цеп") || !testName.Contains("+");
            Console.ForegroundColor = GetMetricColor(metric, isChain);
            Console.Write(new string('█', Math.Min(metric, 10)));
            Console.ResetColor();

            // Корректное отображение коэффициента заполнения
            if (loadFactor > 1.0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($" │ 📈 >100% ");
                Console.ResetColor();
            }
            else
            {
                Console.Write($" │ 📈 {displayLoadFactor,6:P1} ");
            }

            Console.Write($"│ ⚡ {timeMs,5:F0}мс");

            // Рейтинг эффективности
            Console.ForegroundColor = GetEfficiencyColor(metric, isChain);
            Console.Write($" │ {GetEfficiencyRating(metric, isChain)}");
            Console.ResetColor();

            Console.WriteLine();
        }

        static void ShowErrorCard(string title, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"\n    ❌ {title}: ");
            Console.ResetColor();
            Console.WriteLine(message);
        }

        static string GetEfficiencyStars(int metric, bool isChain)
        {
            int stars = isChain
                ? metric switch { < 3 => 5, < 10 => 4, < 20 => 3, < 30 => 2, _ => 1 }
                : metric switch { < 5 => 5, < 15 => 4, < 25 => 3, < 40 => 2, _ => 1 };

            return new string('★', stars) + new string('☆', 5 - stars);
        }

        static void ShowComparisonVisualization(List<BenchmarkResult> results, bool isChain)
        {
            var topResults = results
                .OrderBy(r => isChain ? r.Statistics.LongestChain : r.Statistics.LongestCluster)
                .Take(5)
                .ToList();

            foreach (var result in topResults)
            {
                int value = isChain ? result.Statistics.LongestChain : result.Statistics.LongestCluster;
                string name = result.TestName.PadRight(25).Substring(0, 25);

                // График в виде горизонтальных полос
                int barLength = Math.Min(value * (isChain ? 2 : 1), 30);
                string bar = new string('█', barLength);

                Console.Write($"    {name} │ ");
                Console.ForegroundColor = GetPerformanceColor(value, isChain ? "chain" : "cluster");
                Console.WriteLine($"{bar} {value}");
                Console.ResetColor();
            }
        }

        static void ShowConclusionCard(string title, string value, string description)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"\n    • {title}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"      {description}");
            Console.ResetColor();
        }

        static string GetOverallRecommendation()
        {
            if (_task1Results.Count == 0 || _task2Results.Count == 0)
                return "Недостаточно данных";

            var bestChain = GetBestChainResult();
            var bestOpen = GetBestOpenAddressingResult();

            if (bestChain == null || bestOpen == null)
                return "Недостаточно данных";

            int chainMetric = bestChain.Statistics.LongestChain;
            int openMetric = bestOpen.Statistics.LongestCluster;

            // Простая логика рекомендации
            if (chainMetric < 5 && openMetric < 10)
                return "Оба метода эффективны. Выбор зависит от конкретной задачи.";
            else if (chainMetric < openMetric * 2)
                return "Метод цепочек показывает лучшие результаты";
            else
                return "Открытая адресация более эффективна для этих данных";
        }

        static void SaveResultsToFile()
        {
            try
            {
                using var writer = new StreamWriter("results.txt", false, System.Text.Encoding.UTF8);
                writer.WriteLine("ОТЧЕТ ПО ЛАБОРАТОРНОЙ РАБОТЕ: ХЕШ-ТАБЛИЦЫ");
                writer.WriteLine(new string('=', 60));
                writer.WriteLine($"Дата генерации: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                writer.WriteLine();

                if (_task1Results.Count > 0)
                {
                    writer.WriteLine("ЗАДАНИЕ 1: ХЕШ-ТАБЛИЦЫ С ЦЕПОЧКАМИ");
                    writer.WriteLine(new string('-', 60));

                    foreach (var result in _task1Results.OrderBy(r => r.Statistics.LongestChain))
                    {
                        writer.WriteLine($"{result.TestName}:");
                        writer.WriteLine($"  • Длина цепочки: {result.Statistics.LongestChain}");
                        writer.WriteLine($"  • Коэффициент заполнения: {result.Statistics.LoadFactor:P2}");
                        writer.WriteLine($"  • Время: {result.Duration.TotalMilliseconds:F0} мс");
                        writer.WriteLine();
                    }
                }

                if (_task2Results.Count > 0)
                {
                    writer.WriteLine("ЗАДАНИЕ 2: ХЕШ-ТАБЛИЦЫ С ОТКРЫТОЙ АДРЕСАЦИЕЙ");
                    writer.WriteLine(new string('-', 60));

                    foreach (var result in _task2Results.OrderBy(r => r.Statistics.LongestCluster))
                    {
                        writer.WriteLine($"{result.TestName}:");
                        writer.WriteLine($"  • Длина кластера: {result.Statistics.LongestCluster}");
                        writer.WriteLine($"  • Коэффициент заполнения: {result.Statistics.LoadFactor:P2}");
                        writer.WriteLine($"  • Вставлено: {result.InsertedCount}/{result.ElementCount}");
                        writer.WriteLine();
                    }
                }
            }
            catch
            {
                // Игнорируем ошибки записи
            }
        }

        static ConsoleColor GetMetricColor(int value, bool isChain)
        {
            if (isChain)
            {
                // Для метода цепочек учитываем, что могут быть длинные цепочки
                return value switch
                {
                    < 5 => ConsoleColor.Green,      // Отлично (< 5)
                    < 20 => ConsoleColor.Yellow,    // Хорошо (< 20)
                    < 50 => ConsoleColor.DarkYellow,// Удовлетворительно (< 50)
                    < 100 => ConsoleColor.Red,      // Плохо (< 100)
                    _ => ConsoleColor.DarkRed       // Очень плохо (≥ 100)
                };
            }
            else
            {
                return value switch
                {
                    < 10 => ConsoleColor.Green,     // Отлично (< 10)
                    < 30 => ConsoleColor.Yellow,    // Хорошо (< 30)
                    < 60 => ConsoleColor.DarkYellow,// Удовлетворительно (< 60)
                    < 100 => ConsoleColor.Red,      // Плохо (< 100)
                    _ => ConsoleColor.DarkRed       // Очень плохо (≥ 100)
                };
            }
        }

        static ConsoleColor GetEfficiencyColor(int value, bool isChain)
        {
            if (isChain)
            {
                return value switch
                {
                    < 3 => ConsoleColor.Green,
                    < 10 => ConsoleColor.Yellow,
                    < 20 => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.Red
                };
            }
            else
            {
                return value switch
                {
                    < 5 => ConsoleColor.Green,
                    < 15 => ConsoleColor.Yellow,
                    < 30 => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.Red
                };
            }
        }

        static string GetEfficiencyRating(int value, bool isChain)
        {
            if (isChain)
            {
                return value switch
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
                return value switch
                {
                    < 5 => "Отлично ⭐⭐⭐⭐⭐",
                    < 15 => "Очень хорошо ⭐⭐⭐⭐",
                    < 30 => "Хорошо ⭐⭐⭐",
                    < 50 => "Удовлетворительно ⭐⭐",
                    _ => "Плохо ⭐"
                };
            }
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

        #endregion

        #region === ГАЛЕРЕЯ ВИЗУАЛИЗАЦИИ ===

        static void ShowVisualizationMenu()
        {
            Console.Clear();
            ShowSectionHeader("🎨 ГАЛЕРЕЯ ВИЗУАЛИЗАЦИИ", ConsoleColor.Magenta);

            Console.ForegroundColor = ConsoleColor.White;
            CenterText("Выберите тип визуализации:", 2);
            Console.ResetColor();

            ShowMenuOption("1️⃣", "Демонстрация метода цепочек", "Наглядный пример работы цепочек");
            ShowMenuOption("2️⃣", "Демонстрация открытой адресации", "Визуализация кластеров");
            ShowMenuOption("3️⃣", "Анимация вставки элемента", "Пошаговая демонстрация вставки");
            ShowMenuOption("4️⃣", "Тепловая карта заполнения", "Графическое представление распределения");
            ShowMenuOption("5️⃣", "Генерация HTML-отчета", "Создание интерактивного отчета в браузере");
            ShowMenuOption("6️⃣", "🔙 Назад", "Возврат в главное меню");

            Console.Write("\n  Выбор (1-6): ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DemoChainedTable();
                    break;
                case "2":
                    DemoOpenAddressingTable();
                    break;
                case "3":
                    DemoInsertionAnimation();
                    break;
                case "4":
                    DemoHeatmap();
                    break;
                case "5":
                    GenerateHtmlReport();
                    break;
                default:
                    return;
            }
        }

        static void DemoChainedTable()
        {
            Console.Clear();
            ShowSectionHeader("🏗️  ДЕМОНСТРАЦИЯ МЕТОДА ЦЕПОЧЕК", ConsoleColor.Cyan);

            // Создаем небольшую таблицу для демонстрации
            var table = new ChainedHashTable<int, string>(15, new DivisionHash());
            var random = new Random();

            Console.WriteLine("\n  Генерируем и вставляем 20 случайных элементов...\n");

            // Вставляем элементы
            int inserted = 0;
            for (int i = 0; i < 20; i++)
            {
                int key = random.Next(1, 100);
                if (table.Insert(key, $"Value_{i}"))
                {
                    inserted++;
                    Console.Write($"  Вставка {i + 1:00}: ключ {key:00} → ");

                    int hash = key % 15;
                    Console.WriteLine($"индекс {hash}");

                    // Простая анимация
                    ShowSimpleTableAnimation(15, hash);
                    Thread.Sleep(100);

                    if (i < 19) Console.SetCursorPosition(0, Console.CursorTop - 2);
                }
            }

            Console.SetCursorPosition(0, Console.CursorTop + 3);
            Console.WriteLine($"  ✅ Успешно вставлено: {inserted} элементов");

            // Визуализируем таблицу
            Console.WriteLine("\n  📊 Состояние хеш-таблицы:");
            Console.WriteLine("  ──────────────────────────────");

            for (int i = 0; i < 15; i++)
            {
                Console.Write($"  [{i,2}] → ");
                if (table.Search(i, out var value))
                    Console.WriteLine($"Ключ: {value.Replace("Value_", "")}");
                else
                    Console.WriteLine("∅");
            }

            // Статистика
            var stats = table.GetStatistics();
            Console.WriteLine("\n  📈 Статистика:");
            Console.WriteLine($"    • Размер таблицы: 15 ячеек");
            Console.WriteLine($"    • Элементов: {table.Count}");
            Console.WriteLine($"    • Коэффициент заполнения: {stats.LoadFactor:P2}");
            Console.WriteLine($"    • Самая длинная цепочка: {stats.LongestChain}");
            Console.WriteLine($"    • Пустых ячеек: {stats.EmptyBuckets}");

            // Визуализация цепочек
            Console.WriteLine("\n  🔗 Визуализация цепочек:");
            var chainLengths = table.GetChainLengths();
            for (int i = 0; i < chainLengths.Length; i++)
            {
                if (chainLengths[i] > 0)
                {
                    Console.Write($"    [{i}]: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(new string('●', chainLengths[i]) + $" ({chainLengths[i]})");
                    Console.ResetColor();
                }
            }

            ShowPressAnyKey();
        }

        static void DemoOpenAddressingTable()
        {
            Console.Clear();
            ShowSectionHeader("🔍 ДЕМОНСТРАЦИЯ ОТКРЫТОЙ АДРЕСАЦИИ", ConsoleColor.Magenta);

            // Создаем таблицу для демонстрации
            var table = new OpenAddressingHashTable<int, string>(20, new DivisionHash(), new LinearProbing());
            var random = new Random();

            Console.WriteLine("\n  Вставляем 15 элементов в таблицу размером 20...\n");

            // Вставляем элементы
            for (int i = 0; i < 15; i++)
            {
                int key = random.Next(1, 50);
                try
                {
                    if (table.Insert(key, $"Value_{i}"))
                    {
                        Console.Write($"  Вставка {i + 1:00}: ключ {key:00} → ");

                        // Показываем поиск ячейки
                        for (int attempt = 0; attempt < 3; attempt++)
                        {
                            int index = (key + attempt) % 20;
                            Console.Write($"[{index}] ");
                            Thread.Sleep(50);
                        }
                        Console.WriteLine("✓");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ❌ Ошибка: {ex.Message}");
                }
                Thread.Sleep(100);
            }

            // Визуализируем таблицу
            Console.WriteLine("\n  📊 Состояние таблицы:");
            Console.WriteLine("  (занятые ячейки отмечены цветом)");
            Console.WriteLine("  ──────────────────────────────");

            int cols = 5;
            for (int row = 0; row < 4; row++)
            {
                Console.Write("    ");
                for (int col = 0; col < cols; col++)
                {
                    int idx = row * cols + col;
                    if (table.Search(idx, out var value))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        string display = value.Replace("Value_", "");
                        Console.Write($"[{display.PadLeft(2)}]");
                        Console.ResetColor();
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write($"[  ] ");
                    }
                }
                Console.WriteLine();
            }

            // Статистика
            var stats = table.GetStatistics();
            Console.WriteLine("\n  📈 Статистика:");
            Console.WriteLine($"    • Размер таблицы: 20 ячеек");
            Console.WriteLine($"    • Элементов: {table.Count}");
            Console.WriteLine($"    • Коэффициент заполнения: {stats.LoadFactor:P2}");
            Console.WriteLine($"    • Самый длинный кластер: {stats.LongestCluster}");
            Console.WriteLine($"    • Пустых ячеек: {stats.EmptyBuckets}");

            // Визуализация кластеров
            Console.WriteLine("\n  🔍 Поиск самого длинного кластера...");
            int longestCluster = table.CalculateLongestCluster();
            Console.WriteLine($"    Самый длинный кластер: {longestCluster} ячеек");

            if (longestCluster > 0)
            {
                Console.Write("    Визуализация: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(new string('█', Math.Min(longestCluster, 20)));
                Console.ResetColor();
                Console.WriteLine("    (это последовательные занятые ячейки)");
            }

            ShowPressAnyKey();
        }

        static void DemoInsertionAnimation()
        {
            Console.Clear();
            ShowSectionHeader("🎬 АНИМАЦИЯ ВСТАВКИ ЭЛЕМЕНТА", ConsoleColor.Green);

            Console.WriteLine("\n  Демонстрация процесса вставки элемента в хеш-таблицу");
            Console.WriteLine("  ──────────────────────────────────────────────\n");

            var table = new ChainedHashTable<int, string>(10, new DivisionHash());
            var random = new Random();

            // Очищаем место для анимации
            int animationStartLine = Console.CursorTop;

            // Демонстрация 3 вставок
            for (int demo = 0; demo < 3; demo++)
            {
                // Сохраняем позицию для текущей демонстрации
                int demoStartLine = Console.CursorTop;

                int key = random.Next(1, 30);
                string value = $"Demo_{demo}";

                Console.WriteLine($"\n  🎯 Вставка #{demo + 1}:");
                Console.WriteLine($"    Ключ: {key}");
                Console.WriteLine($"    Значение: {value}");

                // Оставляем пустые строки для таблицы
                Console.WriteLine("\n    Текущее состояние таблицы:");
                Console.WriteLine("    ───────────────────────────");

                // Выводим начальное состояние таблицы
                var chainLengths = table.GetChainLengths();
                for (int i = 0; i < chainLengths.Length; i++)
                {
                    Console.WriteLine($"      [{i}]: ∅");
                }

                // Шаг 1: Вычисление хеша
                Console.SetCursorPosition(0, demoStartLine + 3);
                Console.Write("    Шаг 1: Вычисляем хеш...");
                Thread.Sleep(500);
                int hash = key % 10;
                Console.WriteLine($" hash({key}) = {hash}");

                // Шаг 2: Определение индекса
                Console.Write("    Шаг 2: Определяем индекс...");
                Thread.Sleep(500);
                int index = hash % 10;
                Console.WriteLine($" {hash} % 10 = {index}");

                // Шаг 3: Вставка с анимацией
                Console.Write($"    Шаг 3: Проверяем ячейку [{index}]...");
                Thread.Sleep(500);

                bool inserted = table.Insert(key, value);

                if (inserted)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine(" СВОБОДНО → вставляем ✓");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" ЗАНЯТО → создаем цепочку ⛓️");
                    Console.ResetColor();
                }

                Thread.Sleep(1000);

                // Обновляем отображение таблицы
                Console.SetCursorPosition(0, demoStartLine + 5);
                Console.WriteLine("    Текущее состояние таблицы:");
                Console.WriteLine("    ───────────────────────────");

                chainLengths = table.GetChainLengths();
                for (int i = 0; i < chainLengths.Length; i++)
                {
                    Console.Write($"      [{i}]: ");
                    if (chainLengths[i] > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(new string('●', chainLengths[i]));
                        Console.ResetColor();
                        Console.WriteLine($" ({chainLengths[i]})");
                    }
                    else
                    {
                        Console.WriteLine("∅");
                    }
                }

                if (demo < 2)
                {
                    Console.WriteLine("\n  ⏭️  Переходим к следующей вставке...");
                    Thread.Sleep(2000);

                    // Очищаем область для следующей демонстрации
                    int linesToClear = Console.CursorTop - demoStartLine;
                    Console.SetCursorPosition(0, demoStartLine);
                    for (int i = 0; i < linesToClear; i++)
                    {
                        Console.WriteLine(new string(' ', Console.WindowWidth));
                    }
                    Console.SetCursorPosition(0, demoStartLine);
                }
            }

            // Финальное отображение
            Console.WriteLine("\n\n  🎉 Анимация завершена!");
            Console.WriteLine("  В таблицу успешно вставлены 3 элемента.");

            // Показываем итоговую статистику
            var finalStats = table.GetStatistics();
            Console.WriteLine("\n  📊 Итоговая статистика:");
            Console.WriteLine($"    • Размер таблицы: 10 ячеек");
            Console.WriteLine($"    • Элементов: {table.Count}");
            Console.WriteLine($"    • Коэффициент заполнения: {finalStats.LoadFactor:P2}");
            Console.WriteLine($"    • Самая длинная цепочка: {finalStats.LongestChain}");
            Console.WriteLine($"    • Пустых ячеек: {finalStats.EmptyBuckets}");

            ShowPressAnyKey();
        }

        static void DemoHeatmap()
        {
            Console.Clear();
            ShowSectionHeader("🔥 ТЕПЛОВАЯ КАРТА ЗАПОЛНЕНИЯ", ConsoleColor.Red);

            Console.WriteLine("\n  Визуализация распределения элементов в хеш-таблице");
            Console.WriteLine("  ──────────────────────────────────────────────────────\n");

            // Создаем таблицу с разными методами для сравнения
            Console.WriteLine("  Тестируем 2 метода с одинаковыми данными:\n");

            // Метод 1: Линейное исследование
            Console.WriteLine("  1. Открытая адресация (линейное исследование)");
            var table1 = new OpenAddressingHashTable<int, string>(100, new DivisionHash(), new LinearProbing());
            var random = new Random();

            Console.Write("     Заполняем на 70%... ");
            int inserted1 = 0;
            for (int i = 0; i < 70; i++)
            {
                try
                {
                    if (table1.Insert(random.Next(1, 200), $"Value_{i}"))
                        inserted1++;
                }
                catch
                {
                    break;
                }
            }
            Console.WriteLine($"✅ ({inserted1} элементов)");

            var stats1 = table1.GetStatistics();
            Console.WriteLine($"     • Самый длинный кластер: {stats1.LongestCluster}");
            Console.WriteLine($"     • Пустых ячеек: {stats1.EmptyBuckets}");

            // Метод 2: Метод цепочек
            Console.WriteLine("\n  2. Метод цепочек");
            var table2 = new ChainedHashTable<int, string>(100, new DivisionHash());

            Console.Write("     Заполняем на 70%... ");
            int inserted2 = 0;
            for (int i = 0; i < 70; i++)
            {
                if (table2.Insert(random.Next(1, 200), $"Value_{i}"))
                    inserted2++;
            }
            Console.WriteLine($"✅ ({inserted2} элементов)");

            var stats2 = table2.GetStatistics();
            Console.WriteLine($"     • Самая длинная цепочка: {stats2.LongestChain}");
            Console.WriteLine($"     • Пустых ячеек: {stats2.EmptyBuckets}");

            Console.WriteLine("\n  🔥 ТЕПЛОВЫЕ КАРТЫ:");
            Console.WriteLine("  ──────────────────────────────────────────────────────");

            // Тепловая карта для открытой адресации
            Console.WriteLine("\n  📍 Открытая адресация:");
            Console.WriteLine("  Каждый символ = 2 ячейки таблицы (всего 100 ячеек)");
            Console.WriteLine("  Цвет показывает заполненность области:\n");

            var occupancyMap1 = table1.GetOccupancyMap();
            DrawHeatmapWithLegend(occupancyMap1, "Линейное исследование");

            // Анализ
            Console.WriteLine("\n  📊 АНАЛИЗ ОТКРЫТОЙ АДРЕСАЦИИ:");
            if (stats1.LongestCluster > 15)
            {
                Console.WriteLine("    ❗ Обнаружена СИЛЬНАЯ КЛАСТЕРИЗАЦИЯ");
                Console.WriteLine("    • Длинные кластеры замедляют поиск");
                Console.WriteLine("    • Рекомендуется использовать другой метод разрешения");
            }
            else if (stats1.LongestCluster > 8)
            {
                Console.WriteLine("    ⚠️  Обнаружена УМЕРЕННАЯ кластеризация");
                Console.WriteLine("    • Это нормально для линейного исследования");
                Console.WriteLine("    • Можно попробовать квадратичное или двойное хеширование");
            }
            else
            {
                Console.WriteLine("    ✅ ХОРОШЕЕ распределение");
                Console.WriteLine("    • Минимальная кластеризация");
                Console.WriteLine("    • Линейное исследование работает эффективно");
            }

            // Тепловая карта для метода цепочек
            Console.WriteLine("\n\n  ⛓️  Метод цепочек:");
            Console.WriteLine("  Каждый символ = 1 ячейка таблицы (всего 100 ячеек)");
            Console.WriteLine("  Цвет показывает длину цепочки в ячейке:\n");

            var chainLengths = table2.GetChainLengths();
            DrawChainHeatmap(chainLengths);

            // Анализ
            Console.WriteLine("\n  📊 АНАЛИЗ МЕТОДА ЦЕПОЧЕК:");
            if (stats2.LongestChain > 5)
            {
                Console.WriteLine("    ❗ Обнаружены ДЛИННЫЕ ЦЕПОЧКИ");
                Console.WriteLine("    • Поиск в длинных цепочках замедлен");
                Console.WriteLine("    • Рекомендуется улучшить хеш-функцию");
            }
            else if (stats2.LongestChain > 3)
            {
                Console.WriteLine("    ⚠️  НЕКОТОРЫЕ длинные цепочки");
                Console.WriteLine("    • Распределение удовлетворительное");
                Console.WriteLine("    • Можно рассмотреть оптимизацию");
            }
            else
            {
                Console.WriteLine("    ✅ ОТЛИЧНОЕ распределение");
                Console.WriteLine("    • Короткие цепочки обеспечивают быстрый поиск");
                Console.WriteLine("    • Хеш-функция работает эффективно");
            }

            Console.WriteLine("\n  💡 ВЫВОД:");
            Console.WriteLine("  Тепловая карта помогает визуально оценить:");
            Console.WriteLine("  • Равномерность распределения элементов");
            Console.WriteLine("  • Наличие кластеров (для открытой адресации)");
            Console.WriteLine("  • Длины цепочек (для метода цепочек)");
            Console.WriteLine("  • Эффективность хеш-функции");

            ShowPressAnyKey();
        }

        static void DrawHeatmapWithLegend(bool[] occupancyMap, string title)
        {
            int width = 25; // 25 символов ширины
            int height = 4; // 4 строки = 100 ячеек (25×4=100)

            Console.WriteLine($"  {title}:");
            Console.WriteLine("  " + new string('▁', width + 2));

            for (int y = 0; y < height; y++)
            {
                Console.Write("  ▏");
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    if (idx < occupancyMap.Length)
                    {
                        bool occupied = occupancyMap[idx];
                        Console.ForegroundColor = occupied ? ConsoleColor.Red : ConsoleColor.DarkGray;
                        Console.Write(occupied ? "█" : "·");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("▕");
            }

            Console.WriteLine("  " + new string('▔', width + 2));

            // Легенда
            Console.WriteLine("\n  📖 Легенда:");
            Console.Write("    ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("·");
            Console.ResetColor();
            Console.WriteLine(" - пустая ячейка");

            Console.Write("    ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("█");
            Console.ResetColor();
            Console.WriteLine(" - занятая ячейка");

            Console.WriteLine("\n  📐 Масштаб: 1 символ = 1 ячейка таблицы");
        }

        static void DrawChainHeatmap(int[] chainLengths)
        {
            int width = 10; // 10 столбцов
            int height = 10; // 10 строк = 100 ячеек

            Console.WriteLine("  " + new string('▁', width + 2));

            for (int y = 0; y < height; y++)
            {
                Console.Write("  ▏");
                for (int x = 0; x < width; x++)
                {
                    int idx = y * width + x;
                    if (idx < chainLengths.Length)
                    {
                        int length = chainLengths[idx];
                        Console.ForegroundColor = GetChainHeatmapColor(length);
                        Console.Write(GetChainHeatmapChar(length));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("▕");
            }

            Console.WriteLine("  " + new string('▔', width + 2));

            // Легенда для цепочек
            Console.WriteLine("\n  📖 Легенда (длина цепочки):");
            Console.WriteLine("    0    1    2    3    4+");
            Console.Write("    ");
            Console.ForegroundColor = ConsoleColor.DarkGray; Console.Write("·");
            Console.ForegroundColor = ConsoleColor.Green; Console.Write("░");
            Console.ForegroundColor = ConsoleColor.Yellow; Console.Write("▒");
            Console.ForegroundColor = ConsoleColor.Red; Console.Write("▓");
            Console.ResetColor();
            Console.WriteLine();
        }

        static ConsoleColor GetChainHeatmapColor(int length)
        {
            return length switch
            {
                0 => ConsoleColor.DarkGray,
                1 => ConsoleColor.Green,
                2 => ConsoleColor.Yellow,
                3 => ConsoleColor.DarkYellow,
                _ => ConsoleColor.Red
            };
        }

        static char GetChainHeatmapChar(int length)
        {
            return length switch
            {
                0 => '·',
                1 => '░',
                2 => '▒',
                3 => '▓',
                _ => '█'
            };
        }
        static void ShowSimpleTableAnimation(int size, int activeIndex)
        {
            Console.Write("    ");
            for (int i = 0; i < size; i++)
            {
                if (i == activeIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
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

        static void GenerateHtmlReport()
        {
            Console.Clear();
            ShowSectionHeader("🌐 ГЕНЕРАЦИЯ HTML-ОТЧЕТА", ConsoleColor.Cyan);

            Console.WriteLine("\n  Создание интерактивного отчета для браузера\n");
            Console.WriteLine("  ⚙️  Настройка параметров отчета:");
            Console.WriteLine("  ──────────────────────────────────\n");

            // Выбор типа таблицы
            Console.WriteLine("  1. Выберите тип хеш-таблицы:");
            Console.WriteLine("     [1] Метод цепочек");
            Console.WriteLine("     [2] Открытая адресация");
            Console.Write("     Ваш выбор (1-2): ");
            string tableTypeChoice = Console.ReadLine();

            // Выбор хеш-функции
            Console.WriteLine("\n  2. Выберите хеш-функцию:");
            Console.WriteLine("     [1] Метод деления");
            Console.WriteLine("     [2] Метод умножения");
            Console.WriteLine("     [3] Custom 1 (XOR сдвиг)");
            Console.WriteLine("     [4] Custom 2 (FNV-1a inspired)");
            Console.WriteLine("     [5] Custom 3 (Многочленный)");
            Console.WriteLine("     [6] Custom 4 (MD5-like)");
            Console.Write("     Ваш выбор (1-6): ");
            string hashChoice = Console.ReadLine();

            // Размер таблицы
            Console.Write("\n  3. Размер таблицы (по умолчанию 100): ");
            string sizeInput = Console.ReadLine();
            int tableSize = string.IsNullOrEmpty(sizeInput) ? 100 : int.Parse(sizeInput);

            // Количество элементов
            Console.Write("  4. Количество элементов для вставки (по умолчанию 70): ");
            string countInput = Console.ReadLine();
            int elementCount = string.IsNullOrEmpty(countInput) ? 70 : int.Parse(countInput);

            // Название отчета
            Console.Write("  5. Название отчета (по умолчанию 'Анализ хеш-таблицы'): ");
            string reportTitle = Console.ReadLine();
            if (string.IsNullOrEmpty(reportTitle))
                reportTitle = "Анализ хеш-таблицы";

            Console.WriteLine("\n  ⚡ Генерация отчета...");

            try
            {
                IHashFunction<int> hashFunction = hashChoice switch
                {
                    "2" => new MultiplicationHash(),
                    "3" => new CustomHash1(),
                    "4" => new CustomHash2(),
                    "5" => new CustomHash3(),
                    "6" => new CustomHash4(),
                    _ => new DivisionHash()
                };

                string html = "";

                if (tableTypeChoice == "2")
                {
                    // Открытая адресация
                    Console.Write("  Выберите метод разрешения коллизий [1-линейное, 2-квадратичное, 3-двойное]: ");
                    string resolverChoice = Console.ReadLine();

                    ICollisionResolver resolver = resolverChoice switch
                    {
                        "2" => new QuadraticProbing(),
                        "3" => new DoubleHashing(),
                        _ => new LinearProbing()
                    };

                    var table = new OpenAddressingHashTable<int, string>(tableSize, hashFunction, resolver);
                    var random = new Random();

                    Console.Write($"  Вставляем {elementCount} элементов... ");
                    for (int i = 0; i < elementCount; i++)
                    {
                        try
                        {
                            table.Insert(random.Next(1, tableSize * 3), $"Element_{i}");
                        }
                        catch
                        {
                            break;
                        }
                    }
                    Console.WriteLine("✅");

                    html = HtmlVisualizer.GenerateOpenAddressingReport(table, hashFunction, resolver, reportTitle);
                }
                else
                {
                    // Метод цепочек
                    var table = new ChainedHashTable<int, string>(tableSize, hashFunction);
                    var random = new Random();

                    Console.Write($"  Вставляем {elementCount} элементов... ");
                    for (int i = 0; i < elementCount; i++)
                    {
                        table.Insert(random.Next(1, tableSize * 3), $"Element_{i}");
                    }
                    Console.WriteLine("✅");

                    html = HtmlVisualizer.GenerateChainedReport(table, hashFunction, reportTitle);
                }

                // Сохранение отчета
                Console.Write("  Сохраняем отчет... ");
                string fileName = $"hash_report_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                HtmlVisualizer.SaveHtmlReport(html, fileName);

                Console.WriteLine("✅\n");

                Console.WriteLine("  🎉 ОТЧЕТ УСПЕШНО СОЗДАН!");
                Console.WriteLine($"  📁 Файл: {fileName}");
                Console.WriteLine($"  📊 Тип таблицы: {(tableTypeChoice == "2" ? "Открытая адресация" : "Метод цепочек")}");
                Console.WriteLine($"  🔑 Хеш-функция: {hashFunction.Name}");
                Console.WriteLine($"  📏 Размер таблицы: {tableSize} ячеек");
                Console.WriteLine($"  🔢 Элементов: {elementCount}");

                Console.WriteLine("\n  🚀 Действия:");
                Console.WriteLine("  1. Откройте файл в браузере");
                Console.WriteLine("  2. Для сравнения создайте несколько отчетов с разными параметрами");
                Console.WriteLine("  3. Используйте отчет для анализа эффективности методов");

                // Открыть в браузере?
                Console.Write("\n  🌐 Открыть отчет в браузере сейчас? (Да/Нет): ");
                if (Console.ReadLine()?.ToLower().StartsWith("д") ?? false)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = fileName,
                            UseShellExecute = true
                        });
                    }
                    catch
                    {
                        Console.WriteLine("  ⚠️  Не удалось открыть браузер. Откройте файл вручную.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n  ❌ Ошибка: {ex.Message}");
                Console.WriteLine("  Проверьте введенные параметры и попробуйте снова.");
            }

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

        #endregion
    }
}