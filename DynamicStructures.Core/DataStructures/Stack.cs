using System.Runtime.InteropServices;

namespace DynamicStructures.Core.DataStructures;

public class Node
{
    public string? Data;
    public Node next;
}
public class Stack
{
    
    public static void Push(ref Node top, string value)
    {
        var elem =  new Node();
        elem.next = top;
        elem.Data = value;
        top =  elem;
        
    }
    public static void Print(Node top)
    {
        var current = top;
        while (current.next != null)
        {
            Console.WriteLine(current.Data);
            current = current.next;
        }
        
    }

    public static string Pop(ref Node top, int index)
    {
        int current_number = 0;
        var current = top;
        while (current_number <= index && current.next != null)
        {
            if (current_number == index)
            {
                top = top.next;
                break;
            }
            current = current.next;
            current_number++;
            
        }
        return current.Data;
    }

    public static string Top(Node top, int index)
    {
        if (top == null) return "null";
        return  top.Data;
    }
    public static bool IsEmpty(Node top)
    {
        return top == null;
    }
    public static void LoadFile(ref Node top)
{
    string? s = "";
    using (var fileStream = new StreamReader("input.txt"))
    {
        s = fileStream.ReadLine();
    }
    
    if (string.IsNullOrWhiteSpace(s))
    {
        Console.WriteLine("Файл пуст или строка не найдена.");
        return;
    }

    string[] commands = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    foreach (string command in commands)
    {
        if (command.StartsWith("1,"))
        {
            string[] parts = command.Split(',');
            if (parts.Length > 1)
            {
                string element = parts[1];
                Push(ref top, element);
                Console.WriteLine($"Push({element}) -> Элемент добавлен");
            }
        }
        else
        {
            switch (command)
            {
                case "2": // Pop()
                    try
                    {
                        string popped = Pop(ref top, 0); 
                        Console.WriteLine($"Pop() -> Извлечен элемент: {popped}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Pop() -> Ошибка: {ex.Message}");
                    }
                    break;

                case "3": // Top()
                    if (top != null)
                        Console.WriteLine($"Top() -> На вершине: {top.Data}");
                    else
                        Console.WriteLine("Top() -> Стек пуст");
                    break;

                case "4": // isEmpty()
                    bool empty = (top == null);
                    Console.WriteLine($"isEmpty() -> {empty}");
                    break;

                case "5": // Print()
                    Console.Write("Print() -> Текущий стек (от вершины к основанию): ");
                    Print(top);
                    break;

                default:
                    Console.WriteLine($"Неизвестная команда: {command}");
                    break;
            }
        }
    }
}

}