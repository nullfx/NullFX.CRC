//
// Crc16.cs
//
// Author:
//       steve whitley <steve@nullfx.com>
//
// Copyright (c) 2017 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;

namespace NullFX.CRC {
    /// <summary>
    /// A CRC 16 class wrapping various CRC 16 algorithms
    /// </summary>
    public static class Crc16 {
        /// <summary>
        /// A CRC 16 Utility using reversed x^16 + x^15 + x^2 + 1 (0xA001) polynomial with an initial CRC value of 0
        /// </summary>
        internal static class Standard {
            // pre-computed table using reversed x^16 + x^15 + x^2 + 1 poly / 0xA001
            static ushort[] table = { 0x0000, 0xC0C1, 0xC181, 0x0140, 0xC301, 0x03C0, 0x0280, 0xC241, 0xC601, 0x06C0, 0x0780, 0xC741, 0x0500, 0xC5C1, 0xC481, 0x0440, 0xCC01, 0x0CC0, 0x0D80, 0xCD41, 0x0F00, 0xCFC1, 0xCE81, 0x0E40, 0x0A00, 0xCAC1, 0xCB81, 0x0B40, 0xC901, 0x09C0, 0x0880, 0xC841, 0xD801, 0x18C0, 0x1980, 0xD941, 0x1B00, 0xDBC1, 0xDA81, 0x1A40, 0x1E00, 0xDEC1, 0xDF81, 0x1F40, 0xDD01, 0x1DC0, 0x1C80, 0xDC41, 0x1400, 0xD4C1, 0xD581, 0x1540, 0xD701, 0x17C0, 0x1680, 0xD641, 0xD201, 0x12C0, 0x1380, 0xD341, 0x1100, 0xD1C1, 0xD081, 0x1040, 0xF001, 0x30C0, 0x3180, 0xF141, 0x3300, 0xF3C1, 0xF281, 0x3240, 0x3600, 0xF6C1, 0xF781, 0x3740, 0xF501, 0x35C0, 0x3480, 0xF441, 0x3C00, 0xFCC1, 0xFD81, 0x3D40, 0xFF01, 0x3FC0, 0x3E80, 0xFE41, 0xFA01, 0x3AC0, 0x3B80, 0xFB41, 0x3900, 0xF9C1, 0xF881, 0x3840, 0x2800, 0xE8C1, 0xE981, 0x2940, 0xEB01, 0x2BC0, 0x2A80, 0xEA41, 0xEE01, 0x2EC0, 0x2F80, 0xEF41, 0x2D00, 0xEDC1, 0xEC81, 0x2C40, 0xE401, 0x24C0, 0x2580, 0xE541, 0x2700, 0xE7C1, 0xE681, 0x2640, 0x2200, 0xE2C1, 0xE381, 0x2340, 0xE101, 0x21C0, 0x2080, 0xE041, 0xA001, 0x60C0, 0x6180, 0xA141, 0x6300, 0xA3C1, 0xA281, 0x6240, 0x6600, 0xA6C1, 0xA781, 0x6740, 0xA501, 0x65C0, 0x6480, 0xA441, 0x6C00, 0xACC1, 0xAD81, 0x6D40, 0xAF01, 0x6FC0, 0x6E80, 0xAE41, 0xAA01, 0x6AC0, 0x6B80, 0xAB41, 0x6900, 0xA9C1, 0xA881, 0x6840, 0x7800, 0xB8C1, 0xB981, 0x7940, 0xBB01, 0x7BC0, 0x7A80, 0xBA41, 0xBE01, 0x7EC0, 0x7F80, 0xBF41, 0x7D00, 0xBDC1, 0xBC81, 0x7C40, 0xB401, 0x74C0, 0x7580, 0xB541, 0x7700, 0xB7C1, 0xB681, 0x7640, 0x7200, 0xB2C1, 0xB381, 0x7340, 0xB101, 0x71C0, 0x7080, 0xB041, 0x5000, 0x90C1, 0x9181, 0x5140, 0x9301, 0x53C0, 0x5280, 0x9241, 0x9601, 0x56C0, 0x5780, 0x9741, 0x5500, 0x95C1, 0x9481, 0x5440, 0x9C01, 0x5CC0, 0x5D80, 0x9D41, 0x5F00, 0x9FC1, 0x9E81, 0x5E40, 0x5A00, 0x9AC1, 0x9B81, 0x5B40, 0x9901, 0x59C0, 0x5880, 0x9841, 0x8801, 0x48C0, 0x4980, 0x8941, 0x4B00, 0x8BC1, 0x8A81, 0x4A40, 0x4E00, 0x8EC1, 0x8F81, 0x4F40, 0x8D01, 0x4DC0, 0x4C80, 0x8C41, 0x4400, 0x84C1, 0x8581, 0x4540, 0x8701, 0x47C0, 0x4680, 0x8641, 0x8201, 0x42C0, 0x4380, 0x8341, 0x4100, 0x81C1, 0x8081, 0x4040 };
            internal static ushort InitialValue = ushort.MinValue;
            /// <summary>
            /// Computes a CRC 16 of the provided bytes
            /// </summary>
            /// <param name="bytes">bytes to crc</param>
            /// <returns>the CRC 16 of the provided bytes</returns>
            internal static ushort ComputeChecksum ( params byte[] bytes ) {
                return ComputeChecksum ( bytes, 0, bytes?.Length ?? 0 );
            }

