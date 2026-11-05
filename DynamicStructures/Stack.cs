using System.Diagnostics;

namespace DynamicStructures;

/// <summary>
/// Узел односвязного списка для стека строк
/// </summary>
public class Node
{
    public string? Data;
    public Node? next;

    public Node(string? data = null, Node? next = null)
    {
        Data = data;
        this.next = next;
    }
}

/// <summary>
/// Реализация стека на базе собственного связного списка.
/// </summary>
public class Stack
{
    /// <summary>
    /// Помещение элемента на вершину стека: O(1)
    /// </summary>
    public static void Push(ref Node? top, string value)
    {
        var elem = new Node(value, top);
        top = elem;
    }

    /// <summary>
    /// Извлечение элемента с вершины стека: O(1)
    /// </summary>
    public static string Pop(ref Node? top)
    {
        if (top == null)
        {
            throw new InvalidOperationException("Стек пуст! Невозможно извлечь элемент.");
        }

        string result = top.Data ?? "";
        top = top.next;
        return result;
    }

    /// <summary>
    /// Совместимая перегрузка Pop с игнорированием индекса (стек извлекает всегда с вершины)
    /// </summary>
    public static string Pop(ref Node? top, int index)
    {
        return Pop(ref top);
    }

    /// <summary>
    /// Просмотр элемента на вершине стека без удаления: O(1)
    /// </summary>
    public static string? Top(Node? top)
    {
        if (top == null) return null;
        return top.Data;
    }

    /// <summary>
    /// Перегрузка Top для совместимости
    /// </summary>
    public static string Top(Node? top, int index)
    {
        return Top(top) ?? "null";
    }

    /// <summary>
    /// Проверка стека на пустоту: O(1)
    /// </summary>
    public static bool IsEmpty(Node? top)
    {
        return top == null;
    }

    /// <summary>
    /// Вывод элементов стека от вершины к основанию: O(N)
    /// </summary>
    public static void Print(Node? top)
    {
        if (top == null)
        {
            Console.WriteLine("[пусто]");
            return;
        }

        var elements = new List<string>();
        var current = top;
        while (current != null)
        {
            elements.Add(current.Data ?? "null");
            current = current.next;
        }

        Console.WriteLine(string.Join(" -> ", elements));
    }

    /// <summary>
    /// Считывание команд из файла input.txt и замер времени выполнения
    /// </summary>
    public static void LoadFile(ref Node? top, string? filePath = null)
    {
        filePath ??= FindInputFile("input.txt");

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Файл '{filePath}' не найден!");
            return;
        }

        Console.WriteLine($"\n--- Чтение команд из файла: {Path.GetFileName(filePath)} ---");

        var stopwatch = Stopwatch.StartNew();

        string content = File.ReadAllText(filePath);
        string[] commands = content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        stopwatch.Stop();
        long readTimeTicks = stopwatch.ElapsedTicks;
        double readTimeMs = stopwatch.Elapsed.TotalMilliseconds;

        Console.WriteLine($"Команды в файле: {string.Join(" ", commands)}");
        Console.WriteLine($"Время считывания файла: {readTimeMs:F4} мс ({readTimeTicks} тиков)\n");

        stopwatch.Restart();

