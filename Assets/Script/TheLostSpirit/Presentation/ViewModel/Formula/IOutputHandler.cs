using System;

namespace TheLostSpirit.Presentation.ViewModel.Formula {
    public interface IOutputHandler {
        Action OutputAction { set; }
        public void HandleOutput();
    }
}