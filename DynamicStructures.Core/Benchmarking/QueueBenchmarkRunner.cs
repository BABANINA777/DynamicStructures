
using DynamicStructures.Core.CommandProcessing;

namespace DynamicStructures.Core.Benchmarking;

public static class QueueBenchmarkRunner
{
    public static (double CustomTime, double StandardTime) Compare(
        string input,
        int repetitions = 10)
    {
        CustomQueueCommandProcessor customProcessor = new();
        StandardQueueCommandProcessor standardProcessor = new();

        double customTime =
            CommandProcessorBenchmark.MeasureAverageMilliseconds(
                customProcessor.Process,
                input,
                repetitions
            );

        double standardTime =
            CommandProcessorBenchmark.MeasureAverageMilliseconds(
                standardProcessor.Process,
                input,
                repetitions
            );

        return (customTime, standardTime);
    }
}