            /// <summary>
            /// Computes a CRC 16 of the provided bytes
            /// </summary>
            /// <param name="bytes">The buffer to compute the CRC upon</param>
            /// <param name="start">The start index upon which to compute the CRC</param>
            /// <param name="length">The length of the buffer upon which to compute the CRC</param>
            /// <returns>The specified CRC</returns>
            internal static ushort ComputeChecksum ( byte[] bytes, int start, int length ) {
                if ( bytes == null ) { throw new ArgumentNullException ( nameof ( bytes ) ); }
                if ( bytes.Length == 0 ) { throw new ArgumentOutOfRangeException ( nameof ( bytes.Length ) ); }
                if ( start < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( start ) ); }
                if ( start + length > bytes.Length ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                if ( length < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                var crc = InitialValue;
                var end = start + length;
                for ( int i = start; i < end; ++i ) {
                    crc = ( ushort )( ( crc >> 8 ) ^ table[( byte )( crc ^ bytes[i] )] );
                }
                return crc;
            }
        }

        /// <summary>
        /// A CRC 16 Distributed Network Protocol (DNP) utility using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 (0xA6BC) polynomial with an initial CRC value of 0
        /// </summary>
        internal static class Dnp {
            // pre-computed table using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 poly / 0xA6BC
            static ushort[] table = { 0x0000, 0x365E, 0x6CBC, 0x5AE2, 0xD978, 0xEF26, 0xB5C4, 0x839A, 0xFF89, 0xC9D7, 0x9335, 0xA56B, 0x26F1, 0x10AF, 0x4A4D, 0x7C13, 0xB26B, 0x8435, 0xDED7, 0xE889, 0x6B13, 0x5D4D, 0x07AF, 0x31F1, 0x4DE2, 0x7BBC, 0x215E, 0x1700, 0x949A, 0xA2C4, 0xF826, 0xCE78, 0x29AF, 0x1FF1, 0x4513, 0x734D, 0xF0D7, 0xC689, 0x9C6B, 0xAA35, 0xD626, 0xE078, 0xBA9A, 0x8CC4, 0x0F5E, 0x3900, 0x63E2, 0x55BC, 0x9BC4, 0xAD9A, 0xF778, 0xC126, 0x42BC, 0x74E2, 0x2E00, 0x185E, 0x644D, 0x5213, 0x08F1, 0x3EAF, 0xBD35, 0x8B6B, 0xD189, 0xE7D7, 0x535E, 0x6500, 0x3FE2, 0x09BC, 0x8A26, 0xBC78, 0xE69A, 0xD0C4, 0xACD7, 0x9A89, 0xC06B, 0xF635, 0x75AF, 0x43F1, 0x1913, 0x2F4D, 0xE135, 0xD76B, 0x8D89, 0xBBD7, 0x384D, 0x0E13, 0x54F1, 0x62AF, 0x1EBC, 0x28E2, 0x7200, 0x445E, 0xC7C4, 0xF19A, 0xAB78, 0x9D26, 0x7AF1, 0x4CAF, 0x164D, 0x2013, 0xA389, 0x95D7, 0xCF35, 0xF96B, 0x8578, 0xB326, 0xE9C4, 0xDF9A, 0x5C00, 0x6A5E, 0x30BC, 0x06E2, 0xC89A, 0xFEC4, 0xA426, 0x9278, 0x11E2, 0x27BC, 0x7D5E, 0x4B00, 0x3713, 0x014D, 0x5BAF, 0x6DF1, 0xEE6B, 0xD835, 0x82D7, 0xB489, 0xA6BC, 0x90E2, 0xCA00, 0xFC5E, 0x7FC4, 0x499A, 0x1378, 0x2526, 0x5935, 0x6F6B, 0x3589, 0x03D7, 0x804D, 0xB613, 0xECF1, 0xDAAF, 0x14D7, 0x2289, 0x786B, 0x4E35, 0xCDAF, 0xFBF1, 0xA113, 0x974D, 0xEB5E, 0xDD00, 0x87E2, 0xB1BC, 0x3226, 0x0478, 0x5E9A, 0x68C4, 0x8F13, 0xB94D, 0xE3AF, 0xD5F1, 0x566B, 0x6035, 0x3AD7, 0x0C89, 0x709A, 0x46C4, 0x1C26, 0x2A78, 0xA9E2, 0x9FBC, 0xC55E, 0xF300, 0x3D78, 0x0B26, 0x51C4, 0x679A, 0xE400, 0xD25E, 0x88BC, 0xBEE2, 0xC2F1, 0xF4AF, 0xAE4D, 0x9813, 0x1B89, 0x2DD7, 0x7735, 0x416B, 0xF5E2, 0xC3BC, 0x995E, 0xAF00, 0x2C9A, 0x1AC4, 0x4026, 0x7678, 0x0A6B, 0x3C35, 0x66D7, 0x5089, 0xD313, 0xE54D, 0xBFAF, 0x89F1, 0x4789, 0x71D7, 0x2B35, 0x1D6B, 0x9EF1, 0xA8AF, 0xF24D, 0xC413, 0xB800, 0x8E5E, 0xD4BC, 0xE2E2, 0x6178, 0x5726, 0x0DC4, 0x3B9A, 0xDC4D, 0xEA13, 0xB0F1, 0x86AF, 0x0535, 0x336B, 0x6989, 0x5FD7, 0x23C4, 0x159A, 0x4F78, 0x7926, 0xFABC, 0xCCE2, 0x9600, 0xA05E, 0x6E26, 0x5878, 0x029A, 0x34C4, 0xB75E, 0x8100, 0xDBE2, 0xEDBC, 0x91AF, 0xA7F1, 0xFD13, 0xCB4D, 0x48D7, 0x7E89, 0x246B, 0x1235 };
            internal static ushort InitialValue = ushort.MinValue;
            /// <summary>
            /// Computes a CRC 16 Distributed Network Protocol (DNP) utility using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 (0xA6BC) polynomial with an initial CRC value of 0
            /// </summary>
            /// <param name="bytes">bytes to crc</param>
            /// <returns>the CRC 16 of the provided bytes</returns>
            internal static ushort ComputeChecksum ( params byte[] bytes ) {
                return ComputeChecksum ( bytes, 0, bytes?.Length ?? 0 );
            }

