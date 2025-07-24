using System.Collections.Generic;
using Script.TheLostSpirit.SkillSystem.SkillBase;

namespace Script.TheLostSpirit.CircuitSystem {
    public partial class Circuit : IList<Circuit.INode> {
        public partial interface INode {
            public Skill Skill { get; }
            public Circuit.INode.AdjacencyList Adjacencies { get; }
            public void Activate();
        }
    }
}