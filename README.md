# NullFX CRC [![Build Status](https://travis-ci.org/nullfx/NullFX.CRC.svg?branch=master)](https://travis-ci.org/nullfx/NullFX.CRC)
NullFX CRC is a small set of CRC utilities written in native C# released under the MIT License

## Nuget:
[Nuget Package Page](https://www.nuget.org/packages/NullFX.CRC/)


## Install
```ps
PM> Install-Package NullFX.CRC
```

## Examples:
Each CRC library uses a common ComputeChecksum format. It accepts a byte array which can be computed by converting text / numbers / structures etc into an array, then passing it into ComputeChecksum for it's CRC.

```c
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


// checksum of a subset of the array
var bytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };
var segmentedBytesCrc = NullFX.CRC.Crc32.ComputeChecksum ( bytes, 2, 4 );
Console.WriteLine ( "Segment of bytes CRC: {0:X8}", segmentedBytesCrc );
```

### output:
```
Text CRC: 3AD00FD2
Number CRC: 1A5A601F
Random bytes CRC: B63CFBCD
Segment of bytes CRC: A0EC895E
```
