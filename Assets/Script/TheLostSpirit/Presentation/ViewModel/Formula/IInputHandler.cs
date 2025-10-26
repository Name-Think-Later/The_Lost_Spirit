using R3;

namespace TheLostSpirit.Presentation.ViewModel.Formula {
    public interface IInputHandler {
        Observable<Unit> ObservableInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        );
    }
}