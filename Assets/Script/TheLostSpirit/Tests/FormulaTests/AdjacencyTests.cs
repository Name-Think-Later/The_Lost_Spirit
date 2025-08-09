using NSubstitute;
using NUnit.Framework;
using Script.TheLostSpirit.FormulaSystem.NodeModule;

namespace Script.TheLostSpirit.Tests.FormulaTests {
    public class AdjacencyTests {
        Adjacency _left;
        Adjacency _right;

        [SetUp]
        public void SetUp() {
            _left  = new Node(1).Adjacencies[0];
            _right = new Node(1).Adjacencies[0];
        }

        [Test(Description = "接口間是否正確地連接")]
        public void Adjacency_Should_ConnectCorrectly() {
            _left.To(_right);

            Assert.AreEqual(_left.Opposite, _right);
            Assert.AreEqual(_right.Opposite, _left);

            Assert.AreEqual(_left.GetDirection, Direction.Out);
            Assert.AreEqual(_right.GetDirection, Direction.In);
        }

        [Test(Description = "接口間是否正確地切斷")]
        public void Adjacency_Should_DisconnectCorrectly() {
            _left.To(_right);
            _left.Cut();

            Assert.IsNull(_left.Opposite);
            Assert.IsNull(_right.Opposite);


            _left.To(_right);
            _right.Cut();

            Assert.IsNull(_left.Opposite);
            Assert.IsNull(_right.Opposite);
        }

        [Test(Description = "接口是否正確回傳占用狀態")]
        public void Adjacency_ExistStatus_Should_ReturnCorrectly() {
            Assert.IsFalse(_left.IsExist);
            Assert.IsFalse(_right.IsExist);
            Assert.IsTrue(_left.IsEmpty);
            Assert.IsTrue(_right.IsEmpty);

            _left.To(_right);

            Assert.IsTrue(_left.IsExist);
            Assert.IsTrue(_right.IsExist);
            Assert.IsFalse(_left.IsEmpty);
            Assert.IsFalse(_right.IsEmpty);

            _left.Cut();

            Assert.IsFalse(_left.IsExist);
            Assert.IsFalse(_right.IsExist);
            Assert.IsTrue(_left.IsEmpty);
            Assert.IsTrue(_right.IsEmpty);
        }

        [Test(Description = "接口是否正確回傳出入狀態")]
        public void Adjacency_DirectedStatus_Should_ReturnCorrectly() {
            Assert.IsFalse(_left.IsIn);
            Assert.IsFalse(_left.IsOut);
            Assert.IsFalse(_right.IsIn);
            Assert.IsFalse(_right.IsOut);

            _left.To(_right);

            Assert.IsFalse(_left.IsIn);
            Assert.IsTrue(_left.IsOut);
            Assert.IsTrue(_right.IsIn);
            Assert.IsFalse(_right.IsOut);

            _left.Cut();

            Assert.IsFalse(_left.IsIn);
            Assert.IsFalse(_left.IsOut);
            Assert.IsFalse(_right.IsIn);
            Assert.IsFalse(_right.IsOut);
        }
    }
}