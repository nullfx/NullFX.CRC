﻿using System;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace NullFX.CRC.Tests {
    public class CrcTests {
        static byte[] buffer = new byte[] { 0x49, 0x74, 0x20, 0x69, 0x73, 0x20, 0x61, 0x20, 0x6D, 0x69, 0x73, 0x74, 0x61, 0x6B, 0x65, 0x20, 0x74, 0x6F, 0x20, 0x74, 0x68, 0x69, 0x6E, 0x6B, 0x20, 0x79, 0x6F, 0x75, 0x20, 0x63, 0x61, 0x6E, 0x20, 0x73, 0x6F, 0x6C, 0x76, 0x65, 0x20, 0x61, 0x6E, 0x79, 0x20, 0x6D, 0x61, 0x6A, 0x6F, 0x72, 0x20, 0x70, 0x72, 0x6F, 0x62, 0x6C, 0x65, 0x6D, 0x73, 0x20, 0x6A, 0x75, 0x73, 0x74, 0x20, 0x77, 0x69, 0x74, 0x68, 0x20, 0x70, 0x6F, 0x74, 0x61, 0x74, 0x6F, 0x65, 0x73 };
        [Theory]
        [MemberData ( nameof ( Crc8TestData ) )]
        public void PerformCrc8Tests<ExpectedChecksum> ( TestParameter<ExpectedChecksum> parameter ) {
            var range = parameter.TestRange;
            byte actual = 0;
            var start = range.Start.Value;
            var length = range.End.Value - range.Start.Value;
            if ( range.Equals ( Range.All ) ) {
                actual = Crc8.ComputeChecksum ( parameter.TestPayload );
            } else {
                actual = Crc8.ComputeChecksum ( parameter.TestPayload, start, length  );
            }
            var expected = (byte)(object)parameter.ExpectedCrc;
            $"0x{actual:X2}".Should ( ).Be ( $"0x{expected:X2}" );
        }

        [Theory]
        [MemberData ( nameof ( Crc16TestData ) )]
        public void PerformCrc16Tests<ExpectedChecksum> ( TestParameter<ExpectedChecksum> parameter ) {
            var range = parameter.TestRange;
            ushort actual = 0;
            var start = range.Start.Value;
            var length = range.End.Value - range.Start.Value;
            if ( range.Equals ( Range.All ) ) {
                actual = Crc16.ComputeChecksum ( parameter.Algorithm, parameter.TestPayload );
            } else {
                actual = Crc16.ComputeChecksum ( parameter.Algorithm, parameter.TestPayload, start, length );
            }
            var expected = (ushort)(object)parameter.ExpectedCrc;
            $"0x{actual:X4}".Should ( ).Be ( $"0x{expected:X4}" );
        }

        [Theory]
        [MemberData ( nameof ( Crc32TestData ) )]
        public void PerformCrc32Tests<ExpectedChecksum> ( TestParameter<ExpectedChecksum> parameter ) {
            var range = parameter.TestRange;
            uint actual = 0;
            var start = range.Start.Value;
            var length = range.End.Value - range.Start.Value;
            if ( range.Equals ( Range.All ) ) {
                actual = Crc32.ComputeChecksum ( parameter.TestPayload );
            } else {
                actual = Crc32.ComputeChecksum ( parameter.TestPayload, start, length );
            }
            var expected = (uint)(object)parameter.ExpectedCrc;
            $"0x{actual:X8}".Should ( ).Be ( $"0x{expected:X8}" );
        }


        public static IEnumerable<object[]> Crc8TestData ( ) {
            yield return new object[] { new TestParameter<byte> ( buffer, .., 0x49 )     { TestScenario = "CRC 8, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<byte> ( buffer, 3..14, 0x66 )  { TestScenario = "CRC 8, Checksum of bytes 3-14" }  };
            yield return new object[] { new TestParameter<byte> ( buffer, 9..14,  0xB0 ) { TestScenario = "CRC 8, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<byte> ( buffer, 70..76, 0x63 ) { TestScenario = "CRC 8, Checksum of bytes 70-76" } };
        }

        public static IEnumerable<object[]> Crc16TestData ( ) {
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0xFD78 ) { Algorithm = Crc16Algorithm.Standard                 ,TestScenario = "Standard CRC 16, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0xF3C7 ) { Algorithm = Crc16Algorithm.Ccitt                    ,TestScenario = "CRC 16 CCITT, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0x3ACE ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F  ,TestScenario = "CRC 16 CCITT Initial Value: 0x1D0F, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0x2237 ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF  ,TestScenario = "CRC 16 CCITT Initial Value: 0xFFFF, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0x97C0 ) { Algorithm = Crc16Algorithm.CcittKermit              ,TestScenario = "CRC 16 CCITT Kermit, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0x028B ) { Algorithm = Crc16Algorithm.Dnp                      ,TestScenario = "CRC 16 DNP, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, ..,    0x16E2 ) { Algorithm = Crc16Algorithm.Modbus                   ,TestScenario = "CRC 16 Modbus, Checksum of all bytes" } };

            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0x0B3B ) { Algorithm = Crc16Algorithm.Standard                 ,TestScenario = "Standard CRC 16, Checksum of bytes 3-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0x610B ) { Algorithm = Crc16Algorithm.Ccitt                    ,TestScenario = "CRC 16 CCITT, Checksum of bytes 3-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0x4907 ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F  ,TestScenario = "CRC 16 CCITT Initial Value: 0x1D0F, Checksum of bytes 3-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0xB504 ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF  ,TestScenario = "CRC 16 CCITT Initial Value: 0xFFFF, Checksum of bytes 3-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0x2495 ) { Algorithm = Crc16Algorithm.CcittKermit              ,TestScenario = "CRC 16 CCITT Kermit, Checksum of bytes 3-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0x3E29 ) { Algorithm = Crc16Algorithm.Dnp                      ,TestScenario = "CRC 16 DNP, Checksum of bytes 3-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 3..14, 0xEF3D ) { Algorithm = Crc16Algorithm.Modbus                   ,TestScenario = "CRC 16 Modbus, Checksum of bytes 3-14" } };

            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0xE86E ) { Algorithm = Crc16Algorithm.Standard                 ,TestScenario = "Standard CRC 16, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0x8917 ) { Algorithm = Crc16Algorithm.Ccitt                    ,TestScenario = "CRC 16 CCITT, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0x78D9 ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F  ,TestScenario = "CRC 16 CCITT Initial Value: 0x1D0F, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0x981B ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF  ,TestScenario = "CRC 16 CCITT Initial Value: 0xFFFF, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0xCDBE ) { Algorithm = Crc16Algorithm.CcittKermit              ,TestScenario = "CRC 16 CCITT Kermit, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0xF943 ) { Algorithm = Crc16Algorithm.Dnp                      ,TestScenario = "CRC 16 DNP, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 9..14, 0xE84A ) { Algorithm = Crc16Algorithm.Modbus                   ,TestScenario = "CRC 16 Modbus, Checksum of bytes 9-14" } };

            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x24F6 ) { Algorithm = Crc16Algorithm.Standard                ,TestScenario = "Standard CRC 16, Checksum of bytes 70-76" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x7545 ) { Algorithm = Crc16Algorithm.Ccitt                   ,TestScenario = "CRC 16 CCITT, Checksum of bytes 70-76" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x447B ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F ,TestScenario = "CRC 16 CCITT Initial Value: 0x1D0F, Checksum of bytes 70-76" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x7B55 ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF ,TestScenario = "CRC 16 CCITT Initial Value: 0xFFFF, Checksum of bytes 70-76" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x4288 ) { Algorithm = Crc16Algorithm.CcittKermit             ,TestScenario = "CRC 16 CCITT Kermit, Checksum of bytes 70-76" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x5BE9 ) { Algorithm = Crc16Algorithm.Dnp                     ,TestScenario = "CRC 16 DNP, Checksum of bytes 70-76" } };
            yield return new object[] { new TestParameter<ushort> ( buffer, 70..76, 0x3FF6 ) { Algorithm = Crc16Algorithm.Modbus                  ,TestScenario = "CRC 16 Modbus, Checksum of bytes 70-76" } };
        }

        public static IEnumerable<object[]> Crc32TestData ( ) {
            yield return new object[] { new TestParameter<uint> ( buffer, ..,     0xCBE25E99 ) { TestScenario = "CRC 32, Checksum of all bytes" } };
            yield return new object[] { new TestParameter<uint> ( buffer, 3..14,  0xA0D118A0 ) { TestScenario = "CRC 32, Checksum of bytes 3-14" }  };
            yield return new object[] { new TestParameter<uint> ( buffer, 9..14,  0xDEF4CFE9 ) { TestScenario = "CRC 32, Checksum of bytes 9-14" } };
            yield return new object[] { new TestParameter<uint> ( buffer, 70..76, 0xF373B43B ) { TestScenario = "CRC 32, Checksum of bytes 70-76" } };
        }
    }
}

