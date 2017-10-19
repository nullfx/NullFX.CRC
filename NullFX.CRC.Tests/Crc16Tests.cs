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
