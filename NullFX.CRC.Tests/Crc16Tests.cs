//
// Crc16Tests.cs
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NullFX.CRC.Tests {
    [TestClass]
    public class Crc16Tests {
        static byte[] TestBuffer = { 0x74, 0x65, 0x73, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72 };
        static byte[] ExtendedTestBuffer = { 0x00, 0x01, 0x02, 0x74, 0x65, 0x73, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x03, 0x04 };

        static ushort Crc16Crc = 0x132A;
        static ushort CrcDnpCrc = 0xBAAD;
        static ushort CrcCcittCrc = 0x23B9;
        static ushort CrcCcittKCrc = 0xD552;
        static ushort CrcCcitt1Crc = 0x0BB5;
        static ushort CrcCcittFCrc = 0xF7B6;

        // standard crc 16
        [TestMethod]
        public void Crc16StandardValidation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.Standard, TestBuffer ), Crc16Crc );
        }

        [TestMethod]
        public void Crc16StandardSegmentValidation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.Standard, ExtendedTestBuffer, 3, 11  ), Crc16Crc );
        }

        // dnp
        [TestMethod]
        public void Crc16DnpValidation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.Dnp, TestBuffer ), CrcDnpCrc );
        }

        // ccitt kermit
        [TestMethod]
        public void CrcCcittKermitValidation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.CcittKermit, TestBuffer ), CrcCcittKCrc );
        }

        // ccitt w/ initial value of 0x0000
        [TestMethod]
        public void Crc16CcittValidation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.Ccitt, TestBuffer ), CrcCcittCrc );
        }

        // ccitt w/ initial value of 0x1d04
        [TestMethod]
        public void Crc16Ccitt1D04Validation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.CcittInitialValue0x1D0F, TestBuffer ), CrcCcitt1Crc );
        }

        // ccitt w/ initial value of 0xffff
        [TestMethod]
        public void Crc16CcittFfffValidation ( ) {
            Assert.AreEqual ( Crc16.ComputeChecksum ( Crc16Algorithm.CcittInitialValue0xFFFF, TestBuffer ), CrcCcittFCrc );
        }

    }
}
