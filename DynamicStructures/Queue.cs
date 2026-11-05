using System.Diagnostics;

namespace DynamicStructures;

/// <summary>
/// Реализация очереди на базе собственного односвязного списка.
/// Поддерживает ссылки на голову (head) для O(1) Dequeue и хвост (tail) для O(1) Enqueue.
/// </summary>
public class CustomQueue
{
    private Node? _head;
    private Node? _tail;
    private int _count;

    public int Count => _count;

    /// <summary>
    /// Вставка элемента в конец очереди (хвост): O(1)
    /// </summary>
    public void Enqueue(string value)
    {
        var newNode = new Node(value, null);
        if (_tail == null)
        {
            _head = newNode;
            _tail = newNode;
        }
        else
        {
            _tail.next = newNode;
            _tail = newNode;
        }
        _count++;
    }

    /// <summary>
    /// Удаление и возврат элемента из начала очереди (голова): O(1)
    /// </summary>
    public string Dequeue()
    {
        if (_head == null)
            throw new InvalidOperationException("Очередь пуста! Невозможно удалить элемент.");

        string data = _head.Data ?? "";
        _head = _head.next;
        if (_head == null)
            _tail = null;

        _count--;
        return data;
    }

    /// <summary>
    /// Просмотр элемента в начале очереди без удаления: O(1)
    /// </summary>
    public string? Peek()
    {
        if (_head == null) return null;
        return _head.Data;
    }

    /// <summary>
    /// Проверка на пустоту: O(1)
    /// </summary>
    public bool IsEmpty() => _head == null;

    /// <summary>
    /// Очистка очереди
    /// </summary>
    public void Clear()
    {
        _head = null;
        _tail = null;
        _count = 0;
    }

    /// <summary>
    /// Печать элементов от начала (головы) к концу (хвосту): O(N)
    /// </summary>
    public void Print()
    {
        if (_head == null)
        {
            Console.WriteLine("[Очередь пуста]");
            return;
        }

        var elements = new List<string>();
        var curr = _head;
        while (curr != null)
        {
            elements.Add(curr.Data ?? "null");
            curr = curr.next;
        }
        Console.WriteLine(string.Join(" -> ", elements));
    }
}

/// <summary>
/// Модуль для части 2 лабораторной работы:
/// 1. Обработка команд input.txt для собственной очереди и Queue<string>
/// 2. Замер времени выполнения и сравнительный бенчмаркинг
/// </summary>
public static class QueueTaskRunner
{
    /// <summary>
    /// Считывание файла и выполнение команд на собственной очереди
    /// </summary>
    public static (double readMs, double execMs, int ops) RunCustomQueue(string filePath, bool verbose = true)
    {
        var queue = new CustomQueue();
        return ExecuteCommands(filePath, 
            onEnqueue: elem => queue.Enqueue(elem),
            onDequeue: () => queue.Dequeue(),
            onPeek: () => queue.Peek(),
            onIsEmpty: () => queue.IsEmpty(),
            onPrint: () => queue.Print(),
            name: "Своя очередь (CustomQueue)",
            verbose: verbose);
    }

    /// <summary>
    /// Считывание файла и выполнение команд на стандартном классе Queue<string>
    /// </summary>
    public static (double readMs, double execMs, int ops) RunStandardQueue(string filePath, bool verbose = true)
    {
        var queue = new Queue<string>();
        return ExecuteCommands(filePath,
            onEnqueue: elem => queue.Enqueue(elem),
            onDequeue: () => queue.Count > 0 ? queue.Dequeue() : throw new InvalidOperationException("Очередь пуста!"),
            onPeek: () => queue.Count > 0 ? queue.Peek() : null,
            onIsEmpty: () => queue.Count == 0,
            onPrint: () =>
            {
                if (queue.Count == 0) Console.WriteLine("[Очередь пуста]");
                else Console.WriteLine(string.Join(" -> ", queue));
            },
            name: "Стандартная Queue<string>",
            verbose: verbose);
    }