        int operationCount = 0;
        foreach (string command in commands)
        {
            operationCount++;
            if (command.StartsWith("1,"))
            {
                int commaIndex = command.IndexOf(',');
                string element = command.Substring(commaIndex + 1);
                Push(ref top, element);
                Console.WriteLine($"[1] Push({element}) -> Элемент успешно добавлен на вершину");
            }
            else
            {
                switch (command)
                {
                    case "2": // Pop()
                        try
                        {
                            string popped = Pop(ref top);
                            Console.WriteLine($"[2] Pop() -> Извлечен элемент: '{popped}'");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"[2] Pop() -> Ошибка: {ex.Message}");
                        }
                        break;

                    case "3": // Top()
                        if (top != null)
                            Console.WriteLine($"[3] Top() -> На вершине: '{top.Data}'");
                        else
                            Console.WriteLine("[3] Top() -> Стек пуст");
                        break;

                    case "4": // isEmpty()
                        bool empty = IsEmpty(top);
                        Console.WriteLine($"[4] isEmpty() -> {empty}");
                        break;

                    case "5": // Print()
                        Console.Write("[5] Print() -> Текущий стек (вершина -> дно): ");
                        Print(top);
                        break;

                    default:
                        Console.WriteLine($"[?] Неизвестная команда: {command}");
                        break;
                }
            }
        }

        stopwatch.Stop();
        Console.WriteLine($"\nВыполнено операций: {operationCount}");
        Console.WriteLine($"Время выполнения операций: {stopwatch.Elapsed.TotalMilliseconds:F4} мс ({stopwatch.ElapsedTicks} тиков)");
        Console.WriteLine("---------------------------------------------------\n");
    }

    /// <summary>
    /// Поиск файла с учетом рабочей директории и папки проекта
    /// </summary>
    public static string FindInputFile(string filename)
    {
        if (File.Exists(filename)) return filename;

        string appDir = AppContext.BaseDirectory;
        string appPath = Path.Combine(appDir, filename);
        if (File.Exists(appPath)) return appPath;

        // Поиск в родительских папках
        var dir = new DirectoryInfo(Directory.GetCurrentDirectory());
        for (int i = 0; i < 4 && dir != null; i++)
        {
            string candidate = Path.Combine(dir.FullName, filename);
            if (File.Exists(candidate)) return candidate;
            string candidateSub = Path.Combine(dir.FullName, "DynamicStructures", filename);
            if (File.Exists(candidateSub)) return candidateSub;
            dir = dir.Parent;
        }

        return filename;
    }
}

