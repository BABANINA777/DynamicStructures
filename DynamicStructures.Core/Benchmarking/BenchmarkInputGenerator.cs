
namespace DynamicStructures.Core.Benchmarking;

public static class BenchmarkInputGenerator
{
    // Генерация последовательности команд
    public static string Generate(int commandCount, int enqueuePercentage)
    {
        if (commandCount < 0)
            throw new ArgumentOutOfRangeException(nameof(commandCount));

        if (enqueuePercentage < 0 || enqueuePercentage > 100)
            throw new ArgumentOutOfRangeException(nameof(enqueuePercentage));

        Random random = new Random();
        List<string> commands = new();

        int enqueueCount = commandCount * enqueuePercentage / 100;

        for (int i = 0; i < commandCount; i++)
        {
            if (i < enqueueCount)
                commands.Add($"1,{random.Next(1, 1000)}");
            else
                commands.Add("2");
        }

        // Перемешивание команд
        for (int i = commands.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            (commands[i], commands[j]) = (commands[j], commands[i]);
        }

        return string.Join(" ", commands);
    }
}