            /// <summary>
            /// Computes a CRC 16 Distributed Network Protocol (DNP) utility using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 (0xA6BC) polynomial with an initial CRC value of 0
            /// </summary>
            /// <param name="bytes">The buffer to compute the CRC upon</param>
            /// <param name="start">The start index upon which to compute the CRC</param>
            /// <param name="length">The length of the buffer upon which to compute the CRC</param>
            /// <returns>The specified CRC</returns>
            internal static ushort ComputeChecksum ( byte[] bytes, int start, int length ) {
                if ( bytes == null ) { throw new ArgumentNullException ( nameof ( bytes ) ); }
                if ( bytes.Length == 0 ) { throw new ArgumentOutOfRangeException ( nameof ( bytes.Length ) ); }
                if ( start < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( start ) ); }
                if ( start + length > bytes.Length ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                if ( length < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                var crc = InitialValue;
                var end = start + length;
                for ( int i = start; i < end; ++i ) {
                    crc = ( ushort )( ( crc >> 8 ) ^ table[( byte )( crc ^ bytes[i] )] );
                }
                return crc.ByteSwapCompliment ( );
            }
        }

        /// <summary>
        /// A CRC 16 CCITT Utility using a reversed x^16 + x^15 + x^2 + 1 polynomial (0x8408) with an initial CRC value of 0
        /// </summary>
        internal static class CcittKermit {
            // pre-computed table using reversed x^16 + x^15 + x^2 + 1 poly / 0x8408 
            static ushort[] table = { 0x0000, 0x1189, 0x2312, 0x329B, 0x4624, 0x57AD, 0x6536, 0x74BF, 0x8C48, 0x9DC1, 0xAF5A, 0xBED3, 0xCA6C, 0xDBE5, 0xE97E, 0xF8F7, 0x1081, 0x0108, 0x3393, 0x221A, 0x56A5, 0x472C, 0x75B7, 0x643E, 0x9CC9, 0x8D40, 0xBFDB, 0xAE52, 0xDAED, 0xCB64, 0xF9FF, 0xE876, 0x2102, 0x308B, 0x0210, 0x1399, 0x6726, 0x76AF, 0x4434, 0x55BD, 0xAD4A, 0xBCC3, 0x8E58, 0x9FD1, 0xEB6E, 0xFAE7, 0xC87C, 0xD9F5, 0x3183, 0x200A, 0x1291, 0x0318, 0x77A7, 0x662E, 0x54B5, 0x453C, 0xBDCB, 0xAC42, 0x9ED9, 0x8F50, 0xFBEF, 0xEA66, 0xD8FD, 0xC974, 0x4204, 0x538D, 0x6116, 0x709F, 0x0420, 0x15A9, 0x2732, 0x36BB, 0xCE4C, 0xDFC5, 0xED5E, 0xFCD7, 0x8868, 0x99E1, 0xAB7A, 0xBAF3, 0x5285, 0x430C, 0x7197, 0x601E, 0x14A1, 0x0528, 0x37B3, 0x263A, 0xDECD, 0xCF44, 0xFDDF, 0xEC56, 0x98E9, 0x8960, 0xBBFB, 0xAA72, 0x6306, 0x728F, 0x4014, 0x519D, 0x2522, 0x34AB, 0x0630, 0x17B9, 0xEF4E, 0xFEC7, 0xCC5C, 0xDDD5, 0xA96A, 0xB8E3, 0x8A78, 0x9BF1, 0x7387, 0x620E, 0x5095, 0x411C, 0x35A3, 0x242A, 0x16B1, 0x0738, 0xFFCF, 0xEE46, 0xDCDD, 0xCD54, 0xB9EB, 0xA862, 0x9AF9, 0x8B70, 0x8408, 0x9581, 0xA71A, 0xB693, 0xC22C, 0xD3A5, 0xE13E, 0xF0B7, 0x0840, 0x19C9, 0x2B52, 0x3ADB, 0x4E64, 0x5FED, 0x6D76, 0x7CFF, 0x9489, 0x8500, 0xB79B, 0xA612, 0xD2AD, 0xC324, 0xF1BF, 0xE036, 0x18C1, 0x0948, 0x3BD3, 0x2A5A, 0x5EE5, 0x4F6C, 0x7DF7, 0x6C7E, 0xA50A, 0xB483, 0x8618, 0x9791, 0xE32E, 0xF2A7, 0xC03C, 0xD1B5, 0x2942, 0x38CB, 0x0A50, 0x1BD9, 0x6F66, 0x7EEF, 0x4C74, 0x5DFD, 0xB58B, 0xA402, 0x9699, 0x8710, 0xF3AF, 0xE226, 0xD0BD, 0xC134, 0x39C3, 0x284A, 0x1AD1, 0x0B58, 0x7FE7, 0x6E6E, 0x5CF5, 0x4D7C, 0xC60C, 0xD785, 0xE51E, 0xF497, 0x8028, 0x91A1, 0xA33A, 0xB2B3, 0x4A44, 0x5BCD, 0x6956, 0x78DF, 0x0C60, 0x1DE9, 0x2F72, 0x3EFB, 0xD68D, 0xC704, 0xF59F, 0xE416, 0x90A9, 0x8120, 0xB3BB, 0xA232, 0x5AC5, 0x4B4C, 0x79D7, 0x685E, 0x1CE1, 0x0D68, 0x3FF3, 0x2E7A, 0xE70E, 0xF687, 0xC41C, 0xD595, 0xA12A, 0xB0A3, 0x8238, 0x93B1, 0x6B46, 0x7ACF, 0x4854, 0x59DD, 0x2D62, 0x3CEB, 0x0E70, 0x1FF9, 0xF78F, 0xE606, 0xD49D, 0xC514, 0xB1AB, 0xA022, 0x92B9, 0x8330, 0x7BC7, 0x6A4E, 0x58D5, 0x495C, 0x3DE3, 0x2C6A, 0x1EF1, 0x0F78 };
            internal static ushort InitialValue = ushort.MinValue;
            /// <summary>
            /// Computes a CRC 16 CCITT Kermit of the provided bytes using an initial value of 0
            /// </summary>
            /// <param name="bytes">bytes to crc</param>
            /// <returns>the CRC 16 of the provided bytes</returns>
            internal static ushort ComputeChecksum ( params byte[] bytes ) {
                return ComputeChecksum ( bytes, 0, bytes?.Length ?? 0 );
            }

