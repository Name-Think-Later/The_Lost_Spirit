using NUnit.Framework;
using Script.TheLostSpirit.Circuit.CircuitNode;

namespace Script.TheLostSpirit.Tests.CircuitNodeTests {
    public class CircuitNodeTest {
        CircuitNode _nodeLeft, _nodeRight;

        [SetUp]
        public void Setup() {
            _nodeLeft  = new CircuitNode(null);
            _nodeRight = new CircuitNode(null);
        }

        [Test(Description = "不指定支度正確地建立一個節點")]
        public void Node_DefaultInitialize_ShouldHave_DefaultNumberOfAdjacency() {
            const int defaultAdjacencyCount = 2;

            CircuitNode node = new CircuitNode(null);
            Assert.AreEqual(node.Adjacencies.Count, defaultAdjacencyCount);
        }

        [Test(Description = "指定支度正確地建立一個節點")]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(99)]
        public void Node_InitializeWithCount_ShouldHave_CorrectNumberOfAdjacency(int count) {
            CircuitNode node = new CircuitNode(null, count);
            Assert.AreEqual(node.Adjacencies.Count, count);
        }

        [Test(Description = "建立節點時接口是否正確地建立並初始化")]
        public void Adjacency_InNode_Should_InitializeCorrectly() {
            CircuitNode node = new CircuitNode(null, 3);
            foreach (var adjacency in node.Adjacencies) {
                Assert.NotNull(adjacency);
                Assert.AreEqual(adjacency.Owner, node);
                Assert.IsNull(adjacency.Opposite);
                Assert.AreEqual(adjacency.Type, AdjacencyType.In);
            }
        }

        [Test(Description = "接口間是否正確地連接")]
        public void Adjacency_Should_ConnectCorrectly() {
            var adjacencyLeft  = _nodeLeft.Adjacencies[0];
            var adjacencyRight = _nodeRight.Adjacencies[0];
            adjacencyLeft.To(adjacencyRight);

            Assert.AreEqual(adjacencyLeft.Opposite, adjacencyRight);
            Assert.AreEqual(adjacencyRight.Opposite, adjacencyLeft);

            Assert.AreEqual(adjacencyLeft.Type, AdjacencyType.Out);
            Assert.AreEqual(adjacencyRight.Type, AdjacencyType.In);
        }

        [Test(Description = "接口間是否正確地切斷")]
        public void Adjacency_Should_DisconnectCorrectly() {
            var adjacencyLeft  = _nodeLeft.Adjacencies[0];
            var adjacencyRight = _nodeRight.Adjacencies[0];


            adjacencyLeft.To(adjacencyRight);
            adjacencyLeft.Cut();

            Assert.IsNull(adjacencyLeft.Opposite);
            Assert.IsNull(adjacencyRight.Opposite);


            adjacencyLeft.To(adjacencyRight);
            adjacencyRight.Cut();

            Assert.IsNull(adjacencyLeft.Opposite);
            Assert.IsNull(adjacencyRight.Opposite);
        }

        [Test(Description = "接口是否正確回傳占用狀態")]
        public void Adjacency_ExistStatus_Should_ReturnCorrectly() {
            var adjacencyLeft  = _nodeLeft.Adjacencies[0];
            var adjacencyRight = _nodeRight.Adjacencies[0];

            Assert.IsFalse(adjacencyLeft.IsExist);
            Assert.IsFalse(adjacencyRight.IsExist);
            Assert.IsTrue(adjacencyLeft.IsEmpty);
            Assert.IsTrue(adjacencyRight.IsEmpty);

            adjacencyLeft.To(adjacencyRight);

            Assert.IsTrue(adjacencyLeft.IsExist);
            Assert.IsTrue(adjacencyRight.IsExist);
            Assert.IsFalse(adjacencyLeft.IsEmpty);
            Assert.IsFalse(adjacencyRight.IsEmpty);

            adjacencyLeft.Cut();

            Assert.IsFalse(adjacencyLeft.IsExist);
            Assert.IsFalse(adjacencyRight.IsExist);
            Assert.IsTrue(adjacencyLeft.IsEmpty);
            Assert.IsTrue(adjacencyRight.IsEmpty);
        }

        [Test(Description = "接口是否正確回傳出入狀態")]
        public void Adjacency_DirectedStatus_Should_ReturnCorrectly() {
            var adjacencyLeft  = _nodeLeft.Adjacencies[0];
            var adjacencyRight = _nodeRight.Adjacencies[0];

            Assert.IsFalse(adjacencyLeft.IsIn);
            Assert.IsFalse(adjacencyLeft.IsOut);
            Assert.IsFalse(adjacencyRight.IsIn);
            Assert.IsFalse(adjacencyRight.IsOut);

            adjacencyLeft.To(adjacencyRight);

            Assert.IsFalse(adjacencyLeft.IsIn);
            Assert.IsTrue(adjacencyLeft.IsOut);
            Assert.IsTrue(adjacencyRight.IsIn);
            Assert.IsFalse(adjacencyRight.IsOut);

            adjacencyLeft.Cut();

            Assert.IsFalse(adjacencyLeft.IsIn);
            Assert.IsFalse(adjacencyLeft.IsOut);
            Assert.IsFalse(adjacencyRight.IsIn);
            Assert.IsFalse(adjacencyRight.IsOut);
        }
    }
}