using System.Linq;
using MoreLinq;
using Script.TheLostSpirit.Application.ObjectContextContract;
using Sirenix.OdinInspector;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;
using TheLostSpirit.Presentation.IDOnlyViewModel;
using TheLostSpirit.Presentation.ViewModel.Room;
using UnityEngine;
using ZLinq;

namespace TheLostSpirit.Context.Room
{
    public class RoomObjectContext : MonoBehaviour, IRoomObjectContext

    {
        [SerializeField]
        RoomMono _mono;

        [SerializeField, DisableIn(PrefabKind.All)]
        PortalObjectContext[] _portalObjectContexts;


        public RoomEntity Entity { get; private set; }
        public IViewModelOnlyID<RoomID> ViewModelOnlyID { get; private set; }
        public IPortalObjectContext[] Portals => _portalObjectContexts.ToArray<IPortalObjectContext>();

        public RoomObjectContext Instantiate() {
            var roomID = new RoomID();

            Entity = new RoomEntity(roomID, _mono);

            var viewModel = new RoomViewModel(roomID);

            ViewModelOnlyID = viewModel;

            _portalObjectContexts.ForEach(ctx => ctx.Instantiate());

            return this;
        }


#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), DisableInPlayMode]
        void AutoGetPortals() {
            _portalObjectContexts = transform.Children().OfComponent<PortalObjectContext>().ToArray();
        }
#endif
    }
}