            /// <summary>
            /// Computes a CRC 16 CCITT Kermit of the provided bytes using an initial value of 0
            /// </summary>
            /// <param name="bytes">The buffer to compute the CRC upon</param>
            /// <param name="start">The start index upon which to compute the CRC</param>
            /// <param name="length">The length of the buffer upon which to compute the CRC</param>
            /// <returns>The specified CRC</returns>
            internal static ushort ComputeChecksum ( byte[] bytes, int start, int length ) {
                if ( bytes == null ) { throw new ArgumentNullException ( nameof ( bytes ) ); }
                if ( bytes.Length == 0 ) { throw new ArgumentOutOfRangeException ( nameof ( bytes.Length ) ); }
                if ( start < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( start ) ); }
                if ( start + length > bytes.Length ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                if ( length < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                var crc = InitialValue;
                var end = start + length;
                for ( int i = start; i < end; ++i ) {
                    crc = ( ushort )( ( crc >> 8 ) ^ table[( byte )( crc ^ bytes[i] )] );
                }
                return crc.ByteSwap ( );
            }
        }

        /// <summary>
        /// A CRC 16 CCITT Utility using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
        /// </summary>
        internal static class CcittInitialZero {
            internal static ushort InitialValue = ushort.MinValue;
            /// <summary>
            /// Computes a CRC 16 CCITT of the provided bytes using an intial value of 0
            /// </summary>
            /// <param name="bytes">bytes to crc</param>
            /// <returns>the CRC 16 CCITT of the provided bytes</returns>
            public static ushort ComputeChecksum ( params byte[] bytes ) {
                return CrcCcittBase.ComputeChecksum ( InitialValue, bytes );
            }

