using HashTablesLab.HashTables;
using System;
using System.Threading;

namespace HashTablesLab.Visualization
{
    public static class AnimationDemo
    {
        public static void ShowInsertionAnimation<TKey, TValue>(
            ChainedHashTable<TKey, TValue> table,
            TKey key,
            TValue value,
            int delayMs = 100)
        {
            Console.Clear();
            Console.WriteLine("🎬 Анимация вставки элемента в хеш-таблицу");
            Console.WriteLine("═══════════════════════════════════════════════\n");

            Console.WriteLine($"  Ключ: {key}");
            Console.WriteLine($"  Значение: {value}");

            int hash = table.GetHashCode(); // Нужно добавить метод GetHash в таблицу
            Console.WriteLine($"\n  Шаг 1: Вычисляем хеш...");
            Thread.Sleep(delayMs);
            Console.WriteLine($"        hash({key}) = {hash}");

            int index = hash % table.GetTableSize(); // Нужно добавить метод GetTableSize
            Console.WriteLine($"  Шаг 2: Определяем индекс...");
            Thread.Sleep(delayMs);
            Console.WriteLine($"        {hash} % {table.GetTableSize()} = {index}");

            Console.WriteLine($"\n  Шаг 3: Вставляем в ячейку [{index}]...");
            Thread.Sleep(delayMs);

            bool inserted = table.Insert(key, value);

            if (inserted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"  ✓ Элемент успешно вставлен!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"  ⚠️  Элемент уже существует в таблице");
                Console.ResetColor();
            }

            Thread.Sleep(delayMs * 2);
            Console.WriteLine("\n  Текущее состояние таблицы:");
            // Вызываем метод визуализации таблицы
        }

        public static void ShowCollisionAnimation<TKey, TValue>(
            OpenAddressingHashTable<TKey, TValue> table,
            TKey key,
            TValue value,
            int delayMs = 150)
        {
            Console.Clear();
            Console.WriteLine("💥 Анимация разрешения коллизии");
            Console.WriteLine("═══════════════════════════════════════════════\n");

            Console.WriteLine($"  Ключ: {key}");

            int attempt = 0;
            while (attempt < 5) // Показываем первые 5 попыток
            {
                Console.WriteLine($"\n  Попытка #{attempt + 1}:");

                // Эмуляция поиска свободной ячейки
                Console.Write($"    Проверяем ячейку... ");
                Thread.Sleep(delayMs);

                if (attempt == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ЗАНЯТО! (коллизия)");
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = attempt < 3 ? ConsoleColor.Yellow : ConsoleColor.Green;
                    Console.WriteLine(attempt < 3 ? "занято" : "СВОБОДНО!");
                    Console.ResetColor();
                }

                attempt++;
                Thread.Sleep(delayMs);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  ✓ Найдена свободная ячейка после {attempt} попыток!");
            Console.ResetColor();
        }
    }
}