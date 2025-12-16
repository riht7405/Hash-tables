using System;
using System.Linq;
using HashTablesLab.Core.Models;

namespace HashTablesLab.Visualization
{
    public static class HtmlVisualizer
    {
        public static string GenerateChainedReport(
            HashTablesLab.HashTables.ChainedHashTable<int, string> table,
            HashTablesLab.Core.Interfaces.IHashFunction<int> hashFunction,
            string title = "Отчет по хеш-таблице с цепочками")
        {
            var chainLengths = table.GetChainLengths();
            var stats = table.GetStatistics();

            string html = GenerateHtmlHeader(title);

            // Заголовок и мета-информация
            html += $@"
                <div class='container'>
                    <div class='header'>
                        <h1>{title}</h1>
                        <div class='metadata'>
                            <div>Дата: {DateTime.Now:dd.MM.yyyy HH:mm:ss}</div>
                            <div>Тип таблицы: Метод цепочек</div>
                            <div>Хеш-функция: {hashFunction.Name}</div>
                            <div>Размер таблицы: {chainLengths.Length} ячеек</div>
                            <div>Элементов: {table.Count}</div>
                        </div>
                    </div>";

            // Основные метрики
            html += GenerateMetricsSection(stats, true);

            // Визуализация распределения
            html += GenerateChainVisualization(chainLengths);

            // Гистограмма
            html += GenerateHistogram(chainLengths, "Распределение длин цепочек");

            // Анализ эффективности
            html += GenerateEfficiencyAnalysis(stats, true);

            // Закрытие контейнера
            html += @"
                </div>
            </body>
            </html>";

            return html;
        }

        public static string GenerateOpenAddressingReport(
            HashTablesLab.HashTables.OpenAddressingHashTable<int, string> table,
            HashTablesLab.Core.Interfaces.IHashFunction<int> hashFunction,
            HashTablesLab.Core.Interfaces.ICollisionResolver resolver,
            string title = "Отчет по хеш-таблице с открытой адресацией")
        {
            var occupancyMap = table.GetOccupancyMap();
            var stats = table.GetStatistics();

            string html = GenerateHtmlHeader(title);

            // Заголовок и мета-информация
            html += $@"
                <div class='container'>
                    <div class='header'>
                        <h1>{title}</h1>
                        <div class='metadata'>
                            <div>Дата: {DateTime.Now:dd.MM.yyyy HH:mm:ss}</div>
                            <div>Тип таблицы: Открытая адресация</div>
                            <div>Хеш-функция: {hashFunction.Name}</div>
                            <div>Метод разрешения: {resolver.Name}</div>
                            <div>Размер таблицы: {occupancyMap.Length} ячеек</div>
                            <div>Элементов: {table.Count}</div>
                        </div>
                    </div>";

            // Основные метрики
            html += GenerateMetricsSection(stats, false);

            // Визуализация кластеров
            html += GenerateClusterVisualization(occupancyMap);

            // Тепловая карта
            html += GenerateHeatmap(occupancyMap);

            // Анализ эффективности
            html += GenerateEfficiencyAnalysis(stats, false);

            // Закрытие контейнера
            html += @"
                </div>
            </body>
            </html>";

            return html;
        }