            /// <summary>
            /// Computes a CRC 16 CCITT of the provided bytes using an intial value of 0
            /// </summary>
            /// <param name="bytes">The buffer to compute the CRC upon</param>
            /// <param name="start">The start index upon which to compute the CRC</param>
            /// <param name="length">The length of the buffer upon which to compute the CRC</param>
            /// <returns>The specified CRC</returns>
            public static ushort ComputeChecksum ( byte[] bytes, int start, int length ) {
                return CrcCcittBase.ComputeChecksum ( InitialValue, bytes, start, length );
            }
        }

        /// <summary>
        /// A CRC 16 CCITT Utility using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0xffff
        /// </summary>
        internal static class CcittInitial0xFFFF {
            internal static ushort InitialValue = ushort.MaxValue;
            /// <summary>
            /// Computes a CRC 16 CCITT of the provided bytes using an intial value of 0xFFFF
            /// </summary>
            /// <param name="bytes">bytes to crc</param>
            /// <returns>the CRC 16 CCITT of the provided bytes</returns>
            public static ushort ComputeChecksum ( params byte[] bytes ) {
                return CrcCcittBase.ComputeChecksum ( InitialValue, bytes );
            }

            /// <summary>
            /// Computes a CRC 16 CCITT of the provided bytes using an intial value of 0xFFFF
            /// </summary>
            /// <param name="bytes">The buffer to compute the CRC upon</param>
            /// <param name="start">The start index upon which to compute the CRC</param>
            /// <param name="length">The length of the buffer upon which to compute the CRC</param>
            /// <returns>The specified CRC</returns>
            public static ushort ComputeChecksum ( byte[] bytes, int start, int length ) {
                return CrcCcittBase.ComputeChecksum ( InitialValue, bytes, start, length );
            }
        }

        /// <summary>
        /// A CRC 16 CCITT Utility using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0x1D0F
        /// </summary>
        internal static class CcittInitial0x1D0F {
            internal static ushort InitialValue = 0x1D0F;
            /// <summary>
            /// Computes a CRC 16 CCITT of the provided bytes using an intial value of 0x1D0F
            /// </summary>
            /// <param name="bytes">bytes to crc</param>
            /// <returns>the CRC 16 CCITT of the provided bytes</returns>
            public static ushort ComputeChecksum ( params byte[] bytes ) {
                return CrcCcittBase.ComputeChecksum ( InitialValue, bytes );
            }

