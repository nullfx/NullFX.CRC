using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NullFX.CRC.Tests {
    [TestClass]
    public class Crc32Tests {
        static byte[] TestBuffer = { 0x74, 0x65, 0x73, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72 };
        static byte[] ExtendedTestBuffer = { 0x00, 0x01, 0x02, 0x74, 0x65, 0x73, 0x74, 0x20, 0x62, 0x75, 0x66, 0x66, 0x65, 0x72, 0x03, 0x04 };
        static uint Crc32Crc = 0xBCA3571E;
        
        [TestMethod]
        public void Crc32Validation ( ) {
            Assert.AreEqual ( Crc32.ComputeChecksum ( TestBuffer ), Crc32Crc );
        }

        [TestMethod]
        public void Crc32SegmentValidation ( ) {
            Assert.AreEqual ( Crc32.ComputeChecksum ( ExtendedTestBuffer, 3, 11 ), Crc32Crc );
        }
    }
}
