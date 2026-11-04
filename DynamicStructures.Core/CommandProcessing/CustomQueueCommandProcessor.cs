
using DynamicStructures.Core.DataStructures;

namespace DynamicStructures.Core.CommandProcessing;

public class CustomQueueCommandProcessor
{
    public List<string> Process(string input)
    {
        CustomQueue<string> queue = new();
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

                queue.Enqueue(value);
                results.Add($"Enqueue({value}) -> Элемент добавлен");
            }
            else
            {
                switch (command)
                {
                    case "2":
                        if (queue.TryDequeue(out string dequeued))
                            results.Add($"Dequeue() -> Извлечён элемент: {dequeued}");
                        else
                            results.Add("Dequeue() -> Очередь пуста");
                        break;

                    case "3":
                        if (queue.TryPeek(out string first))
                            results.Add($"Peek() -> Первый элемент: {first}");
                        else
                            results.Add("Peek() -> Очередь пуста");
                        break;

                    case "4":
                        results.Add($"isEmpty() -> {queue.IsEmpty()}");
                        break;

                    case "5":
                        results.Add($"Print() -> {queue}");
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
