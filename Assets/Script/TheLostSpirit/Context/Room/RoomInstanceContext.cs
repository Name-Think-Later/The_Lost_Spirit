using System.Linq;
using MoreLinq;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.Port.InstanceContext.InstanceContext;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identity;
using TheLostSpirit.Identity.EntityID;
using TheLostSpirit.Infrastructure;
using TheLostSpirit.Infrastructure.Domain.EntityMono;
using TheLostSpirit.Presentation.ViewModel.Port.ViewModelReference;
using TheLostSpirit.Presentation.ViewModel.Room;
using UnityEngine;
using ZLinq;

namespace TheLostSpirit.Context.Room
{
    public class RoomInstanceContext : MonoBehaviour, IRoomInstanceContext

    {
        [SerializeField]
        RoomMono _mono;

        [SerializeField, DisableIn(PrefabKind.All)]
        PortalInstanceContext[] _portalObjectContexts;


        public RoomEntity Entity { get; private set; }
        public IViewModelReference<RoomID> ViewModelReference { get; private set; }
        public IPortalInstanceContext[] Portals => _portalObjectContexts.ToArray<IPortalInstanceContext>();

        public RoomInstanceContext Construct() {
            var roomID = RoomID.New();

            Entity = new RoomEntity(roomID, _mono);

            var viewModel = new RoomViewModel(roomID);

            ViewModelReference = viewModel;

            _portalObjectContexts.ForEach(ctx => ctx.Construct());

            return this;
        }


#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), DisableInPlayMode]
        void AutoGetPortals() {
            _portalObjectContexts = transform.Children().OfComponent<PortalInstanceContext>().ToArray();
        }
#endif
    }
}