
using System.Diagnostics;

namespace DynamicStructures.Core.Benchmarking;

public static class CommandProcessorBenchmark
{
    public static double MeasureAverageMilliseconds(
        Func<string, List<string>> processor,
        string input,
        int repetitions = 10)//repetitions = 10 по-умолчанию, но можно указать своё. Если не указывать, выполнится с 10 пвоторениями.
                             //Однократный замер может оказаться неточным из-за фоновой нагрузки системы, поэтому нужны повторения.
    {
        if (repetitions <= 0)
            throw new ArgumentOutOfRangeException(nameof(repetitions));

        // Прогрев перед замером. Защита от JIT-компиляция и т.п.
        processor(input);

        Stopwatch stopwatch = Stopwatch.StartNew();

        for (int i = 0; i < repetitions; i++)
        {
            processor(input);
        }

        stopwatch.Stop();

        return stopwatch.Elapsed.TotalMilliseconds / repetitions;
    }
}
