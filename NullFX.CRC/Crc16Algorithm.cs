//
// Crc16Algorithm.cs
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
		/// A CRC 16 CCITT Utility using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0 (used in XMODEM, Bluetooth PACTOR, SD, DigRF and other communication)
		/// </summary>
		Ccitt,
		/// <summary>
		/// Performs CRC 16 CCITT using a reversed  x^16 + x^12 + x^5 + 1 polynomial with an initial CRC value of 0
		/// </summary>
		CcittKermit,
		/// <summary>
		/// Performs CRC 16 CCITT using  x^16 + x^12 + x^5 + 1 polynomial with an initial CRC value of 0xffff
		/// </summary>
		CcittInitialValue0xFFFF,
		/// <summary>
		/// Performs CRC 16 CCITT using x^16 + x^12 + x^5 + 1 polynomial with an initial CRC value of 0x1D0F
		/// </summary>
		CcittInitialValue0x1D0F,
		/// <summary>
		/// Performs CRC 16 using reversed x^16 + x^13 + x^12 + x^11 + x^10 + x^8 + x^6 + x^5 + x^2 + 1 (0xA6BC) with an initial CRC value of 0 (used in Distributed Network Protocol communication)
		/// </summary>
		Dnp,
		/// <summary>
		/// Performs CRC 16 using x^16 + x^15 + x^2 + 1 polynomial with an initial CRC value of 0xffff (used in Modbus communication)
		/// </summary>
		Modbus
	}
}
