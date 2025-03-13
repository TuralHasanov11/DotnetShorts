using BenchmarkDotNet.Running;
using Benchmarks;

// Logging Benchmarks
//BenchmarkRunner.Run<LoggingBenchmarks>();

// Value Object Benchmarks
BenchmarkRunner.Run<ValueObjectBenchmarks>();