        private static string GenerateHtmlHeader(string title)
        {
            return $@"<!DOCTYPE html>
            <html lang='ru'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>{title}</title>
                <style>
                    /* Основные стили */
                    * {{
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }}
                    
                    body {{
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                        line-height: 1.6;
                        color: #333;
                        background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                        min-height: 100vh;
                        padding: 20px;
                    }}
                    
                    .container {{
                        max-width: 1200px;
                        margin: 0 auto;
                        background: white;
                        border-radius: 15px;
                        box-shadow: 0 20px 60px rgba(0,0,0,0.3);
                        overflow: hidden;
                    }}
                    
                    /* Шапка */
                    .header {{
                        background: linear-gradient(135deg, #4CAF50, #2E7D32);
                        color: white;
                        padding: 30px;
                        text-align: center;
                    }}
                    
                    .header h1 {{
                        font-size: 2.5rem;
                        margin-bottom: 15px;
                        text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
                    }}
                    
                    .metadata {{
                        display: flex;
                        justify-content: center;
                        flex-wrap: wrap;
                        gap: 20px;
                        margin-top: 20px;
                        font-size: 0.9rem;
                        opacity: 0.9;
                    }}
                    
                    .metadata div {{
                        background: rgba(255,255,255,0.2);
                        padding: 8px 15px;
                        border-radius: 20px;
                    }}
                    
                    /* Карточки с метриками */
                    .metrics-grid {{
                        display: grid;
                        grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
                        gap: 20px;
                        padding: 30px;
                        background: #f8f9fa;
                    }}
                    
                    .metric-card {{
                        background: white;
                        padding: 25px;
                        border-radius: 10px;
                        box-shadow: 0 5px 15px rgba(0,0,0,0.1);
                        transition: transform 0.3s;
                    }}
                    
                    .metric-card:hover {{
                        transform: translateY(-5px);
                    }}
                    
                    .metric-card h3 {{
                        color: #2E7D32;
                        margin-bottom: 15px;
                        font-size: 1.2rem;
                        border-bottom: 2px solid #4CAF50;
                        padding-bottom: 10px;
                    }}
                    
                    .metric-value {{
                        font-size: 2.5rem;
                        font-weight: bold;
                        color: #333;
                        margin: 10px 0;
                    }}
                    
                    .metric-description {{
                        color: #666;
                        font-size: 0.9rem;
                    }}
                    
                    /* Визуализация */
                    .visualization {{
                        padding: 30px;
                        background: white;
                        margin: 20px;
                        border-radius: 10px;
                        box-shadow: 0 5px 15px rgba(0,0,0,0.1);
                    }}
                    
                    .visualization h2 {{
                        color: #333;
                        margin-bottom: 20px;
                        font-size: 1.5rem;
                    }}
                    
                    /* Сетка ячеек */
                    .cell-grid {{
                        display: grid;
                        grid-template-columns: repeat(auto-fill, minmax(40px, 1fr));
                        gap: 8px;
                        margin: 20px 0;
                        max-height: 400px;
                        overflow-y: auto;
                        padding: 15px;
                        background: #f8f9fa;
                        border-radius: 8px;
                    }}
                    
                    .cell {{
                        width: 40px;
                        height: 40px;
                        display: flex;
                        align-items: center;
                        justify-content: center;
                        border-radius: 6px;
                        font-weight: bold;
                        font-size: 0.8rem;
                        transition: all 0.3s;
                        cursor: help;
                    }}
                    
                    .cell:hover {{
                        transform: scale(1.2);
                        box-shadow: 0 0 15px rgba(0,0,0,0.3);
                        z-index: 10;
                    }}
                    
                    /* Цвета ячеек */
                    .cell-0 {{ background: #e9ecef; color: #6c757d; border: 1px solid #dee2e6; }}
                    .cell-1 {{ background: #d4edda; color: #155724; border: 1px solid #c3e6cb; }}
                    .cell-2 {{ background: #fff3cd; color: #856404; border: 1px solid #ffeaa7; }}
                    .cell-3 {{ background: #f8d7da; color: #721c24; border: 1px solid #f5c6cb; }}
                    .cell-4 {{ background: #d1ecf1; color: #0c5460; border: 1px solid #bee5eb; }}
                    .cell-5 {{ background: #cce5ff; color: #004085; border: 1px solid #b8daff; }}
                    
                    /* Гистограмма */
                    .histogram {{
                        display: flex;
                        height: 300px;
                        align-items: flex-end;
                        gap: 2px;
                        margin: 30px 0;
                        padding: 20px;
                        background: #f8f9fa;
                        border-radius: 10px;
                    }}
                    
                    .bar {{
                        flex: 1;
                        background: linear-gradient(to top, #4CAF50, #8BC34A);
                        border-radius: 4px 4px 0 0;
                        position: relative;
                        min-width: 20px;
                    }}
                    
                    .bar-label {{
                        position: absolute;
                        bottom: -25px;
                        left: 0;
                        right: 0;
                        text-align: center;
                        font-size: 0.8rem;
                        color: #666;
                    }}
                    
                    .bar-value {{
                        position: absolute;
                        top: -25px;
                        left: 0;
                        right: 0;
                        text-align: center;
                        font-size: 0.8rem;
                        font-weight: bold;
                        color: #333;
                    }}
                    
                    /* Легенда */
                    .legend {{
                        display: flex;
                        justify-content: center;
                        flex-wrap: wrap;
                        gap: 20px;
                        margin: 20px 0;
                        padding: 15px;
                        background: #f8f9fa;
                        border-radius: 8px;
                    }}
                    
                    .legend-item {{
                        display: flex;
                        align-items: center;
                        gap: 8px;
                    }}
                    
                    .legend-color {{
                        width: 20px;
                        height: 20px;
                        border-radius: 4px;
                    }}
                    
                    /* Тепловая карта */
                    .heatmap {{
                        display: grid;
                        grid-template-columns: repeat(50, 1fr);
                        gap: 1px;
                        margin: 20px 0;
                        max-width: 800px;
                        margin-left: auto;
                        margin-right: auto;
                    }}
                    
                    .heatmap-cell {{
                        width: 15px;
                        height: 15px;
                        border-radius: 2px;
                    }}
                    
                    /* Анализ эффективности */
                    .analysis {{
                        padding: 25px;
                        margin: 20px;
                        border-radius: 10px;
                        border-left: 5px solid;
                    }}
                    
                    .analysis-good {{
                        background: #d4edda;
                        border-color: #28a745;
                    }}
                    
                    .analysis-warning {{
                        background: #fff3cd;
                        border-color: #ffc107;
                    }}
                    
                    .analysis-bad {{
                        background: #f8d7da;
                        border-color: #dc3545;
                    }}
                    
                    .analysis h3 {{
                        margin-bottom: 15px;
                        color: #333;
                    }}
                    
                    /* Адаптивность */
                    @media (max-width: 768px) {{
                        .metadata {{
                            flex-direction: column;
                            align-items: center;
                        }}
                        
                        .cell-grid {{
                            grid-template-columns: repeat(auto-fill, minmax(35px, 1fr));
                        }}
                        
                        .cell {{
                            width: 35px;
                            height: 35px;
                        }}
                        
                        .heatmap {{
                            grid-template-columns: repeat(25, 1fr);
                        }}
                    }}
                    
                    /* Анимации */
                    @keyframes fadeIn {{
                        from {{ opacity: 0; transform: translateY(20px); }}
                        to {{ opacity: 1; transform: translateY(0); }}
                    }}
                    
                    .metric-card, .visualization, .analysis {{
                        animation: fadeIn 0.5s ease-out;
                    }}
                    
                    /* Полоса прокрутки */
                    ::-webkit-scrollbar {{
                        width: 8px;
                        height: 8px;
                    }}
                    
                    ::-webkit-scrollbar-track {{
                        background: #f1f1f1;
                        border-radius: 4px;
                    }}
                    
                    ::-webkit-scrollbar-thumb {{
                        background: #4CAF50;
                        border-radius: 4px;
                    }}
                    
                    ::-webkit-scrollbar-thumb:hover {{
                        background: #2E7D32;
                    }}
                </style>
            </head>
            <body>";
        }

        private static string GenerateMetricsSection(Statistics stats, bool isChained)
        {
            string efficiencyColor = GetEfficiencyColorClass(stats, isChained);
            string efficiencyText = GetEfficiencyText(stats, isChained);

            return $@"
                <div class='metrics-grid'>
                    <div class='metric-card'>
                        <h3>📊 Коэффициент заполнения</h3>
                        <div class='metric-value'>{stats.LoadFactor:P2}</div>
                        <div class='metric-description'>Процент занятых ячеек от общего количества</div>
                    </div>
                    
                    <div class='metric-card'>
                        <h3>{(isChained ? "⛓️ Самая длинная цепочка" : "🔍 Самый длинный кластер")}</h3>
                        <div class='metric-value'>{GetMainMetric(stats, isChained)}</div>
                        <div class='metric-description'>{(isChained ? "Максимальное число элементов в одной ячейке" : "Максимальное число последовательных занятых ячеек")}</div>
                    </div>
                    
                    <div class='metric-card'>
                        <h3>📈 Производительность</h3>
                        <div class='metric-value'>{stats.InsertionTime.TotalMilliseconds:F0} мс</div>
                        <div class='metric-description'>Время вставки всех элементов</div>
                    </div>
                    
                    <div class='metric-card'>
                        <h3>🎯 Эффективность</h3>
                        <div class='metric-value {efficiencyColor}'>{efficiencyText}</div>
                        <div class='metric-description'>Оценка распределения элементов</div>
                    </div>
                </div>";
        }

        private static string GenerateChainVisualization(int[] chainLengths)
        {
            string html = @"
                <div class='visualization'>
                    <h2>🎨 Визуализация распределения цепочек</h2>
                    <p>Каждый квадрат представляет ячейку таблицы. Цвет показывает длину цепочки:</p>
                    
                    <div class='cell-grid'>";

            for (int i = 0; i < chainLengths.Length; i++)
            {
                int length = chainLengths[i];
                string className = $"cell cell-{Math.Min(length, 5)}";
                string title = $"Ячейка [{i}]: {length} элемент(ов)";

                html += $@"<div class='{className}' title='{title}'>{length}</div>";
            }

            html += @"
                    </div>
                    
                    <div class='legend'>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #e9ecef;'></div>
                            <span>Пусто (0)</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #d4edda;'></div>
                            <span>1 элемент</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #fff3cd;'></div>
                            <span>2 элемента</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #f8d7da;'></div>
                            <span>3 элемента</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #d1ecf1;'></div>
                            <span>4 элемента</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #cce5ff;'></div>
                            <span>5+ элементов</span>
                        </div>
                    </div>
                </div>";

            return html;
        }

        private static string GenerateClusterVisualization(bool[] occupancyMap)
        {
            // Находим кластеры
            int currentCluster = 0;
            int maxClusterSize = 0;
            var clusters = new System.Collections.Generic.List<int>();

            for (int i = 0; i < occupancyMap.Length; i++)
            {
                if (occupancyMap[i])
                {
                    currentCluster++;
                }
                else if (currentCluster > 0)
                {
                    clusters.Add(currentCluster);
                    if (currentCluster > maxClusterSize)
                        maxClusterSize = currentCluster;
                    currentCluster = 0;
                }
            }

            if (currentCluster > 0)
            {
                clusters.Add(currentCluster);
                if (currentCluster > maxClusterSize)
                    maxClusterSize = currentCluster;
            }

            string html = @"
                <div class='visualization'>
                    <h2>🔍 Визуализация кластеров</h2>
                    <p>Кластеры - это последовательные занятые ячейки. Идеальное распределение - короткие кластеры:</p>
                    
                    <div style='display: flex; flex-wrap: wrap; gap: 5px; margin: 20px 0;'>";

            foreach (var clusterSize in clusters)
            {
                string color = GetClusterColor(clusterSize);
                html += $@"<div style='background: {color}; color: white; padding: 5px 10px; border-radius: 4px; font-weight: bold;' title='Кластер из {clusterSize} ячеек'>{clusterSize}</div>";
            }

            html += $@"
                    </div>
                    
                    <div class='legend'>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #4CAF50;'></div>
                            <span>Короткие кластеры (1-3)</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #FFC107;'></div>
                            <span>Средние кластеры (4-7)</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #F44336;'></div>
                            <span>Длинные кластеры (8+)</span>
                        </div>
                    </div>
                    
                    <p style='margin-top: 20px;'>
                        <strong>Всего кластеров:</strong> {clusters.Count}<br>
                        <strong>Самый длинный кластер:</strong> {maxClusterSize} ячеек<br>
                        <strong>Средний размер кластера:</strong> {(clusters.Any() ? clusters.Average() : 0):F1} ячеек
                    </p>
                </div>";

            return html;
        }

        private static string GenerateHeatmap(bool[] occupancyMap)
        {
            string html = @"
                <div class='visualization'>
                    <h2>🔥 Тепловая карта заполнения</h2>
                    <p>Интенсивность цвета показывает заполненность области таблицы:</p>
                    
                    <div class='heatmap'>";

            int cellsPerBlock = Math.Max(1, occupancyMap.Length / 250); // 250 клеток для визуализации

            for (int i = 0; i < occupancyMap.Length; i += cellsPerBlock)
            {
                int occupiedCount = 0;
                for (int j = 0; j < cellsPerBlock && i + j < occupancyMap.Length; j++)
                {
                    if (occupancyMap[i + j])
                        occupiedCount++;
                }

                double ratio = (double)occupiedCount / cellsPerBlock;
                string color = GetHeatmapColor(ratio);

                html += $@"<div class='heatmap-cell' style='background: {color}' title='Заполнение: {ratio:P0}'></div>";
            }

            html += @"
                    </div>
                    
                    <div class='legend'>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #E0F7FA;'></div>
                            <span>0-20% заполнения</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #80DEEA;'></div>
                            <span>21-40% заполнения</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #4CAF50;'></div>
                            <span>41-60% заполнения</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #FF9800;'></div>
                            <span>61-80% заполнения</span>
                        </div>
                        <div class='legend-item'>
                            <div class='legend-color' style='background: #F44336;'></div>
                            <span>81-100% заполнения</span>
                        </div>
                    </div>
                </div>";

            return html;
        }

        private static string GenerateHistogram(int[] chainLengths, string title)
        {
            // Группируем по длине цепочки
            var maxLength = chainLengths.Max();
            var frequencies = new int[maxLength + 1];

            foreach (var length in chainLengths)
            {
                frequencies[length]++;
            }

            string html = $@"
                <div class='visualization'>
                    <h2>📈 {title}</h2>
                    <p>Распределение ячеек по длине цепочки:</p>
                    
                    <div class='histogram'>";

            int maxFrequency = frequencies.Max();

            for (int i = 0; i < frequencies.Length; i++)
            {
                int frequency = frequencies[i];
                if (maxFrequency > 0)
                {
                    double heightPercentage = (double)frequency / maxFrequency * 100;
                    html += $@"
                        <div class='bar' style='height: {heightPercentage}%' title='Длина {i}: {frequency} ячеек ({frequency * 100.0 / chainLengths.Length:F1}%)'>
                            <div class='bar-value'>{frequency}</div>
                            <div class='bar-label'>{i}</div>
                        </div>";
                }
            }

            html += $@"
                    </div>
                    
                    <div style='margin-top: 20px;'>
                        <p><strong>Статистика распределения:</strong></p>
                        <ul style='margin-left: 20px;'>
                            <li>Всего ячеек: {chainLengths.Length}</li>
                            <li>Пустых ячеек: {frequencies[0]} ({frequencies[0] * 100.0 / chainLengths.Length:F1}%)</li>
                            <li>Ячеек с 1 элементом: {frequencies[1]} ({frequencies[1] * 100.0 / chainLengths.Length:F1}%)</li>
                            <li>Средняя длина цепочки: {chainLengths.Average():F2}</li>
                        </ul>
                    </div>
                </div>";

            return html;
        }

        private static string GenerateEfficiencyAnalysis(Statistics stats, bool isChained)
        {
            string efficiencyClass = GetEfficiencyColorClass(stats, isChained);
            string analysisClass = efficiencyClass switch
            {
                "analysis-good" => "analysis-good",
                "analysis-warning" => "analysis-warning",
                _ => "analysis-bad"
            };

            string mainMetric = GetMainMetric(stats, isChained).ToString();
            string metricName = isChained ? "самой длинной цепочки" : "самого длинного кластера";
            string recommendation = GetRecommendation(stats, isChained);

            return $@"
                <div class='analysis {analysisClass}'>
                    <h3>📋 Анализ эффективности</h3>
                    
                    <h4>📊 Результаты:</h4>
                    <ul style='margin-left: 20px; margin-bottom: 15px;'>
                        <li>Коэффициент заполнения: <strong>{stats.LoadFactor:P2}</strong></li>
                        <li>Длина {metricName}: <strong>{mainMetric}</strong></li>
                        <li>Пустых ячеек: <strong>{stats.EmptyBuckets}</strong></li>
                        <li>Время вставки: <strong>{stats.InsertionTime.TotalMilliseconds:F0} мс</strong></li>
                    </ul>
                    
                    <h4>🎯 Оценка:</h4>
                    <p>{GetEfficiencyText(stats, isChained)}</p>
                    
                    <h4>💡 Рекомендации:</h4>
                    <p>{recommendation}</p>
                    
                    <h4>📖 Критерии оценки:</h4>
                    <p>
                        <strong>Отлично (зеленый):</strong><br>
                        {(isChained ?
                            "• Длина цепочки ≤ 3<br>• Равномерное распределение<br>• Мало пустых ячеек" :
                            "• Длина кластера ≤ 5<br>• Короткие кластеры<br>• Равномерное заполнение")}
                    </p>
                    <p>
                        <strong>Удовлетворительно (желтый):</strong><br>
                        {(isChained ?
                            "• Длина цепочки 4-7<br>• Умеренное распределение<br>• Некоторые длинные цепочки" :
                            "• Длина кластера 6-10<br>• Средние кластеры<br>• Возможна оптимизация")}
                    </p>
                    <p>
                        <strong>Требует оптимизации (красный):</strong><br>
                        {(isChained ?
                            "• Длина цепочки ≥ 8<br>• Неравномерное распределение<br>• Много длинных цепочек" :
                            "• Длина кластера ≥ 11<br>• Длинные кластеры<br>• Сильная кластеризация")}
                    </p>
                </div>";
        }

        // Вспомогательные методы
        private static int GetMainMetric(Statistics stats, bool isChained)
        {
            return isChained ? stats.LongestChain : stats.LongestCluster;
        }

        private static string GetEfficiencyColorClass(Statistics stats, bool isChained)
        {
            int metric = GetMainMetric(stats, isChained);

            if (isChained)
            {
                return metric switch
                {
                    <= 3 => "analysis-good",
                    <= 7 => "analysis-warning",
                    _ => "analysis-bad"
                };
            }
            else
            {
                return metric switch
                {
                    <= 5 => "analysis-good",
                    <= 10 => "analysis-warning",
                    _ => "analysis-bad"
                };
            }
        }

        private static string GetEfficiencyText(Statistics stats, bool isChained)
        {
            int metric = GetMainMetric(stats, isChained);

            if (isChained)
            {
                return metric switch
                {
                    <= 3 => "Отличное распределение! Хеш-функция работает эффективно.",
                    <= 5 => "Хорошее распределение. Короткие цепочки обеспечивают быстрый поиск.",
                    <= 7 => "Удовлетворительное распределение. Некоторые цепочки требуют оптимизации.",
                    _ => "Требуется оптимизация. Длинные цепочки замедляют поиск элементов."
                };
            }
            else
            {
                return metric switch
                {
                    <= 3 => "Идеальное распределение! Минимальная кластеризация.",
                    <= 5 => "Отличное распределение. Короткие кластеры обеспечивают быстрый поиск.",
                    <= 8 => "Хорошее распределение. Умеренная кластеризация допустима.",
                    <= 12 => "Удовлетворительное распределение. Рекомендуется оптимизация.",
                    _ => "Требуется оптимизация. Длинные кластеры значительно замедляют поиск."
                };
            }
        }

        private static string GetRecommendation(Statistics stats, bool isChained)
        {
            int metric = GetMainMetric(stats, isChained);

            if (isChained)
            {
                return metric switch
                {
                    <= 3 => "Продолжайте использовать текущую хеш-функцию. Она демонстрирует отличные результаты.",
                    <= 5 => "Текущая хеш-функция работает хорошо. Для дополнительной оптимизации можно рассмотреть метод умножения или Custom 3.",
                    <= 7 => "Рекомендуется протестировать другие хеш-функции (Custom 2, Custom 4) для улучшения распределения.",
                    _ => "Необходимо выбрать другую хеш-функцию. Рекомендуется метод умножения или Custom 3 для более равномерного распределения."
                };
            }
            else
            {
                return metric switch
                {
                    <= 3 => "Метод разрешения коллизий работает отлично. Продолжайте использовать.",
                    <= 5 => "Хорошие результаты. Для дополнительной оптимизации можно попробовать двойное хеширование.",
                    <= 8 => "Рекомендуется протестировать квадратичное или двойное хеширование для уменьшения кластеризации.",
                    _ => "Сильная кластеризация. Рекомендуется использовать двойное хеширование или рассмотреть метод цепочек для данных параметров."
                };
            }
        }

        private static string GetClusterColor(int clusterSize)
        {
            return clusterSize switch
            {
                <= 3 => "#4CAF50",   // Зеленый
                <= 7 => "#FFC107",   // Желтый
                _ => "#F44336"       // Красный
            };
        }

        private static string GetHeatmapColor(double ratio)
        {
            return ratio switch
            {
                < 0.2 => "#E0F7FA",  // Очень светлый голубой
                < 0.4 => "#80DEEA",  // Светлый голубой
                < 0.6 => "#4CAF50",  // Зеленый
                < 0.8 => "#FF9800",  // Оранжевый
                _ => "#F44336"       // Красный
            };
        }

        public static void SaveHtmlReport(string html, string filePath = "hash_table_report.html")
        {
            try
            {
                System.IO.File.WriteAllText(filePath, html, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения HTML-отчета: {ex.Message}");
            }
        }
    }
}