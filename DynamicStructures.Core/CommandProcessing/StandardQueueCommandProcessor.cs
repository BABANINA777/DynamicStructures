
namespace DynamicStructures.Core.CommandProcessing;

public class StandardQueueCommandProcessor
{
    public List<string> Process(string input)
    {
        var queue = new Queue<string>();
        var results = new List<string>();

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
                        if (queue.Count > 0)
                        {
                            string dequeued = queue.Dequeue();
                            results.Add($"Dequeue() -> Извлечён элемент: {dequeued}");
                        }
                        else
                        {
                            results.Add("Dequeue() -> Коллекция пуста");
                        }
                        break;

                    case "3":
                        if (queue.Count > 0)
                            results.Add($"Peek() -> Первый элемент: {queue.Peek()}");
                        else
                            results.Add("Peek() -> Коллекция пуста");
                        break;

                    case "4":
                        results.Add($"isEmpty() -> {queue.Count == 0}");
                        break;

                    case "5":
                        results.Add($"Print() -> {(queue.Count == 0 ? "Коллекция пуста" : string.Join(" -> ", queue))}");
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
