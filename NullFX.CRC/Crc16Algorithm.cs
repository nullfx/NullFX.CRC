namespace NullFX.CRC {
    /// <summary>
    /// An enumeration defining which CRC 16 Algorithm to use
    /// </summary>
    public enum Crc16Algorithm {
        /// <summary>
        /// Performs CRC 16 using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
        /// </summary>
        Standard,
        /// <summary>
        /// A CRC 16 CCITT Utility using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
        /// </summary>
        Ccitt,
        /// <summary>
        /// Performs CRC 16 CCITT using a reversed x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0
        /// </summary>
        CcittKermit,
        /// <summary>
        /// Performs CRC 16 CCITT using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0xffff
        /// </summary>
        CcittInitialValue0xFFFF,
        /// <summary>
        /// Performs CRC 16 CCITT using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0x1D0F
        /// </summary>
        CcittInitialValue0x1D0F,
        /// <summary>
        /// Performs CRC 16 Distributed Network Protocol (DNP) using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 (0xA6BC) with an initial CRC value of 0
        /// </summary>
        Dnp
    }
}
