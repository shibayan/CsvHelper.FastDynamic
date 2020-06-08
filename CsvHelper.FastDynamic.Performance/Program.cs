using BenchmarkDotNet.Running;

namespace CsvHelper.FastDynamic.Performance
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ReaderBenchmark>();
        }
    }
}
