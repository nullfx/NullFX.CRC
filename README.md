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

# Benchmarks

_Benchmark results are published here automatically by the release workflow._
