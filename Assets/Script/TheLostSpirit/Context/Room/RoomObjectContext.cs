using MoreLinq;
using Sirenix.OdinInspector;
using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.Identify;
using TheLostSpirit.ViewModel.Portal;
using TheLostSpirit.ViewModel.Room;
using UnityEngine;
using ZLinq;

namespace TheLostSpirit.Context.Room {
    public class RoomObjectContext : MonoBehaviour, IObjectContext<RoomID> {
        [SerializeField]
        RoomMono _mono;

        [SerializeField, DisableIn(PrefabKind.All)]
        PortalObjectContext[] _portalSubContexts;


        RoomRepository     _roomRepository;
        RoomViewModelStore _roomViewModelStore;

        PortalRepository     _portalRepository;
        PortalViewModelStore _portalViewModelStore;

        public void Initialize(
            RoomRepository       roomRepository,
            RoomViewModelStore   roomViewModelStore,
            PortalRepository     portalRepository,
            PortalViewModelStore portalViewModelStore
        ) {
            _roomRepository       = roomRepository;
            _roomViewModelStore   = roomViewModelStore;
            _portalRepository     = portalRepository;
            _portalViewModelStore = portalViewModelStore;
        }

        public RoomID Produce() {
            var id = new RoomID();

            var roomEntity = new RoomEntity(id, _mono);
            _roomRepository.Add(roomEntity);

            var roomViewModel = new RoomViewModel(id);
            _roomViewModelStore.Add(roomViewModel);


            _portalSubContexts.ForEach(context => {
                context.Construct(_portalRepository, _portalViewModelStore);
                var portalID = context.Produce();

                roomEntity.IncludePortal(portalID);
            });

            return id;
        }

#if UNITY_EDITOR
        [Button(ButtonSizes.Medium), DisableInPlayMode]
        void AutoGetPortals() {
            _portalSubContexts = transform.Children().OfComponent<PortalObjectContext>().ToArray();
        }
#endif
    }
}