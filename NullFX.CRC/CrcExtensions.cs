using System;

namespace NullFX.CRC {
    /// <summary>
    /// Common set of manipulation methods used by the checksums 
    /// </summary>
    public static class CrcExtensions {
        public static ushort ByteSwap ( this ushort value ) {
            return ( ushort )( ( ( value & 0xff00 ) >> 8 ) | (value & 0x00ff) << 8 );
        }

        public static ushort ByteSwapCompliment(this ushort value) {
            return ByteSwap ( (ushort)(~value) );
        }
    }
}
