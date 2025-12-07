using System;
using R3;

namespace TheLostSpirit.Domain.Port.FormulaIOPolicy
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