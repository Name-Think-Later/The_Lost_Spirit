using System;

namespace TheLostSpirit.Presentation.ViewModel.Formula.OutputHandler {
    public class Once : IOutputHandler {
        public Action OutputAction { get; set; }

        public void HandleOutput() {
            OutputAction();
        }
    }
}