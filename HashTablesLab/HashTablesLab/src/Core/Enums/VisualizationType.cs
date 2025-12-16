namespace HashTablesLab.Core.Enums
{
    /// <summary>
    /// Типы визуализации
    /// </summary>
    public enum VisualizationType
    {
        Console,           // Консольная визуализация
        Graphical,         // Графическая визуализация
        Text,             // Текстовая визуализация
        Chart,            // Графики и диаграммы
        Matrix,           // Матричное представление
        Heatmap           // Тепловая карта
    }

    /// <summary>
    /// Уровень детализации вывода
    /// </summary>
    public enum DetailLevel
    {
        Minimal,          // Минимальная информация
        Normal,           // Нормальный уровень
        Detailed,         // Подробный уровень
        Debug             // Отладочная информация
    }
}