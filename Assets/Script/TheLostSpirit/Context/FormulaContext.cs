using MoreLinq;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.View.Input;
using TheLostSpirit.Presentation.ViewModel.Formula;
using UnityEngine;

namespace TheLostSpirit.Context {
    public class FormulaContext : MonoBehaviour {
        public FormulaViewModelStore FormulaViewModelStore { get; private set; }

        public void Construct(GeneralInputView generalInputView) {
            FormulaViewModelStore = new FormulaViewModelStore();

            var formulaInputViews = generalInputView.Formulas;
            formulaInputViews.ForEach(view => {
                var id = new FormulaID();

                var viewModel = new FormulaViewModel(id);
                FormulaViewModelStore.Save(viewModel);

                view.Bind(viewModel);
            });
        }
    }
}