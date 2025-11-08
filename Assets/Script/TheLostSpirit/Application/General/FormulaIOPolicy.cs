using System;
using R3;
using Script.TheLostSpirit.Presentation.ViewModel.Port;
using TheLostSpirit.Domain.Skill.Core;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Application.General
{
    public class FormulaIOPolicy : IFormulaIOPolicy
    {
        readonly IInputPolicy  _inputPolicy;
        readonly IOutputPolicy _outputPolicy;

        Observable<Unit> _observable;

        public NodeID Head { get; }

        public FormulaIOPolicy(
            NodeID        nodeID,
            IInputPolicy  inputPolicy,
            IOutputPolicy outputPolicy
        ) {
            Head          = nodeID;
            _inputPolicy  = inputPolicy;
            _outputPolicy = outputPolicy;
        }

        public IFormulaIOPolicy BindInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        ) {
            _observable = _inputPolicy.ObservableInput(start, perform, cancel);

            return this;
        }

        public IDisposable ToOutput(Action output) {
            return _observable.Subscribe(_ => {
                _outputPolicy.HandleOutput(output);
            });
        }
    }
}