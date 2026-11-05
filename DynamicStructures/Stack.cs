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
