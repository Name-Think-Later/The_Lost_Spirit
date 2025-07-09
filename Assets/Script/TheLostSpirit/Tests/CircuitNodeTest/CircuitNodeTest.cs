using Script.TheLostSpirit.Circuit;
using NUnit.Framework;
using NSubstitute;


namespace Script.TheLostSpirit.Tests {
    public class CircuitNodeTest {
        CircuitNode _nodeA, _nodeB, _nodeC;

        [SetUp]
        public void Setup() {
            Substitute.For<CircuitNode>();
        }

        [Test]
        public void UnitTestSimplePasses() {
            // Use the Assert class to test conditions
        }
    }
}