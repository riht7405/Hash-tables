namespace HashTablesLab.Core.Enums
{
    /// <summary>
    /// Типы хеш-функций
    /// </summary>
    public enum HashMethodType
    {
        Division,           // Метод деления
        Multiplication,     // Метод умножения
        Custom1,           // Пользовательский метод 1
        Custom2,           // Пользовательский метод 2
        Custom3,           // Пользовательский метод 3
        Custom4,           // Пользовательский метод 4
        FNV,               // FNV-1a
        Murmur,            // MurmurHash
        City               // CityHash
    }

    /// <summary>
    /// Методы разрешения коллизий
    /// </summary>
    public enum CollisionResolutionType
    {
        Chaining,          // Метод цепочек
        LinearProbing,     // Линейное исследование
        QuadraticProbing,  // Квадратичное исследование
        DoubleHashing,     // Двойное хеширование
        Custom1,           // Пользовательский метод 1
        Custom2,           // Пользовательский метод 2
        RobinHood,         // Robin Hood hashing
        Cuckoo             // Cuckoo hashing
    }

    /// <summary>
    /// Режим работы хеш-таблицы
    /// </summary>
    public enum HashTableMode
    {
        Chaining,          // Цепочечный метод
        OpenAddressing     // Открытая адресация
    }

    /// <summary>
    /// Статус операции с хеш-таблицей
    /// </summary>
    public enum OperationStatus
    {
        Success,           // Успешно
        KeyExists,         // Ключ уже существует
        KeyNotFound,       // Ключ не найден
        TableFull,         // Таблица переполнена
        Collision,         // Произошла коллизия
        Error              // Ошибка
    }
}