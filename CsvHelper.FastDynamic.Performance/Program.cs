using BenchmarkDotNet.Running;

using CsvHelper.FastDynamic.Performance;

BenchmarkRunner.Run<ReaderBenchmark>();
BenchmarkRunner.Run<WriterBenchmark>();
