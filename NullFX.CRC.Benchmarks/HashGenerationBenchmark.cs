using System;
using System.Data.HashFunction.CRC;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using NullFX.CRC;

namespace NullFX.CRC.Benchmarks
{
	public class HashGenerationBenchmark
	{
		public const int Seed = 1337;
		
		[Params(10, 100, 1000)]
		public int ArraySize { get; set; }

		private byte[] data;

		private ICRC crc8Provider;
		private ICRC crc16Provider;
		private ICRC crc32Provider;

		[GlobalSetup]
		public void PrepareBenchmark()
		{
			data = new byte[ArraySize];
			Random rnd = new Random(Seed);
			rnd.NextBytes(data);
			
			crc8Provider = CRCFactory.Instance.Create(CRCConfig.CRC8);
			crc16Provider = CRCFactory.Instance.Create(CRCConfig.CRC16_USB);
			crc32Provider = CRCFactory.Instance.Create(CRCConfig.CRC32);
		}
		
		[Benchmark]
		public byte NullFxCrc8()
		{
			var hash = NullFX.CRC.Crc8.ComputeChecksum(data, 0, ArraySize);
			return hash;
		}

		[Benchmark]
		public ushort NullFxCrc16()
		{
			var hash = NullFX.CRC.Crc16.ComputeChecksum(Crc16Algorithm.Standard, data, 0, ArraySize);
			return hash;
		}
		
		[Benchmark]
		public uint NullFxCrc32()
		{
			var hash = NullFX.CRC.Crc32.ComputeChecksum(data, 0, ArraySize);
			return hash;
		}

		[Benchmark]
		public byte[] DataHashFunctionCrc8()
		{
			var hash = crc8Provider.ComputeHash(data);
			return hash.Hash;
		}
		
		[Benchmark]
		public byte[] DataHashFunctionCrc16()
		{
			var hash = crc16Provider.ComputeHash(data);
			return hash.Hash;
		}
		
		[Benchmark]
		public byte[] DataHashFunctionCrc32()
		{
			var hash = crc32Provider.ComputeHash(data);
			return hash.Hash;
		}
	}
}