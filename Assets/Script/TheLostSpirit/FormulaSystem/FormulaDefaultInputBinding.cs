using System;

namespace Script.TheLostSpirit.FormulaSystem {
    public class FormulaDefaultInputBinding {
        readonly ActionMap.GeneralActions _general;
        readonly Formula[]                _formulas;

        IDisposable _disposable;

        public FormulaDefaultInputBinding(
            ActionMap.GeneralActions general,
            Formula[]                formulas
        ) {
            _general  = general;
            _formulas = formulas;

            ApplyFormulaInputs();
        }

        void ApplyFormulaInputs() {
            _formulas[0].SetDefaultActiveInput(_general.FirstCircuit);
            _formulas[1].SetDefaultActiveInput(_general.SecondCircuit);
        }
    }
}