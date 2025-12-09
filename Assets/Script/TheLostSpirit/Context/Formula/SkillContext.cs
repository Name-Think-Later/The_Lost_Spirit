using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.EventHandler.Formula;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Context.Anchor;
using TheLostSpirit.Domain.Skill;
using TheLostSpirit.Infrastructure.Database;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    [Serializable, FoldoutGroup("Skill Context"), HideLabel]
    public class SkillContext
    {
        [SerializeField, Indent]
        SkillDatabase _skillDatabase;

        [SerializeField, AssetSelector, Indent]
        AnchorInstanceContext _anchorInstanceContext;

        [SerializeField, Indent]
        public ManifestationContext manifestationContext;

        public SkillRepository SkillRepository { get; private set; }
        public SkillFactory SkillFactory { get; private set; }
        public AnchorRepository AnchorRepository { get; private set; }
        public AnchorViewModelStore AnchorViewModelStore { get; private set; }
        public AnchorInstanceFactory AnchorInstanceFactory { get; private set; }


        public ActiveSkillUseCase ActiveSkillUseCase { get; private set; }
        public CreateSkillUseCase CreateSkillUseCase { get; private set; }
        public CreateAnchorUseCase CreateAnchorUseCase { get; private set; }

        public SkillContext Construct() {
            manifestationContext.Construct();

            SkillRepository       = new SkillRepository();
            SkillFactory          = new SkillFactory(_skillDatabase);
            AnchorRepository      = new AnchorRepository();
            AnchorViewModelStore  = new AnchorViewModelStore();
            AnchorInstanceFactory = new AnchorInstanceFactory(_anchorInstanceContext);

            ActiveSkillUseCase = new ActiveSkillUseCase(SkillRepository);
            CreateSkillUseCase = new CreateSkillUseCase(SkillFactory, SkillRepository);
            CreateAnchorUseCase =
                new CreateAnchorUseCase(
                    AnchorInstanceFactory,
                    AnchorRepository,
                    AnchorViewModelStore
                );


            _ = new AsyncCoreActivatedEventHandler(CreateAnchorUseCase, AnchorRepository);

            _ =
                new AsyncManifestActivatedEventHandler(
                    AnchorRepository,
                    manifestationContext.ManifestationRepository,
                    manifestationContext.ManifestationViewModelStore,
                    manifestationContext.ManifestationInstanceFactory
                );

            return this;
        }
    }
}