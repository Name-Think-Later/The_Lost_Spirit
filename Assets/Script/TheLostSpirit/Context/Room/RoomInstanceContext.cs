using System.Linq;
using MoreLinq;
using Script.TheLostSpirit.Application.Port.InstanceContext;
using Script.TheLostSpirit.Presentation.ViewModel.Room;
using Script.TheLostSpirit.Presentation.ViewModel.UseCasePort;
using Sirenix.OdinInspector;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;
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
        public IViewModelOnlyID<RoomID> ViewModelOnlyID { get; private set; }
        public IPortalInstanceContext[] Portals => _portalObjectContexts.ToArray<IPortalInstanceContext>();

        public RoomInstanceContext Construct() {
            var roomID = RoomID.New();

            Entity = new RoomEntity(roomID, _mono);

            var viewModel = new RoomViewModel(roomID);

            ViewModelOnlyID = viewModel;

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