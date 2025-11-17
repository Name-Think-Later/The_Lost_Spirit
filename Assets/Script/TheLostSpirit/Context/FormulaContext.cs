using Script.TheLostSpirit.Presentation.ViewModel.Formula;
using TheLostSpirit.Application.EventHandler.Formula;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Domain.Formula;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Identify;
using UnityEngine;

namespace TheLostSpirit.Context
{
    public class FormulaContext : MonoBehaviour
    {
        [SerializeField]
        SkillFactoryConfig _skillFactoryConfig;

        const int _formulaCount = 2;
        public FormulaRepository FormulaRepository { get; private set; }
        public FormulaViewModelStore FormulaViewModelStore { get; private set; }
        public NodeRepository NodeRepository { get; private set; }
        public SkillRepository SkillRepository { get; private set; }
        public SkillFactory SkillFactory { get; private set; }

        public CreateNodeUseCase CreateNodeUseCase { get; private set; }
        public ConnectNodeUseCase ConnectNodeUseCase { get; private set; }
        public TraverseFormulaUseCase TraverseFormulaUseCase { get; private set; }
        public CreateSkillUseCase CreateSkillUseCase { get; private set; }
        public NodeContainSkillUseCase NodeContainSkillUseCase { get; private set; }
        public AddNodeInFormulaUseCase AddNodeInFormulaUseCase { get; private set; }


        public FormulaContext Construct(UserInputContext userInputContext) {
            FormulaRepository     = new FormulaRepository();
            FormulaViewModelStore = new FormulaViewModelStore();
            NodeRepository        = new NodeRepository();
            SkillRepository       = new SkillRepository();
            SkillFactory          = new SkillFactory(_skillFactoryConfig);

            CreateNodeUseCase       = new CreateNodeUseCase(NodeRepository);
            ConnectNodeUseCase      = new ConnectNodeUseCase(NodeRepository);
            TraverseFormulaUseCase  = new TraverseFormulaUseCase(FormulaRepository);
            CreateSkillUseCase      = new CreateSkillUseCase(SkillFactory, SkillRepository);
            NodeContainSkillUseCase = new NodeContainSkillUseCase(NodeRepository);
            AddNodeInFormulaUseCase = new AddNodeInFormulaUseCase(FormulaRepository, NodeRepository);

            _ = new AddedCoreNodeInFormulaEventHandler(SkillRepository, FormulaViewModelStore);
            _ = new VisitedNodeEventHandler(NodeRepository);

            var formulaInputViews = userInputContext.GeneralInputView.Formulas;

            for (int i = 0; i < _formulaCount; i++) {
                var formulaID = FormulaID.New();

                var formulaEntity = new FormulaEntity(formulaID);
                FormulaRepository.Save(formulaEntity);

                var formulaViewModel = new FormulaViewModel(formulaID, TraverseFormulaUseCase);
                FormulaViewModelStore.Save(formulaViewModel);

                formulaInputViews[i].Bind(formulaViewModel);
            }


            return this;
        }
    }
}