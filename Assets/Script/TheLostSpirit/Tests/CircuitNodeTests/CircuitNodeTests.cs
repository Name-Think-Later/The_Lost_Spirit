using NUnit.Framework;
using Script.TheLostSpirit.CircuitSystem;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.Tests.CircuitNodeTests {
    public class CircuitNodeTest {
        Circuit.SkillNode<Skill> _skillNodeLeft, _skillNodeRight;

        [SetUp]
        public void Setup() {
            _skillNodeLeft  = new Circuit.SkillNode<Skill>(null);
            _skillNodeRight = new Circuit.SkillNode<Skill>(null);
        }

        [Test(Description = "不指定支度正確地建立一個節點")]
        public void Node_DefaultInitialize_ShouldHave_DefaultNumberOfAdjacency() {
            const int defaultAdjacencyCount = 2;

            Circuit.SkillNode<Skill> skillNode = new Circuit.SkillNode<Skill>(null);
            Assert.AreEqual(skillNode.Adjacencies.Count, defaultAdjacencyCount);
        }

        [Test(Description = "指定支度正確地建立一個節點")]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(99)]
        public void Node_InitializeWithCount_ShouldHave_CorrectNumberOfAdjacency(int count) {
            Circuit.SkillNode<Skill> skillNode = new Circuit.SkillNode<Skill>(null, count);
            Assert.AreEqual(skillNode.Adjacencies.Count, count);
        }

        [Test(Description = "建立節點時接口是否正確地建立並初始化")]
        public void Adjacency_InNode_Should_InitializeCorrectly() {
            Circuit.SkillNode<Skill> skillNode = new Circuit.SkillNode<Skill>(null, 3);
            foreach (var adjacency in skillNode.Adjacencies) {
                Assert.NotNull(adjacency);
                Assert.AreEqual(adjacency.Owner, skillNode);
                Assert.IsNull(adjacency.Opposite);
                Assert.AreEqual(adjacency.GetDirection, Circuit.INode.Adjacency.Direction.In);
            }
        }

        [Test(Description = "接口間是否正確地連接")]
        public void Adjacency_Should_ConnectCorrectly() {
            var adjacencyLeft  = _skillNodeLeft.Adjacencies[0];
            var adjacencyRight = _skillNodeRight.Adjacencies[0];
            adjacencyLeft.To(adjacencyRight);

            Assert.AreEqual(adjacencyLeft.Opposite, adjacencyRight);
            Assert.AreEqual(adjacencyRight.Opposite, adjacencyLeft);

            Assert.AreEqual(adjacencyLeft.GetDirection, Circuit.INode.Adjacency.Direction.Out);
            Assert.AreEqual(adjacencyRight.GetDirection, Circuit.INode.Adjacency.Direction.In);
        }

        [Test(Description = "接口間是否正確地切斷")]
        public void Adjacency_Should_DisconnectCorrectly() {
            var adjacencyLeft  = _skillNodeLeft.Adjacencies[0];
            var adjacencyRight = _skillNodeRight.Adjacencies[0];


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
            var adjacencyLeft  = _skillNodeLeft.Adjacencies[0];
            var adjacencyRight = _skillNodeRight.Adjacencies[0];

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
            var adjacencyLeft  = _skillNodeLeft.Adjacencies[0];
            var adjacencyRight = _skillNodeRight.Adjacencies[0];

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