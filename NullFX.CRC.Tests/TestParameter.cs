using System;
namespace NullFX.CRC.Tests {
	public class TestParameter<ExpectedChecksum> {
		public byte[] TestPayload { get; set; }
		public TestRange TestRange { get; set; }
		public ExpectedChecksum ExpectedCrc { get; set; }
		public Crc16Algorithm Algorithm { get; set; }
		public string TestScenario { get; set; }
		public TestParameter ( byte[] payload, TestRange range, ExpectedChecksum expected ) {
			TestPayload = payload;
			TestRange = range;
			ExpectedCrc = expected;
			TestScenario = "";
		}
	}
}

