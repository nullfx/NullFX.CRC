using System.Collections.Generic;
using Xunit;

namespace NullFX.CRC.Tests {
	public class CrcTests {
		static byte[] buffer = new byte[] { 0x49, 0x74, 0x20, 0x69, 0x73, 0x20, 0x61, 0x20, 0x6D, 0x69, 0x73, 0x74, 0x61, 0x6B, 0x65, 0x20, 0x74, 0x6F, 0x20, 0x74, 0x68, 0x69, 0x6E, 0x6B, 0x20, 0x79, 0x6F, 0x75, 0x20, 0x63, 0x61, 0x6E, 0x20, 0x73, 0x6F, 0x6C, 0x76, 0x65, 0x20, 0x61, 0x6E, 0x79, 0x20, 0x6D, 0x61, 0x6A, 0x6F, 0x72, 0x20, 0x70, 0x72, 0x6F, 0x62, 0x6C, 0x65, 0x6D, 0x73, 0x20, 0x6A, 0x75, 0x73, 0x74, 0x20, 0x77, 0x69, 0x74, 0x68, 0x20, 0x70, 0x6F, 0x74, 0x61, 0x74, 0x6F, 0x65, 0x73 };
		[Theory]
		[MemberData ( nameof ( Crc8TestData ) )]
		public void PerformCrc8Tests<ExpectedChecksum> ( TestParameter<ExpectedChecksum> parameter ) {
			var range = parameter.TestRange;
			byte actual = 0;
			if ( range == TestRange.All ) {
				actual = Crc8.ComputeChecksum ( parameter.TestPayload );
			} else {
				actual = Crc8.ComputeChecksum ( parameter.TestPayload, range.Start, range.Length );
			}
			var expected = (byte)(object)parameter.ExpectedCrc;
			Assert.Equal ( expected, actual );
		}

		[Theory]
		[MemberData ( nameof ( Crc16TestData ) )]
		public void PerformCrc16Tests<ExpectedChecksum> ( TestParameter<ExpectedChecksum> parameter ) {
			var range = parameter.TestRange;
			ushort actual = 0;
			if ( range == TestRange.All ) {
				actual = Crc16.ComputeChecksum ( parameter.Algorithm, parameter.TestPayload );
			} else {
				actual = Crc16.ComputeChecksum ( parameter.Algorithm, parameter.TestPayload, range.Start, range.Length );
			}
			var expected = (ushort)(object)parameter.ExpectedCrc;
			Assert.Equal ( expected, actual );
		}

		[Theory]
		[MemberData ( nameof ( Crc32TestData ) )]
		public void PerformCrc32Tests<ExpectedChecksum> ( TestParameter<ExpectedChecksum> parameter ) {
			var range = parameter.TestRange;
			uint actual = 0;
			if ( range == TestRange.All ) {
				actual = Crc32.ComputeChecksum ( parameter.TestPayload );
			} else {
				actual = Crc32.ComputeChecksum ( parameter.TestPayload, range.Start, range.Length );
			}
			var expected = (uint)(object)parameter.ExpectedCrc;
			Assert.Equal ( expected, actual );
		}


		public static IEnumerable<object[]> Crc8TestData ( ) {
			yield return new object[] { new TestParameter<byte> ( buffer, TestRange.All, 0x49 ) };
			yield return new object[] { new TestParameter<byte> ( buffer, new TestRange ( 3, 11 ), 0x66 ) };
			yield return new object[] { new TestParameter<byte> ( buffer, new TestRange ( 9, 5 ), 0xB0 ) };
			yield return new object[] { new TestParameter<byte> ( buffer, new TestRange ( 70, 6 ), 0x63 ) };
		}

		public static IEnumerable<object[]> Crc16TestData ( ) {
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0xFD78 ) { Algorithm = Crc16Algorithm.Standard } };
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0xF3C7 ) { Algorithm = Crc16Algorithm.Ccitt } };
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0x3ACE ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F } };
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0x2237 ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF } };
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0xC097 ) { Algorithm = Crc16Algorithm.CcittKermit } };
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0x8B02 ) { Algorithm = Crc16Algorithm.Dnp } };
			yield return new object[] { new TestParameter<ushort> ( buffer, TestRange.All, 0x16E2 ) { Algorithm = Crc16Algorithm.Modbus } };

			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0x0B3B ) { Algorithm = Crc16Algorithm.Standard } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0x610B ) { Algorithm = Crc16Algorithm.Ccitt } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0x4907 ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0xB504 ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0x9524 ) { Algorithm = Crc16Algorithm.CcittKermit } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0x293E ) { Algorithm = Crc16Algorithm.Dnp } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 3, 11 ), 0xEF3D ) { Algorithm = Crc16Algorithm.Modbus } };

			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0xE86E ) { Algorithm = Crc16Algorithm.Standard } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0x8917 ) { Algorithm = Crc16Algorithm.Ccitt } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0x78D9 ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0x981B ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0xBECD ) { Algorithm = Crc16Algorithm.CcittKermit } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0x43F9 ) { Algorithm = Crc16Algorithm.Dnp } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 9, 5 ), 0xE84A ) { Algorithm = Crc16Algorithm.Modbus } };

			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0x24F6 ) { Algorithm = Crc16Algorithm.Standard } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0x7545 ) { Algorithm = Crc16Algorithm.Ccitt } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0x447B ) { Algorithm = Crc16Algorithm.CcittInitialValue0x1D0F } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0x7B55 ) { Algorithm = Crc16Algorithm.CcittInitialValue0xFFFF } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0x8842 ) { Algorithm = Crc16Algorithm.CcittKermit } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0xE95B ) { Algorithm = Crc16Algorithm.Dnp } };
			yield return new object[] { new TestParameter<ushort> ( buffer, new TestRange ( 70, 6 ), 0x3FF6 ) { Algorithm = Crc16Algorithm.Modbus } };
		}

		public static IEnumerable<object[]> Crc32TestData ( ) {
			yield return new object[] { new TestParameter<uint> ( buffer, TestRange.All, 0xCBE25E99 ) };
			yield return new object[] { new TestParameter<uint> ( buffer, new TestRange ( 3, 11 ), 0xA0D118A0 ) };
			yield return new object[] { new TestParameter<uint> ( buffer, new TestRange ( 9, 5 ), 0xDEF4CFE9 ) };
			yield return new object[] { new TestParameter<uint> ( buffer, new TestRange ( 70, 6 ), 0xF373B43B ) };
		}
	}
}

