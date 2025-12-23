using System;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Infrastructure.Database;
using UnityEngine;

namespace TheLostSpirit.Context.Formula
{
    [Serializable, FoldoutGroup("Manifestation Context"), HideLabel]
    public class ManifestationContext
    {
        [SerializeField]
        ManifestationDatabase _manifestationDatabase;

        public ManifestationRepository ManifestationRepository { get; private set; }
        public ManifestationViewModelStore ManifestationViewModelStore { get; private set; }
        public ManifestationInstanceFactory ManifestationInstanceFactory { get; private set; }

        public ManifestationDoFrameActionsUseCase ManifestationDoFrameActionsUseCase { get; private set; }
        public ManifestationFinishUseCase ManifestationFinishUseCase { get; private set; }

        public ManifestationContext Construct() {
            ManifestationRepository     = new ManifestationRepository();
            ManifestationViewModelStore = new ManifestationViewModelStore();

            ManifestationDoFrameActionsUseCase = new ManifestationDoFrameActionsUseCase(ManifestationRepository);
            ManifestationFinishUseCase         = new ManifestationFinishUseCase(ManifestationRepository);

            ManifestationInstanceFactory =
                new ManifestationInstanceFactory(
                    _manifestationDatabase,
                    ManifestationDoFrameActionsUseCase,
                    ManifestationFinishUseCase
                );


            return this;
        }
    }
}