    private static (double readMs, double execMs, int ops) ExecuteCommands(
        string filePath,
        Action<string> onEnqueue,
        Func<string> onDequeue,
        Func<string?> onPeek,
        Func<bool> onIsEmpty,
        Action onPrint,
        string name,
        bool verbose)
    {
        string path = Stack.FindInputFile(filePath);
        if (!File.Exists(path))
        {
            if (verbose) Console.WriteLine($"Файл '{path}' не найден!");
            return (0, 0, 0);
        }

        var sw = Stopwatch.StartNew();
        string text = File.ReadAllText(path);
        string[] commands = text.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        sw.Stop();
        double readMs = sw.Elapsed.TotalMilliseconds;

        if (verbose)
        {
            Console.WriteLine($"\n--- [{name}] Чтение файла '{Path.GetFileName(path)}' ---");
            Console.WriteLine($"Время считывания: {readMs:F4} мс");
        }

        sw.Restart();
        int ops = 0;
        foreach (var cmd in commands)
        {
            ops++;
            if (cmd.StartsWith("1,"))
            {
                string elem = cmd.Substring(cmd.IndexOf(',') + 1);
                onEnqueue(elem);
                if (verbose) Console.WriteLine($"[1] Вставка({elem}) -> Добавлен в конец");
            }
            else
            {
                switch (cmd)
                {
                    case "2":
                        try
                        {
                            string removed = onDequeue();
                            if (verbose) Console.WriteLine($"[2] Удаление() -> Извлечен: '{removed}'");
                        }
                        catch (Exception ex)
                        {
                            if (verbose) Console.WriteLine($"[2] Удаление() -> Ошибка: {ex.Message}");
                        }
                        break;

                    case "3":
                        string? first = onPeek();
                        if (verbose)
                        {
                            if (first != null) Console.WriteLine($"[3] Начало очереди: '{first}'");
                            else Console.WriteLine("[3] Начало очереди: Очередь пуста");
                        }
                        break;

                    case "4":
                        bool empty = onIsEmpty();
                        if (verbose) Console.WriteLine($"[4] isEmpty() -> {empty}");
                        break;

                    case "5":
                        if (verbose)
                        {
                            Console.Write("[5] Печать() -> Очередь (голова -> хвост): ");
                            onPrint();
                        }
                        break;

                    default:
                        if (verbose) Console.WriteLine($"[?] Неизвестная команда: {cmd}");
                        break;
                }
            }
        }
        sw.Stop();
        double execMs = sw.Elapsed.TotalMilliseconds;

        if (verbose)
        {
            Console.WriteLine($"Выполнено операций: {ops}");
            Console.WriteLine($"Время выполнения: {execMs:F4} мс\n");
        }

        return (readMs, execMs, ops);
    }

