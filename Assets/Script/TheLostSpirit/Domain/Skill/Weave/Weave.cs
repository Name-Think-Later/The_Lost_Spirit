using Codice.Client.BaseCommands;
using TheLostSpirit.Domain.Formula.Node;
using UnityEngine;

namespace TheLostSpirit.Domain.Skill.Weave
{
    public class Weave
    {
        readonly WeaveConfig _config;

        int  _count;
        bool _isPass = true;
    
        public TraversalPolicy TraversalPolicy => _config.TraversalPolicy;

        public Weave(WeaveConfig config) {
            _config = config;
        }

        public bool Verify() {
            var gate = _config.Gate;
            if (gate.Use) {
                if (!VerifyGate(gate)) return false;
            }

            return true;
        }

        bool VerifyGate(Gate gate) {
            var threshold = _isPass ? gate.PassTime : gate.BlockTime;
            if (_count == threshold) {
                _isPass = !_isPass;
                _count  = 0;
            }

            _count++;

            return _isPass;
        }
    }
}