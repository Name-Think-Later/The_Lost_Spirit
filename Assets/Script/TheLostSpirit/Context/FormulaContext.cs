using MoreLinq;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.View.Input;
using TheLostSpirit.Presentation.ViewModel.Formula;
using UnityEngine;

namespace TheLostSpirit.Context
{
    public class FormulaContext : MonoBehaviour
    {
        const int _formulaCount = 5;
        public FormulaInputViewModelStore FormulaInputViewModelStore { get; private set; }


        public FormulaContext Construct() {
            FormulaInputViewModelStore = new FormulaInputViewModelStore();

            for (int i = 0; i < _formulaCount; i++) {
                var formulaID             = new FormulaID();
                var formulaInputViewModel = new FormulaInputViewModel(formulaID);
                FormulaInputViewModelStore.Save(formulaInputViewModel);
            }

            return this;
        }
    }
}