    /// <summary>
    /// Запуск обеих реализаций на одном и том же файле с выводом сравнения
    /// </summary>
    public static void CompareBothOnInputFile(string filePath = "input.txt")
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n========================================================================");
        Console.WriteLine("    СРАВНИТЕЛЬНЫЙ ЗАПУСК ОБЕИХ РЕАЛИЗАЦИЙ ОЧЕРЕДИ НА ФАЙЛЕ INPUT.TXT    ");
        Console.WriteLine("========================================================================");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        var custom = RunCustomQueue(filePath, verbose: true);
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        var std = RunStandardQueue(filePath, verbose: true);
        Console.ResetColor();

        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine($"ИТОГИ СРАВНЕНИЯ ДЛЯ {custom.ops} ОПЕРАЦИЙ:");
        Console.WriteLine($"1. Своя очередь (CustomQueue):      Считывание: {custom.readMs:F4} мс | Выполнение: {custom.execMs:F4} мс");
        Console.WriteLine($"2. Стандартный класс Queue<string>: Считывание: {std.readMs:F4} мс | Выполнение: {std.execMs:F4} мс");
        Console.WriteLine("------------------------------------------------------------------------\n");
    }

    /// <summary>
    /// Автоматическое построение замеров времени для различных наборов операций,
    /// как того требует методичка:
    /// 1) Различные по длине наборы
    /// 2) Одинаковые по длине, но различные по составу операций (для демонстрации максимального расхождения)
    /// </summary>
    public static void RunFullBenchmarkSuite()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n========================================================================");
        Console.WriteLine("    ЭКСПЕРИМЕНТАЛЬНЫЙ БЕНЧМАРК ОЧЕРЕДИ: СВОЯ VS СТАНДАРТНАЯ QUEUE       ");
        Console.WriteLine("========================================================================");
        Console.ResetColor();

        string tempBenchFile = "bench_temp_queue.txt";

        // Серия 1: Различные по длине наборы
        Console.WriteLine("\n--- СЕРИЯ 1: НАБОРЫ РАЗЛИЧНОЙ ДЛИНЫ (Сбалансированные операции 1..5) ---");
        int[] lengths = { 100, 500, 1000, 5000, 10000 };

        Console.WriteLine("{0,-8} | {1,-20} | {2,-20} | {3,-15}", "N", "Своя очередь (мс)", "Стандартная Queue", "Разница (раз)");
        Console.WriteLine(new string('-', 72));

        foreach (int n in lengths)
        {
            GenerateBenchmarkFile(tempBenchFile, n, pEnqueue: 0.4, pDequeue: 0.3, pPeek: 0.15, pIsEmpty: 0.1, pPrint: 0.05);

            // Прогрев JIT
            RunCustomQueue(tempBenchFile, verbose: false);
            RunStandardQueue(tempBenchFile, verbose: false);

            var c = RunCustomQueue(tempBenchFile, verbose: false);
            var s = RunStandardQueue(tempBenchFile, verbose: false);

            double ratio = s.execMs > 0 ? c.execMs / s.execMs : 1.0;
            Console.WriteLine("{0,-8} | {1,-20:F4} | {2,-20:F4} | x{3,-14:F2}", n, c.execMs, s.execMs, ratio);
        }

        // Серия 2: Одинаковая длина (N = 5000), но кардинально разный состав операций
        Console.WriteLine("\n--- СЕРИЯ 2: ОДИНАКОВАЯ ДЛИНА (N = 5000), РАЗНЫЙ СОСТАВ ОПЕРАЦИЙ ---");
        Console.WriteLine("(Демонстрация максимального расхождения времени выполнения)");
        Console.WriteLine(new string('-', 75));

        var scenarios = new (string name, double enq, double deq, double peek, double empty, double print)[]
        {
            ("Набор А: Преобладание Enqueue (90% вставки)", 0.90, 0.08, 0.01, 0.01, 0.00),
            ("Набор Б: Быстрые Peek/IsEmpty (80% просмотр)", 0.10, 0.10, 0.40, 0.40, 0.00),
            ("Набор В: Частая печать Print (15% O(N) обход)", 0.40, 0.35, 0.05, 0.05, 0.15)
        };

        foreach (var sc in scenarios)
        {
            GenerateBenchmarkFile(tempBenchFile, 5000, sc.enq, sc.deq, sc.peek, sc.empty, sc.print);

            var c = RunCustomQueue(tempBenchFile, verbose: false);
            var s = RunStandardQueue(tempBenchFile, verbose: false);

            double diff = Math.Abs(c.execMs - s.execMs);
            Console.WriteLine($"\n{sc.name}:");
            Console.WriteLine($"   • Своя очередь:      {c.execMs:F4} мс");
            Console.WriteLine($"   • Стандартная Queue: {s.execMs:F4} мс");
            Console.WriteLine($"   • Разница во времени: {diff:F4} мс (в {Math.Max(c.execMs, s.execMs) / Math.Max(0.0001, Math.Min(c.execMs, s.execMs)):F2} раз)");
        }

        if (File.Exists(tempBenchFile))
            File.Delete(tempBenchFile);

        Console.WriteLine("\n========================================================================\n");
    }

    private static void GenerateBenchmarkFile(string path, int n, double pEnqueue, double pDequeue, double pPeek, double pIsEmpty, double pPrint)
    {
        var rnd = new Random(42);
        var cmds = new List<string>(n);

        for (int i = 0; i < n; i++)
        {
            double r = rnd.NextDouble();
            if (r < pEnqueue)
            {
                cmds.Add($"1,{rnd.Next(1, 1000)}");
            }
            else if (r < pEnqueue + pDequeue)
            {
                cmds.Add("2");
            }
            else if (r < pEnqueue + pDequeue + pPeek)
            {
                cmds.Add("3");
            }
            else if (r < pEnqueue + pDequeue + pPeek + pIsEmpty)
            {
                cmds.Add("4");
            }
            else
            {
                cmds.Add("5");
            }
        }

        File.WriteAllText(path, string.Join(" ", cmds));
    }
}
