# NullFX CRC [![Build Status](https://travis-ci.org/nullfx/NullFX.CRC.svg?branch=master)](https://travis-ci.org/nullfx/NullFX.CRC) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nullfx_NullFX.CRC&metric=alert_status)](https://sonarcloud.io/dashboard?id=nullfx_NullFX.CRC)
NullFX CRC is a small set of CRC utilities written in native C# released under the MIT License

## Nuget:
[Nuget Package Page](https://github.com/nullfx/NullFX.CRC/packages)

Disappointingly, I've stopped publishing this package on `nuget.org`.  Between versions 1.0.1 and v1.1.0 of this library  `nuget.org` switched account management to microsoft accounts orphaning my nuget package on their website with no ability for me to recover it or log in and update it. Dispite sending multiple emails (which they state is the only way to reclaim my package) I've had no response from `Microsoft Support` (surprise) so sadly, so i'll simply maintain the updated library feed here...


## Install
```sh
dotnet add PROJECT package NullFX.CRC --version 1.1.1
```

## Examples:
Each CRC library uses a common ComputeChecksum format. It accepts a byte array which can be computed by converting text / numbers / structures etc into an array, then passing it into ComputeChecksum for it's CRC.

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


this repository is also mirrored on [GitLab](https://gitlab.com/nullfx/NullFX.CRC) and [BitBucket](https://bitbucket.org/nullfx/nullfx.crc) just in case the new evil overlords kill off the greatness of github...
