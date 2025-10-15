using System;
using System.Collections.Generic;
using TheLostSpirit.Identify;
using TheLostSpirit.Infrastructure.Domain;

namespace TheLostSpirit.Domain.Room {
    public class RoomEntity : IEntity<RoomID>, IDisposable {
        readonly Room      _room;
        readonly IRoomMono _roomMono;

        public RoomID ID { get; }

        public IReadOnlyList<PortalID> IncludedPortal => _room.IncludedPortal;
        public IReadOnlyList<PortalID> AvailablePortal => _room.AvailablePortal;
        public IReadOnlyList<PortalID> UnavailablePortal => _room.UnavailablePortal;

        public RoomEntity(RoomID id, IRoomMono mono) {
            ID = id;

            _room = new Room();

            _roomMono = mono;
            _roomMono.Initialize(ID);
        }


        public void IncludePortal(PortalID portal) {
            _room.AvailablePortal.Add(portal);
        }

        public void TurnPortalToUnavailable(PortalID portal) {
            _room.AvailablePortal.Remove(portal);
            _room.UnavailablePortal.Add(portal);
        }

        public void TurnPortalToAvailable(PortalID portal) {
            _room.UnavailablePortal.Remove(portal);
            _room.AvailablePortal.Add(portal);
        }

        public void Associate(RoomID other) {
            _room.AssociatedRoom.Add(other);
        }

        public bool IsAssociate(RoomID other) {
            return _room.AssociatedRoom.Contains(other);
        }

        public void Dispose() {
            _roomMono.Destroy();
        }
    }
}