using TheLostSpirit.Application.Repository;
using TheLostSpirit.Application.ViewModelStore;
using TheLostSpirit.Identify;
using TheLostSpirit.Context.Room;
using TheLostSpirit.Domain.Portal;
using TheLostSpirit.Domain.Room;
using TheLostSpirit.ViewModel.Portal;
using TheLostSpirit.ViewModel.Room;
using UnityEngine;

namespace TheLostSpirit.Factory {
    public class RoomFactory : IRoomFactory {
        readonly RoomFactoryConfig    _config;
        readonly RoomRepository       _roomRepository;
        readonly RoomViewModelStore   _roomViewModelStore;
        readonly PortalRepository     _portalRepository;
        readonly PortalViewModelStore _portalViewModelStore;

        int _counter = 0;

        public RoomFactory(
            RoomFactoryConfig    config,
            RoomRepository       roomRepository,
            RoomViewModelStore   roomViewModelStore,
            PortalRepository     portalRepository,
            PortalViewModelStore portalViewModelStore
        ) {
            _config               = config;
            _roomRepository       = roomRepository;
            _roomViewModelStore   = roomViewModelStore;
            _portalRepository     = portalRepository;
            _portalViewModelStore = portalViewModelStore;
        }

        public RoomID CreateRandom() {
            var length = _config.RoomPattern.Length;
            var index  = Random.Range(0, length);
            var target = _config.RoomPattern[index];

            var parent = _config.RoomDrawingGrid.transform;

            var offset   = _config.Offset;
            var position = Vector2.right * offset * _counter;

            var instance = Object.Instantiate(target, position, Quaternion.identity, parent);

            instance.Initialize(
                _roomRepository,
                _roomViewModelStore,
                _portalRepository,
                _portalViewModelStore
            );

            _counter++;

            return instance.Produce();
        }

        public void ResetCounter() {
            _counter = 0;
        }
    }
}