            /// <summary>
            /// Computes a CRC 16 CCITT of the provided bytes using an intial value of 0x1D0F
            /// </summary>
            /// <param name="bytes">The buffer to compute the CRC upon</param>
            /// <param name="start">The start index upon which to compute the CRC</param>
            /// <param name="length">The length of the buffer upon which to compute the CRC</param>
            /// <returns>The specified CRC</returns>
            public static ushort ComputeChecksum ( byte[] bytes, int start, int length ) {
                return CrcCcittBase.ComputeChecksum ( InitialValue, bytes, start, length );
            }
        }
        // Base CRC 16 CCITT implementation
        internal static class CrcCcittBase {
            // pre-computed table using x^16 + x^15 + x^2 + 1 polynomial / 0x1021
            internal static ushort[] Table { get; private set; } = { 0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7, 0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF, 0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6, 0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE, 0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485, 0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D, 0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4, 0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC, 0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823, 0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B, 0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12, 0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A, 0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41, 0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49, 0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70, 0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78, 0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F, 0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067, 0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E, 0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256, 0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D, 0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405, 0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C, 0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634, 0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB, 0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3, 0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A, 0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92, 0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9, 0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1, 0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8, 0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0 };
            // base compute checksum code
            internal static ushort ComputeChecksum ( ushort initialValue, params byte[] bytes ) {
                return ComputeChecksum ( initialValue, bytes, 0, bytes?.Length ?? 0 );
            }
            internal static ushort ComputeChecksum ( ushort initialValue, byte[] bytes, int start, int length ) {
                if ( bytes == null ) { throw new ArgumentNullException ( nameof ( bytes ) ); }
                if ( bytes.Length == 0 ) { throw new ArgumentOutOfRangeException ( nameof ( bytes.Length ) ); }
                if ( start < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( start ) ); }
                if ( start + length > bytes.Length ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                if ( length < 0 ) { throw new ArgumentOutOfRangeException ( nameof ( length ) ); }
                var crc = initialValue;
                var end = start + length;
                for ( int i = start; i < end; ++i ) {
                    crc = ( ushort )( ( crc << 8 ) ^ Table[( ( crc >> 8 ) ^ ( 0xff & bytes[i] ) )] );
                }
                return crc;
            }
        }

        /// <summary>
        /// Computes a CRC 16 checksum of the specified bytes using the given algorithm
        /// </summary>
        /// <param name="algorithm">The CRC 16 Algorithm to use</param>
        /// <param name="bytes">The buffer to compute the CRC upon</param>
        /// <returns>The specified CRC</returns>
        public static ushort ComputeChecksum ( Crc16Algorithm algorithm, params byte[] bytes ) {
            return ComputeChecksum ( algorithm, bytes, 0, bytes?.Length ?? 0 );
        }

        /// <summary>
        /// Computes a CRC 16 checksum of the specified bytes using the given algorithm
        /// </summary>
        /// <param name="algorithm">The CRC 16 Algorithm to use</param>
        /// <param name="bytes">The buffer to compute the CRC upon</param>
        /// <param name="start">The start index upon which to compute the CRC</param>
        /// <param name="length">The length of the buffer upon which to compute the CRC</param>
        /// <returns>The specified CRC</returns>
        public static ushort ComputeChecksum ( Crc16Algorithm algorithm, byte[] bytes, int start, int length ) {
            switch ( algorithm ) {
                case Crc16Algorithm.Standard:
                return Standard.ComputeChecksum ( bytes, start, length );
                case Crc16Algorithm.Ccitt:
                return CcittInitialZero.ComputeChecksum ( bytes, start, length );
                case Crc16Algorithm.CcittKermit:
                return CcittKermit.ComputeChecksum ( bytes, start, length );
                case Crc16Algorithm.CcittInitialValue0xFFFF:
                return CcittInitial0xFFFF.ComputeChecksum ( bytes, start, length );
                case Crc16Algorithm.CcittInitialValue0x1D0F:
                return CcittInitial0x1D0F.ComputeChecksum ( bytes, start, length );
                case Crc16Algorithm.Dnp:
                return Dnp.ComputeChecksum ( bytes );
            }
            throw new UnknownAlgorithmException ( "Unknown Algorithm" );
        }
    }
}
