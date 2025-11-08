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
        public TraverseNodeUseCase TraverseNodeUseCase { get; private set; }
        public CreateSkillUseCase CreateSkillUseCase { get; private set; }
        public NodeContainSkillUseCase NodeContainSkillUseCase { get; private set; }
        public FormulaAddNodeUseCase FormulaAddNodeUseCase { get; private set; }


        public FormulaContext Construct(UserInputContext userInputContext) {
            FormulaRepository     = new FormulaRepository();
            FormulaViewModelStore = new FormulaViewModelStore();
            NodeRepository        = new NodeRepository();
            SkillRepository       = new SkillRepository();
            SkillFactory          = new SkillFactory(_skillFactoryConfig);

            CreateNodeUseCase       = new CreateNodeUseCase(NodeRepository);
            ConnectNodeUseCase      = new ConnectNodeUseCase(NodeRepository);
            TraverseNodeUseCase     = new TraverseNodeUseCase(NodeRepository, FormulaRepository);
            CreateSkillUseCase      = new CreateSkillUseCase(SkillFactory, SkillRepository);
            NodeContainSkillUseCase = new NodeContainSkillUseCase(NodeRepository);
            FormulaAddNodeUseCase   = new FormulaAddNodeUseCase(FormulaRepository);

            _ = new FormulaAddedCoreNodeEventHandler(SkillRepository, FormulaViewModelStore);


            var formulaInputViews = userInputContext.GeneralInputView.Formulas;

            for (int i = 0; i < _formulaCount; i++) {
                var formulaID = FormulaID.New();

                var formulaEntity = new FormulaEntity(formulaID);
                FormulaRepository.Save(formulaEntity);

                var formulaViewModel = new FormulaViewModel(formulaID, TraverseNodeUseCase);
                FormulaViewModelStore.Save(formulaViewModel);

                formulaInputViews[i].Bind(formulaViewModel);
            }


            return this;
        }
    }
}