using R3;

namespace TheLostSpirit.Presentation.ViewModel.Formula.InputHandler {
    public class SingleClick : IInputHandler {
        public Observable<Unit> ObservableInput(
            Observable<Unit> start,
            Observable<Unit> perform,
            Observable<Unit> cancel
        ) {
            return perform;
        }
    }
}