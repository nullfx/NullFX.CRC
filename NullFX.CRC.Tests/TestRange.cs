using System;
using System.Diagnostics.CodeAnalysis;

namespace NullFX.CRC.Tests {
	public struct TestRange : IEquatable<TestRange> {
		private Guid id = Guid.NewGuid();
		public static TestRange All = new TestRange(int.MinValue, int.MinValue);
		public int Start;
		public int Length;
		public TestRange ( int start, int length ) {
			Start = start;
			Length = length;
		}

		public bool Equals ( TestRange other ) {
			return Start == other.Start && Length == other.Length;
		}
		public override bool Equals ( [NotNullWhen ( true )] object obj ) {
			if ( obj is TestRange ) {
				return Equals ( ( TestRange )obj );
			}
			return false;
		}
		public static bool operator == ( TestRange a, TestRange b ) {
			return a.Start == b.Start && a.Length == b.Length;
		}
		public static bool operator != ( TestRange a, TestRange b ) {
			return a.Start != b.Start || a.Length != b.Length;
		}

		public override int GetHashCode ( ) {
			return id.GetHashCode ( );
		}
	}
}

