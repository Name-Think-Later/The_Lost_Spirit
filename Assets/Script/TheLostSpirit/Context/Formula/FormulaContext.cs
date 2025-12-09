using TheLostSpirit.Application.EventHandler.Formula;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Presentation.ViewModel.Formula;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    public class FormulaContext : MonoBehaviour
    {
        const int _formulaCount = 2;

        [SerializeField]
        public NodeContext nodeContext;

        [SerializeField]
        public SkillContext skillContext;

        public FormulaRepository FormulaRepository { get; private set; }
        public FormulaViewModelStore FormulaViewModelStore { get; private set; }

        public TraverseFormulaUseCase FormulaTraverseUseCase { get; private set; }
        public FormulaAddNodeUseCase FormulaAddNodeUseCase { get; private set; }


        public FormulaContext Construct(UserInputContext userInputContext) {
            skillContext.Construct();
            nodeContext.Construct(skillContext);

            FormulaRepository     = new FormulaRepository();
            FormulaViewModelStore = new FormulaViewModelStore();

            FormulaTraverseUseCase = new TraverseFormulaUseCase(FormulaRepository);
            FormulaAddNodeUseCase  = new FormulaAddNodeUseCase(FormulaRepository, nodeContext.NodeRepository);

            _ = new FormulaAddedCoreNodeEventHandler(skillContext.SkillRepository, FormulaViewModelStore);

            var formulaInputViews = userInputContext.GeneralInputView.Formulas;

            for (var i = 0; i < _formulaCount; i++) {
                var formulaID = FormulaID.New();

                var formulaEntity = new FormulaEntity(formulaID);
                FormulaRepository.Save(formulaEntity);

                var formulaViewModel =
                    new FormulaViewModel(
                        formulaID,
                        FormulaTraverseUseCase
                    );

                FormulaViewModelStore.Save(formulaViewModel);

                formulaInputViews[i].Bind(formulaViewModel);
            }

            return this;
        }
    }
}