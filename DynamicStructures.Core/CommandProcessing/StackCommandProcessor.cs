
using DynamicStructures.Core.DataStructures;

namespace DynamicStructures.Core.CommandProcessing;

public class StackCommandProcessor
{
    public List<string> Process(string input)
    {
        CustomStack<string> stack = new();
        List<string> results = new();

        string[] commands = input.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries
        );

        foreach (string command in commands)
        {
            if (command.StartsWith("1,"))
            {
                string value = command.Substring(2);

                stack.Push(value);
                results.Add($"Push({value}) -> Элемент добавлен");
            }
            else
            {
                switch (command)
                {
                    case "2":
                        if (stack.TryPop(out string popped))
                            results.Add($"Pop() -> Извлечён элемент: {popped}");
                        else
                            results.Add("Pop() -> Стек пуст");
                        break;

                    case "3":
                        if (stack.TryTop(out string top))
                            results.Add($"Top() -> На вершине: {top}");
                        else
                            results.Add("Top() -> Стек пуст");
                        break;

                    case "4":
                        results.Add($"isEmpty() -> {stack.IsEmpty()}");
                        break;

                    case "5":
                        results.Add($"Print() -> {stack}");//В C# при интерполяции строк для объекта автоматически вызывается метод ToString().
                        break;

                    default:
                        results.Add($"Неизвестная команда: {command}");
                        break;
                }
            }
        }

        return results;
    }
}