/// <summary>
/// Модуль бенчмаркинга операций стека:
/// 1. Замеры времени считывания и выполнения для наборов разной длины
/// 2. Замеры времени для наборов одинаковой длины, но разного состава операций
/// </summary>
public static class StackBenchmark
{
    public static (double readMs, long readTicks, double execMs, long execTicks, int ops) ExecuteFile(string filePath, bool verbose = false)
    {
        string path = Stack.FindInputFile(filePath);
        if (!File.Exists(path))
            return (0, 0, 0, 0, 0);

        var sw = Stopwatch.StartNew();
        string content = File.ReadAllText(path);
        string[] commands = content.Split(new[] { ' ', '\r', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        sw.Stop();
        double readMs = sw.Elapsed.TotalMilliseconds;
        long readTicks = sw.ElapsedTicks;

        Node? top = null;
        sw.Restart();
        int ops = 0;
        foreach (var cmd in commands)
        {
            ops++;
            if (cmd.StartsWith("1,"))
            {
                int commaIndex = cmd.IndexOf(',');
                string elem = cmd.Substring(commaIndex + 1);
                Stack.Push(ref top, elem);
                if (verbose) Console.WriteLine($"[1] Push({elem})");
            }
            else
            {
                switch (cmd)
                {
                    case "2":
                        try
                        {
                            string popped = Stack.Pop(ref top);
                            if (verbose) Console.WriteLine($"[2] Pop() -> '{popped}'");
                        }
                        catch {}
                        break;

                    case "3":
                        string? t = Stack.Top(top);
                        if (verbose) Console.WriteLine($"[3] Top() -> '{t}'");
                        break;

                    case "4":
                        bool empty = Stack.IsEmpty(top);
                        if (verbose) Console.WriteLine($"[4] isEmpty() -> {empty}");
                        break;

                    case "5":
                        if (verbose)
                        {
                            Console.Write("[5] Print() -> ");
                            Stack.Print(top);
                        }
                        else
                        {
                            // В режиме бенчмарка симулируем обход связного списка O(N) без накладных расходов вывода консоли
                            var curr = top;
                            while (curr != null) curr = curr.next;
                        }
                        break;
                }
            }
        }
        sw.Stop();
        return (readMs, readTicks, sw.Elapsed.TotalMilliseconds, sw.ElapsedTicks, ops);
    }

    /// <summary>
    /// Автоматический прогон полного набора замеров времени для стека
    /// (различные размеры и различные составы операций)
    /// </summary>
    public static void RunFullBenchmarkSuite()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n========================================================================");
        Console.WriteLine("    ЭКСПЕРИМЕНТАЛЬНЫЙ БЕНЧМАРК СТЕКА: РАЗНЫЕ РАЗМЕРЫ И СОСТАВЫ ОПЕРАЦИЙ ");
        Console.WriteLine("========================================================================");
        Console.ResetColor();

        string tempBenchFile = "bench_temp_stack.txt";

        // Серия 1: Различные по длине наборы (Сбалансированные команды)
        Console.WriteLine("\n--- СЕРИЯ 1: НАБОРЫ РАЗЛИЧНОЙ ДЛИНЫ (Сбалансированный поток 1..5) ---");
        int[] lengths = { 100, 500, 1000, 5000, 10000 };

        Console.WriteLine("{0,-8} | {1,-22} | {2,-22}", "N", "Считывание файла", "Выполнение операций");
        Console.WriteLine("{0,-8} | {1,-10} {2,-11} | {3,-10} {4,-11}", "", "Время (мс)", "Тики", "Время (мс)", "Тики");
        Console.WriteLine(new string('-', 62));

        foreach (int n in lengths)
        {
            GenerateBenchmarkFile(tempBenchFile, n, pPush: 0.45, pPop: 0.35, pTop: 0.10, pEmpty: 0.05, pPrint: 0.05);

            // Прогрев JIT
            ExecuteFile(tempBenchFile, verbose: false);

            var res = ExecuteFile(tempBenchFile, verbose: false);
            Console.WriteLine("{0,-8} | {1,-10:F4} {2,-11} | {3,-10:F4} {4,-11}", n, res.readMs, res.readTicks, res.execMs, res.execTicks);
        }

        // Серия 2: Одинаковая длина (N = 5000), но кардинально разный состав операций
        Console.WriteLine("\n--- СЕРИЯ 2: ОДИНАКОВАЯ ДЛИНА (N = 5000), РАЗНЫЙ СОСТАВ ОПЕРАЦИЙ ---");
        Console.WriteLine("(Демонстрация влияния характера нагрузки на время выполнения)");
        Console.WriteLine(new string('-', 72));

        var scenarios = new (string name, string desc, double push, double pop, double top, double empty, double print)[]
        {
            ("Набор А", "Push-heavy (85% Push, 10% Pop, 5% инспекция)", 0.85, 0.10, 0.03, 0.02, 0.00),
            ("Набор Б", "Сбалансированный поток (48% Push, 48% Pop, 4% инспекция)", 0.48, 0.48, 0.02, 0.02, 0.00),
            ("Набор В", "Read-heavy (10% Push, 10% Pop, 40% Top, 40% isEmpty)", 0.10, 0.10, 0.40, 0.40, 0.00),
            ("Набор Г", "Print-heavy (35% Push, 35% Pop, 15% Print, 15% инспекция)", 0.35, 0.35, 0.10, 0.05, 0.15)
        };

        Console.WriteLine("{0,-10} | {1,-18} | {2,-18} | {3}", "Сценарий", "Считывание (мс)", "Выполнение (мс)", "Описание нагрузки");
        Console.WriteLine(new string('-', 85));

        foreach (var sc in scenarios)
        {
            GenerateBenchmarkFile(tempBenchFile, 5000, sc.push, sc.pop, sc.top, sc.empty, sc.print);

            var res = ExecuteFile(tempBenchFile, verbose: false);
            Console.WriteLine("{0,-10} | {1,-18:F4} | {2,-18:F4} | {3}", sc.name, res.readMs, res.execMs, sc.desc);
        }

        if (File.Exists(tempBenchFile))
            File.Delete(tempBenchFile);

        Console.WriteLine("\n========================================================================\n");
    }

    private static void GenerateBenchmarkFile(string path, int n, double pPush, double pPop, double pTop, double pEmpty, double pPrint)
    {
        var rnd = new Random(42);
        var cmds = new List<string>(n);

        for (int i = 0; i < n; i++)
        {
            double r = rnd.NextDouble();
            if (r < pPush)
            {
                cmds.Add($"1,{rnd.Next(1, 1000)}");
            }
            else if (r < pPush + pPop)
            {
                cmds.Add("2");
            }
            else if (r < pPush + pPop + pTop)
            {
                cmds.Add("3");
            }
            else if (r < pPush + pPop + pTop + pEmpty)
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
