using BenchmarkDotNet.Running;

namespace NullFX.CRC.Benchmarks
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			BenchmarkRunner.Run<HashGenerationBenchmark>();
		}
	}
}