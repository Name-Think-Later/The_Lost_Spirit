using System;
using R3;

namespace Script.TheLostSpirit.Domain.ViewModelPort
{
    public interface IFormulaIOPolicy
    {
        IFormulaIOPolicy BindInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        );

        IDisposable ToOutput(Action output);
    }
}