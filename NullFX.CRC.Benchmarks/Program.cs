using BenchmarkDotNet.Running;

namespace NullFX.CRC.Benchmarks
{
	internal static class Program
	{
		static void Main(string[] args)
		{
			// Forward command-line arguments so CI can pick the job, exporters and
			// artifacts path (e.g. --job short --exporters json github). With no
			// arguments this behaves exactly as before.
			BenchmarkRunner.Run<HashGenerationBenchmark>(args: args);
		}
	}
}