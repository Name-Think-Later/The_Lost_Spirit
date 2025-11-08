using System;
using R3;
using TheLostSpirit.Identify;

namespace Script.TheLostSpirit.Presentation.ViewModel.Port
{
    public interface IFormulaIOPolicy
    {
        NodeID Head { get; }

        IFormulaIOPolicy BindInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        );

        IDisposable ToOutput(Action output);
    }
}