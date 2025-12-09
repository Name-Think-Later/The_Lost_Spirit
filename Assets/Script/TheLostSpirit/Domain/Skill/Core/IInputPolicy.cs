using R3;

namespace TheLostSpirit.Domain.Skill.Core
{
    public interface IInputPolicy
    {
        Observable<Unit> ObservableInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        );
    }
}