# NullFX CRC [![CI](https://github.com/nullfx/NullFX.CRC/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/nullfx/NullFX.CRC/actions/workflows/ci.yml) [![CodeQL](https://github.com/nullfx/NullFX.CRC/actions/workflows/codeql.yml/badge.svg?branch=main)](https://github.com/nullfx/NullFX.CRC/actions/workflows/codeql.yml) [![NuGet](https://img.shields.io/nuget/v/NullFX.CRC.svg)](https://www.nuget.org/packages/NullFX.CRC)

NullFX CRC is a small set of CRC utilities written in native C# released under the MIT License

## NuGet:

[GitHub Package Page](https://github.com/nullfx/NullFX.CRC/packages)

[NuGet.org Package Page](https://www.nuget.org/packages/NullFX.CRC)

## Install

```sh
dotnet add package NullFX.CRC
```

Run this from the folder that contains your project file (or add the project path after `dotnet add`). The latest released version is installed; the NuGet badge above shows the current version.

Supported target frameworks: `netstandard2.0`, `netcoreapp3.1`, `net40`, `net45`, `net46`, `net47`, `net48`, `net6.0`, `net7.0`, `net8.0` and `net10.0`.

## Examples:

Each CRC library uses a common `ComputeChecksum` format. It accepts a byte array which can be obtained by converting text / numbers / structures etc into an array, then passing the byte array into `ComputeChecksum` for it's CRC.

The `ComputeChecksum` method also has a `params` argument parameter allowing individual bytes to be passed into the method one at a time rather than as an array.

If a checksum needs to be performed on a segment of an array, rather than creating a copy of the array to perform the CRC on, you can pass in the entire buffer and specify the section of the array in which to perform the CRC calculation, saving time and memory:

```csharp
// using text
var text = "I am string content";
// convert text to a byte array
var textBuffer = System.Text.Encoding.UTF8.GetBytes ( text );

// get the CRC for the text
var textCrc = NullFX.CRC.Crc32.ComputeChecksum ( textBuffer );
Console.WriteLine ( "Text CRC: {0:X8}", textCrc );


// use a large number
var aNumber = 0xDEADBEEF;
// convert that to a byte array
var numberBuffer = System.BitConverter.GetBytes ( aNumber );

// get the CRC for the number
var numberCrc = NullFX.CRC.Crc32.ComputeChecksum ( numberBuffer );
Console.WriteLine ( "Number CRC: {0:X8}", numberCrc );


// bytes as params
var randomCrc = NullFX.CRC.Crc32.ComputeChecksum ( 0x01, 0x02, 0x03, 0x04 );
Console.WriteLine ( "Random bytes CRC: {0:X8}", randomCrc );


/// checksum of a subset of an array. perform the CRC on bytes at indices
// 2, 3, 4 and 5
var bytes = new byte[] { 0xFE, 0x2C, 0xED, 0x4B, 0x88, 0x31, 0x07, 0xBE };
var segmentedBytesCrc = Crc32.ComputeChecksum ( bytes, 2, 4 );
Console.WriteLine ( "Segment of bytes CRC: {0:X8}", segmentedBytesCrc );
```

### Output:

```
Text CRC: 3AD00FD2
Number CRC: 1A5A601F
Random bytes CRC: B63CFBCD
Segment of bytes CRC: DB1A36A1
```

## API Information

`Crc8`, and `Crc32`'s `ComputeChecksum` have 2 different signatures

`ComputeChecksum`(`byte[]` bytes)

and

`ComputeChecksum`(`byte[]` bytes, `int` start, `int` length )

`Crc16` has one additional initial parameter ( `Crc16Algorithm` )
where `Crc16Algorithm` is one of the following:

- Standard CRC 16
- CRC 16 CCITT with initial values of `0`, `FFFF` and `1D0F`
  - CRC 16 CCITT Kermit
- Modbus

`ComputeChecksum` ( `Crc16Algorithm` algorithm, byte[] bytes )

and

`ComputeChecksum` ( `Crc16Algorithm` algorithm, byte[] bytes, `int` start, `int` length )

.

**Note**: this repository is also mirrored on [GitLab](https://gitlab.com/nullfx-crc/nullfx.crc)

## Benchmarks

_BenchmarkDotNet ShortRun results from the release build for v1.1.15 on a GitHub-hosted runner. Absolute timings vary between runs and machines; compare rows within a table._

```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26100.33438/24H2/2024Update/HudsonValley) (Hyper-V)
AMD EPYC 7763 2.44GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]   : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  ShortRun : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method                | ArraySize | Mean         | Error       | StdDev     |
|---------------------- |---------- |-------------:|------------:|-----------:|
| **NullFxCrc8**            | **10**        |     **8.676 ns** |   **0.4364 ns** |  **0.0239 ns** |
| NullFxCrc16           | 10        |    13.695 ns |   3.3818 ns |  0.1854 ns |
| NullFxCrc32           | 10        |    12.138 ns |   1.0032 ns |  0.0550 ns |
| DataHashFunctionCrc8  | 10        |    98.618 ns |  26.8391 ns |  1.4711 ns |
| DataHashFunctionCrc16 | 10        |   110.368 ns |  14.7429 ns |  0.8081 ns |
| DataHashFunctionCrc32 | 10        |   114.110 ns |  18.3534 ns |  1.0060 ns |
| **NullFxCrc8**            | **100**       |   **155.654 ns** |   **4.5711 ns** |  **0.2506 ns** |
| NullFxCrc16           | 100       |   261.538 ns |   1.3877 ns |  0.0761 ns |
| NullFxCrc32           | 100       |   232.148 ns |   9.4263 ns |  0.5167 ns |
| DataHashFunctionCrc8  | 100       |   427.921 ns |  18.4161 ns |  1.0095 ns |
| DataHashFunctionCrc16 | 100       |   568.411 ns |  33.0251 ns |  1.8102 ns |
| DataHashFunctionCrc32 | 100       |   580.170 ns |  51.8906 ns |  2.8443 ns |
| **NullFxCrc8**            | **1000**      | **1,848.433 ns** |  **34.3229 ns** |  **1.8814 ns** |
| NullFxCrc16           | 1000      | 2,809.540 ns |  93.3972 ns |  5.1194 ns |
| NullFxCrc32           | 1000      | 2,507.788 ns |  14.5636 ns |  0.7983 ns |
| DataHashFunctionCrc8  | 1000      | 3,399.219 ns | 770.8561 ns | 42.2532 ns |
| DataHashFunctionCrc16 | 1000      | 5,151.755 ns | 299.4059 ns | 16.4114 ns |
| DataHashFunctionCrc32 | 1000      | 5,167.148 ns | 146.2924 ns |  8.0188 ns |
