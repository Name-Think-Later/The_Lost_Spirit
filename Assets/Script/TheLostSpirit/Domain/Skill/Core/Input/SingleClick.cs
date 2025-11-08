using R3;

namespace TheLostSpirit.Domain.Skill.Core.Input {
    public class SingleClick : IInputPolicy {
        public Observable<Unit> ObservableInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        ) {
            return perform;
        }
    }
}