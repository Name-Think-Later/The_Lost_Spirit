using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Script.TheLostSpirit.FormulaSystem.NodeModule;

namespace Script.TheLostSpirit.Tests.FormulaTests {
    public class NodeTests {
        [Test]
        public async Task Should_Traversal_With_DeepFirst() {
            var nodeCount = 7;

            var sequence = new Queue<int>();

            var nodes =
                Enumerable
                    .Range(0, nodeCount)
                    .Select(index => {
                        var fakeNode = Substitute.ForPartsOf<Node>(3);
                        fakeNode
                            .When(async node => await node.AsyncVisited())
                            .Do(c => sequence.Enqueue(index));

                        return fakeNode;
                    })
                    .ToArray();
            
            /*
             *            ↱ n4
             *   ↱ n1 ➝ n3
             * n0         ↳ n5 ➝ n6
             *   ↳ n2
             */
            nodes[0].Adjacencies[0].To(nodes[1].Adjacencies[0]);
            nodes[0].Adjacencies[1].To(nodes[2].Adjacencies[0]);

            nodes[1].Adjacencies[1].To(nodes[3].Adjacencies[0]);

            nodes[3].Adjacencies[1].To(nodes[4].Adjacencies[0]);
            nodes[3].Adjacencies[2].To(nodes[5].Adjacencies[0]);

            nodes[5].Adjacencies[1].To(nodes[6].Adjacencies[0]);


            await nodes[0].AsyncVisited();

            var excepted = new int[] { 0, 1, 3, 4, 5, 6, 2 };
            Assert.AreEqual(excepted, sequence);
        }
    }
}