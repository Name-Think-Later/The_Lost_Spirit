using System;
using R3;
using Script.TheLostSpirit.Domain.ViewModelPort;
using TheLostSpirit.Domain.Skill.Core;

namespace TheLostSpirit.Domain.Formula
{
    public class FormulaIOPolicy : IFormulaIOPolicy
    {
        readonly IInputPolicy  _inputPolicy;
        readonly IOutputPolicy _outputPolicy;

        Observable<Unit> _observable;

        public FormulaIOPolicy(
            IInputPolicy  inputPolicy,
            IOutputPolicy outputPolicy
        ) {
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