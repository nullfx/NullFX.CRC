using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NullFX.CRC.Tests {
    [TestClass]
    public class Crc8Tests {
        static byte[] TestBuffer = { 0x74, 0x65, 0x73, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72 };
        static byte[] ExtendedTestBuffer = { 0x00, 0x01, 0x02, 0x74, 0x65, 0x73, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x03, 0x04 };
        static byte Crc8Crc = 0xF9;
        
        [TestMethod]
        public void Crc8Validation ( ) {
            Assert.AreEqual ( Crc8.ComputeChecksum ( TestBuffer ), Crc8Crc );
        }

        [TestMethod]
        public void Crc8SegmentValidation ( ) {
            Assert.AreEqual ( Crc8.ComputeChecksum ( ExtendedTestBuffer, 3, 11 ), Crc8Crc );
        }
    }
}
